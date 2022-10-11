using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace HealthyTasty.Infrastructure.Jwt
{
    public static class JwtExtension
    {
        public const string SectionName = "jwt";

        public static void AddJwt(this IServiceCollection serviceCollection, JwtOptions jwtOptions)
        {
            serviceCollection.AddAuthentication(config =>
            {
                config.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(jwtConfig =>
            {
                jwtConfig.SaveToken = true;
                jwtConfig.RequireHttpsMetadata = false;
                jwtConfig.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtOptions.AccessSecret)),
                    ValidIssuer = jwtOptions.Issuer,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = jwtOptions.Audience,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.FromMinutes(1)
                };
            });

            serviceCollection.AddSingleton(jwtOptions);
            serviceCollection.AddSingleton<IJwtHelper, JwtHelper>();
        }
    }
}
