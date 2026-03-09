using Microsoft.OpenApi.Models;

namespace Backend.Extensions
{
    public static class SwaggerExtensions
    {
        public static IServiceCollection AddSwaggerServices(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Snackstasy API",
                    Version = "v1",
                    Description = "API for managing user accounts and balances in the Snackstasy application."
                });
            });

            return services;
        }

        public static WebApplication UseSwaggerMiddleware(this WebApplication app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Snackstasy API v1");
                options.RoutePrefix = "swagger";
                options.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.List);
            });

            return app;
        }
    }
}
