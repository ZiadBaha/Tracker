namespace Account.Apis.Extentions
{
    public static class SwaggerServiceExtention
    {
        public static IServiceCollection AddSwaggerService(this IServiceCollection service)
        {
            // Add API Explorer services
            service.AddEndpointsApiExplorer();

            // Add Swagger generator
            service.AddSwaggerGen();

            // Return the updated service collection
            return service;
        }
        public static WebApplication UseSwaggerMiddlewares(this WebApplication app)
        {
            // Enable Swagger middleware
            app.UseSwagger();

            // Enable Swagger UI middleware
            app.UseSwaggerUI();

            // Return the updated application
            return app;
        }

    }
}
