﻿using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace AssetManagement.Api.Extensions
{
    public class SwaggerResponseFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            operation.Responses.Add("401", new OpenApiResponse
            {
                Description = "Unauthorized - Missing or invalid JWT token"
            });
            operation.Responses.Add("403", new OpenApiResponse
            {
                Description = "Forbidden - User lacks required role"
            });
            operation.Responses.Add("404", new OpenApiResponse
            {
                Description = "Not Found - Resource does not exist"
            });
        }
    }
}
