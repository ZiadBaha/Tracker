using Account.Core.Models.Identity;
using Account.Core.Services;
using Account.Reposatory.Data.Identity;
using Account.Reposatory.Reposatories;
using Account.services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel;
using System.Text;

namespace Account.Apis.Extentions
{
    public static class IdentityServicesExtentions
    {
        public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Add identity services with specified user and role classes
            services.AddIdentity<AppUser, IdentityRole>(Options =>
            {
            })
            // Configure identity to use Entity Framework stores
            .AddEntityFrameworkStores<AppIdentityDbContext>()
            .AddDefaultTokenProviders().AddRoles<IdentityRole>();

            // Add authentication services
            services.AddAuthentication(Options =>
            {
                // Set default authentication scheme to JWT bearer authentication
                Options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                Options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            // Configure JWT bearer authentication
            .AddJwtBearer(Options =>
            {
                Options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidIssuer = configuration["JWT:ValidIssuer"],
                    ValidateAudience = true,
                    ValidAudience = configuration["JWT:ValidAudience"],
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:key"]))
                };
            });

            services.AddDbContext<AppIdentityDbContext>(Options =>
            {
                Options.UseSqlServer(configuration.GetConnectionString("IdentityConnections"));
            });

            // Register custom token service
            services.AddScoped<IAccountService,AccountService>();
            services.AddScoped<IOtpService, OtpService>();
            services.AddScoped<ITokenService,TokenService>();


            services.AddScoped<IAdminRepository, AdminRepository>();




            // Add here any other injections.....
            return services;
        }



    }
}
