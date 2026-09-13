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
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Serilog.Events;
using System.Text;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.AspNetCore.Hosting.Diagnostics", LogEventLevel.Information) // Required for request logs
    .MinimumLevel.Override("Microsoft.AspNetCore.Routing", LogEventLevel.Information)             // Required for endpoint matching
    .Enrich.FromLogContext()

    // Will catch all the general things
    //.Filter.ByExcluding(evt =>
    //    evt.Exception != null ||
    //    (evt.Properties.ContainsKey("SourceContext") && (
    //        evt.Properties["SourceContext"].ToString().Contains("Serilog.AspNetCore.RequestLoggingMiddleware") ||
    //        evt.Properties["SourceContext"].ToString().Contains("Microsoft.EntityFrameworkCore.Database.Command")
    //    ))
    //)
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

try
{
    Log.Information("Starting Artway Application. Let it rip");
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
    var jwtSettings = builder.Configuration.GetSection("Jwt_Auth");
    var secretKey = Encoding.UTF8.GetBytes(jwtSettings["Secret_Key"] ?? throw new InvalidOperationException("JWT Key not found"));

    builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtSettings["Issuer"],
                ValidAudience = jwtSettings["Audience"],
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