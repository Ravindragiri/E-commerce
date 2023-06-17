using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SourceFuse.E_commerce.API.Handlers;
using SourceFuse.E_commerce.API.Infrastructure.Managers.Interfaces;
using SourceFuse.E_commerce.Entities.Identity;
using SourceFuse.E_commerce.Persistence.Context;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Text.Json;

namespace SourceFuse.E_commerce.API.Extensions
{
    public static class AuthExtensions
    {
        public static void AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<EcommerceContext>(options => options.UseSqlite(configuration.GetSection("DataSources:SQLite:ConnectionString").Value));
            services.AddScoped<IDbContext>(prov => prov.GetRequiredService<EcommerceContext>());

            //services.AddIdentity<IdentityUser, ApplicationRole>()
            services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
            {
                options.User.RequireUniqueEmail = true;

                // options.SignIn.RequireConfirmedEmail = true;

                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireDigit = false;
                options.Password.RequireUppercase = false;
            })
                .AddEntityFrameworkStores<EcommerceContext>()
            .AddDefaultTokenProviders();

            // ===== Add Jwt Authentication ========
            var issuer = configuration["Jwt:Issuer"];
            var audience = configuration.GetSection("Jwt:Audience").Value;
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear(); // => remove default claims
            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    //options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(cfg =>
                {
                    //cfg.IncludeErrorDetails = true;
                    //cfg.RequireHttpsMetadata = false;
                    //cfg.SaveToken = true;
                    cfg.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidIssuer = issuer, //configuration["Jwt::JwtIssuer"],
                        ValidAudience = audience, //configuration["Jwt::JwtAudience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:key"])),
                        //new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt::JwtKey"])),
                        ValidateIssuerSigningKey = true,
                        ValidateAudience = true,
                        ValidateIssuer = true,
                        ValidateLifetime = true,
                        //ClockSkew = TimeSpan.Zero // remove delay of token when expire
                    };
                    cfg.Events = new JwtBearerEvents()
                    {
                        OnChallenge = context =>
                        {
                            context.HandleResponse();
                            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                            context.Response.ContentType = "application/json";

                            // Ensure we always have an error and error description.
                            if (string.IsNullOrEmpty(context.Error))
                                context.Error = "invalid_token";
                            if (string.IsNullOrEmpty(context.ErrorDescription))
                                context.ErrorDescription = "This request requires a valid JWT access token to be provided";

                            // Add some extra context for expired tokens.
                            if (context.AuthenticateFailure != null && context.AuthenticateFailure.GetType() == typeof(SecurityTokenExpiredException))
                            {
                                var authenticationException = context.AuthenticateFailure as SecurityTokenExpiredException;
                                context.Response.Headers.Add("x-token-expired", authenticationException.Expires.ToString("o"));
                                context.ErrorDescription = $"The token expired on {authenticationException.Expires.ToString("o")}";
                            }

                            return context.Response.WriteAsync(JsonSerializer.Serialize(new
                            {
                                error = context.Error,
                                error_description = context.ErrorDescription
                            }));
                        }
                    };
                });
        }

        public static void AddAppAuthorization(this IServiceCollection services, IConfiguration configuration)
        {
            IConfigurationService settingsService =
                services.BuildServiceProvider().GetRequiredService<IConfigurationService>();

            services.AddAuthorization(options =>
            {
                options.AddPolicy(settingsService.GetManageProductPolicyName(), policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.AddRequirements(
                        new ResourceAuthorizationHandler.AllowedToManageProductRequirement(settingsService
                            .GetWhoIsAllowedToManageProducts()));
                });


                options.AddPolicy(settingsService.GetCreateCommentPolicyName(), policy =>
                {
                    policy.Requirements.Add(
                        new ResourceAuthorizationHandler.AllowedToCreateCommentRequirement(settingsService
                            .GetWhoIsAllowedToCreateComments()));
                });
            });
            // CORS
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", builder =>
                {
                    builder.AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            }
            );
        }
    }
}
