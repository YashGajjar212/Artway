using Artway.Application.Interfaces.Customers;
using Artway.Application.Services.Customers;
using Artway.Database.DBContext;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<RouteOptions>(options => options.LowercaseUrls = true);

// Add services to the container.

builder.Services.AddTransient<ICustomerServices, CustomerServices>();

builder.Services.AddDbContext<ArtwayContext>(options => 
    options.UseSqlServer(builder.Configuration.GetConnectionString("ArtwayDatabase")));

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

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
