using AssetManagement.Api.Extentions;
using AssetManagement.Api.Filters;
using AssetManagement.Api.Settings;
using AssetManagement.Application.Interfaces.Auth;
using AssetManagement.Application.Interfaces.User;
using AssetManagement.Application.Services.Auth;
using AssetManagement.Application.Validators.Accounts;
using AssetManagement.Application.Services.User;
using AssetManagement.Core.Interfaces.Services.Auth;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using AssetManagement.Application.Interfaces;
using AssetManagement.Application.Services;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
var appSettings = new AppSettings();
configuration.Bind(appSettings);

/// Add services to the container.

// Add Swagger
builder.Services.AddCustomSwagger();

// Add Infrastructure
builder.Services.AddAssetInfrastructure(opt =>
    configuration.GetSection("InfrastructureSettings").Bind(opt));

// Add Services
builder.Services.AddScoped<ITokenService, JwtService>();
builder.Services.AddScoped<IIdentityService, IdentityService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IReturningRequestService, ReturningRequestService>();

builder.Services.AddScoped<IAssetService, AssetService>();

// Disable automatic model state error response
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

// Add Controller and Validation filters
builder.Services.AddControllers(options =>
{
    options.Filters.Add<ValidationFilter>();
});
builder.Services.AddValidatorsFromAssembly(typeof(ChangePasswordRequestValidator).Assembly);

// Bind settings
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));
var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>();
var key = Encoding.UTF8.GetBytes(jwtSettings.Key);

builder.Services.AddHttpContextAccessor();

// Enable CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("https://localhost:3000", "https://localhost:3001")
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
    });
});

// Exception handling and authentication
builder.Services.AddGlobalExceptionHandling();
builder.Services.AddCustomJwtAuthentication(jwtSettings);


/// App configuration
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Asset Management API v1");
        c.DocumentTitle = "Asset Management API";
        c.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();

app.UseExceptionHandler();

app.UseAuthentication();

app.UseAuthorization();

app.UseCors();

app.MapControllers();

app.Run();
