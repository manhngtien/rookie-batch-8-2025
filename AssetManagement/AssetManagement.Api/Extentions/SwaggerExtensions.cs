using Microsoft.OpenApi.Models;

namespace AssetManagement.Api.Extentions
{
    public static class SwaggerExtensions
    {
        public static IServiceCollection AddCustomSwagger(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer()
            .AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "Asset Management API", Version = "v1" });

                // Document common responses
                options.OperationFilter<SwaggerResponseFilter>();
            });

            return services;
        }
    }
}
