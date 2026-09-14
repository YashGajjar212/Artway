using Artway.Application.Interfaces.Authentication;
using Artway.Application.Interfaces.Customers;
using Artway.Application.Interfaces.Token;
using Artway.Application.Mappings;
using Artway.Application.Services.Authentication;
using Artway.Application.Services.Customers;
using Artway.Application.Services.Token;
using Artway.Database.DBContext;
using Artway.Infrastructure.Repositories.Customers;
using Artway.Models.Customers;
using Artway.Presentation.ExceptionHandlers;
using Artway.Presentation.Middlewares;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Serilog.Events;
using System.Text;

try
{
    Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.AspNetCore.Hosting.Diagnostics", LogEventLevel.Information) // Required for request logs
    .MinimumLevel.Override("Microsoft.AspNetCore.Routing", LogEventLevel.Information)             // Required for endpoint matching
    .Enrich.FromLogContext()
    .WriteTo.File(@"D:\ArtwayLogs\Generic_Logs\Artway-generic-log-.txt",
    outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] [{CorrelationId}] {Message:lj}{NewLine}{Exception}",
        rollingInterval: RollingInterval.Day,
        fileSizeLimitBytes: 3_145_728,
        rollOnFileSizeLimit: true,
        retainedFileCountLimit: 30)

    // API Request logs
    .WriteTo.Logger(lc => lc
    .Filter.ByIncludingOnly(evt =>
        evt.Properties.TryGetValue("SourceContext", out var ctx) &&
        (ctx.ToString().Contains("RequestLoggingMiddleware") ||
         ctx.ToString().Contains("Microsoft.AspNetCore.Hosting.Diagnostics")))
    .WriteTo.File(@"D:\ArtwayLogs\API_Logs\Artway-api-log-.txt",
    outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] [{CorrelationId}] {Message:lj}{NewLine}{Exception}",
        rollingInterval: RollingInterval.Day,
        fileSizeLimitBytes: 3_145_728,
        rollOnFileSizeLimit: true,
        retainedFileCountLimit: 30))

    // Log Database query logs (EF Core commands)
    .WriteTo.Logger(lc => lc
    .Filter.ByIncludingOnly(evt =>
        evt.Properties.TryGetValue("SourceContext", out var ctx) &&
        ctx.ToString().Contains("Microsoft.EntityFrameworkCore.Database.Command"))
    .WriteTo.File(@"D:\ArtwayLogs\Database_Logs\Artway-database-.txt",
    outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] [{CorrelationId}] {Message:lj}{NewLine}{Exception}",
        rollingInterval: RollingInterval.Day,
        fileSizeLimitBytes: 3_145_728,
        rollOnFileSizeLimit: true,
        retainedFileCountLimit: 30))

    // Log all exceptions in a seperate folder
    .WriteTo.Logger(lc => lc
        .Filter.ByIncludingOnly(evt => evt.Exception != null || evt.Level == LogEventLevel.Error || evt.Level == LogEventLevel.Fatal)
        .WriteTo.File(@"D:\ArtwayLogs\Exception_Logs\Artway-exceptions-.txt",
        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] [{CorrelationId}] {Message:lj}{NewLine}{Exception}",
        rollingInterval: RollingInterval.Day,
        fileSizeLimitBytes: 3_145_728,
        rollOnFileSizeLimit: true,
        retainedFileCountLimit: 30))
    .CreateLogger();


    Log.Information("Starting Artway Application. Let it rip");
    // Create a WebApplicationBuilder object that has two primary jobs
    //  1) Provide app level configuration for all the services like DI, logging, exception handling, etc
    //  2) Provide runtime environment for the application
    var builder = WebApplication.CreateBuilder(args);
    builder.Host.UseSerilog();

    builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
    builder.Services.AddProblemDetails();

    builder.Services.Configure<RouteOptions>(options => options.LowercaseUrls = true);

    // Add services to the container.
    builder.Services.AddScoped<ITokenService, TokenService>();
    builder.Services.AddScoped<ICustomerServices, CustomerServices>();
    builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
    builder.Services.AddScoped<IAuthService, AuthService>();
    builder.Services.AddSingleton<IPasswordHasher<Customer>, PasswordHasher<Customer>>();

    builder.Services.AddDbContext<ArtwayContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("ArtwayDatabase")));

    builder.Services.AddAutoMapper(cfg => { }, typeof(AuthMappingProfile).Assembly);

    builder.Services.AddControllers();

    // JWT Configuration
    // Fetch Jwt_Auth and Secret_Key sections from appSettings.json. Convert secret key to byte array because
    // cryptographic algorithm requires byte array to do its calculations and operations. It can't do it on string
    var jwtSettings = builder.Configuration.GetSection("Jwt_Auth");
    var secretKey = Encoding.UTF8.GetBytes(jwtSettings["Secret_Key"] ?? throw new InvalidOperationException("JWT Key not found"));

    builder.Services.AddAuthentication(options =>
    {
        // By default ASp.NET Core selects Cookie Authentication and so we are saying it to use JWT Authentication
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        // Because it selects Cookie-based Authentication, there the default response is to redirect to login page
        // if there is an authorization error. But in JWT web api we want to pass a clean 401 Unauthorized status code
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                // This is a ON/OFF switch and here we are just telling the framework to validate these 3 things.
                // Actual validation will be done in the ValidIssuer and ValidAudience
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                // ValidateIssuerSigningKey ensures the when we receive the token on subsequent requests we validate 
                // re-calcualte the header+payload and compare the signature with the one that is passed in the token.
                // If they match then the request is valid, and if not then the request is tampared by a bad actor
                ValidateIssuerSigningKey = true,
                // Ensure the Issuer and Audience are the same that we configured in the appSettings. Otherwise anyone can
                // create pass any value
                ValidIssuer = jwtSettings["Issuer"],
                ValidAudience = jwtSettings["Audience"],
                // To compare the new signature and the signature passed int he token we need to pass the
                // secret key(byte array) so it can do the validation
                IssuerSigningKey = new SymmetricSecurityKey(secretKey)
            };
        });

    builder.Services.AddAuthorization();

    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(options =>
    {
        options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
        {
            Name = "Authorization",
            Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
            Scheme = "Bearer",
            BearerFormat = "JWT",
            In = Microsoft.OpenApi.Models.ParameterLocation.Header,
            Description = "Enter 'Bearer' [space] and then your token. \n\nExample: \"Bearer eyJhbGciOiJIUzI1Ni...\""
        });

        options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
    });

    // Above this was all configuration. After the builder.Build(), we can't configure anything to ensure
    // thread-safety. After this we chain the middleware elements in proper order and build the application
    var app = builder.Build();

    app.UseMiddleware<CorrelationIdMiddleware>();
    app.UseExceptionHandler();
    app.UseSerilogRequestLogging();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();
    app.UseAuthentication();
    app.UseAuthorization();
    app.MapControllers();

    // Starts the application on the Kestrel web server. This blocks the main thread and tells the
    // app to listen for incoming HTTP requests until it is shut down.
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application startup failed");
}
finally
{
    Log.CloseAndFlush();
}