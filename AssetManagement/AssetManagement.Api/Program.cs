using Microsoft.AspNetCore.Mvc;

namespace AssetManagement.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var configuration = builder.Configuration;

        // Add Swagger
        builder.Services.AddCustomSwagger();

        // Add Infrastructure
        builder.Services.AddAssetInfrastructure(opt => 
            configuration.GetSection("InfrastructureSettings").Bind(opt));

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

        builder.Services.AddHttpContextAccessor();

        // Enable CORS
        builder.Services.AddCors(options =>
        {
            options.AddDefaultPolicy(policy =>
            {
                policy.WithOrigins("https://ntg-asset-management.vercel.app")
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials();
            });
        });

        // Exception handling and authentication
        builder.Services.AddGlobalExceptionHandling();
        builder.Services.AddCustomJwtAuthentication(configuration);

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

        app.UseCors();

        app.UseAuthentication();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}