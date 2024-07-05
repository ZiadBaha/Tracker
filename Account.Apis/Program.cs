using Account.Apis.Errors;
using Account.Apis.Extentions;
using Account.Core.Models.Account;
using Account.Core.Models.Identity;
using Account.Reposatory.Data;
using Account.Reposatory.Data.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using System.Text.Json.Serialization;

namespace Account.Apis
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            #region configure service

            builder.Services.AddControllers();
            builder.Services.Configure<MailSettings>(builder.Configuration.GetSection(nameof(MailSettings)));

            builder.Services.AddIdentityServices(builder.Configuration);

            builder.Services.AddSwaggerService();
            builder.Services.AddAplictionService();
            builder.Services.AddMemoryCache();


            builder.Services.AddDbContext<StoreContext>(Options =>
            
                Options.UseSqlServer(builder.Configuration.GetConnectionString("Defaultconnection"))
                       .EnableSensitiveDataLogging()  // Enable sensitive data logging
                       .LogTo(Console.WriteLine, LogLevel.Information));
        
            builder.Services.AddAplictionService();
             


            #endregion
            var app = builder.Build();
         


            #region Update automatically
            // Create a service scope to resolve services
            using var scope = app.Services.CreateScope();
            var Services = scope.ServiceProvider;

            // Obtain logger factory to create loggers
            var loggerfactory = Services.GetRequiredService<ILoggerFactory>();
            try
            {
                // Get the database context for Identity
                var identityDbContext = Services.GetRequiredService<AppIdentityDbContext>();

                // Apply database migration asynchronously
                await identityDbContext.Database.MigrateAsync();

                // Get the UserManager service to manage users
                var usermanager = Services.GetRequiredService<UserManager<AppUser>>();

                // Seed initial user data for the Identity context
                //await AppIdentityDbContextSeed.SeedUserAsync(usermanager);
            }
            catch (Exception ex)
            {
                // If an exception occurs during migration or seeding, log the error
                var logger = loggerfactory.CreateLogger<Program>();
                logger.LogError(ex, "An Error Occurred During Applying The Migrations");
            }
            #endregion


            #region configure middlewares

            // Configure the HTTP request pipeline.

            app.UseHttpsRedirection();
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                       Path.Combine(builder.Environment.ContentRootPath, "Images")),
                RequestPath = "/Resources"
            });
            app.UseCors();

            /////////////////

            app.UseStatusCodePagesWithReExecute("/errors/{0}");

            app.UseMiddleware<ExeptionMiddleWares>();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwaggerMiddlewares();
            }
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();
            app.UseHttpsRedirection();
            app.UseForwardedHeaders();
            #endregion
            app.Run();
        }
    }
}
