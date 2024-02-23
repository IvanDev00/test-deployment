using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace APIBoilerplate.Common.Middleware
{
    #region keycloakAuth
    public static class SwaggerConfig
    {

        public static void ConfigureAddAuthentication(JwtBearerOptions options, IConfiguration configuration)
        {
            options.Authority = configuration["KEYCLOAK_URL"]; // your Keycloak address
            options.Audience = "admin-cli";
            options.RequireHttpsMetadata = false;

            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
            };

        }

        public static void ConfigureSwaggerGen(SwaggerGenOptions options, IConfiguration configuration)
        {
            // Path for public key
            var xmlPath = configuration["XML_COMMENTS_PATH"];

            options.IncludeXmlComments(xmlPath);

            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Enter The Keycloak token ",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>() // Empty string array
                }
            });
            
            options.SwaggerDoc("v1", new OpenApiInfo() {Title = "Riskwatch Search API", Version = "v1"});
        }
    }
    #endregion 
}
