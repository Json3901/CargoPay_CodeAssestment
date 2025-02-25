using System.Text;
using CargoPay.Application.Interfaces;
using CargoPay.Application.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;

namespace CargoPay.Application;

public static class ExtensionService
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services = services.AddJwtAuthenticacion();
        services = services.AddSwagger();
        
        services.AddTransient<IUserService, UserService>();
        services.AddHostedService<PaymentFeeService>();
        services.AddScoped<ICardService, CardService>();
        services.AddScoped<IPaymentService, PaymentService>();

        return services;
    }

    private static IServiceCollection AddSwagger(this IServiceCollection services)
    {
        var openApi = new OpenApiInfo
        {
            Title = "Cargo Pay API",
            Version = "v1",
            Description = "API for Cargo Payments"
        };

        services.AddSwaggerGen(x =>
        {
            openApi.Version = "v1";
            x.SwaggerDoc("v1", openApi);

            var securityScheme = new OpenApiSecurityScheme
            {
                Description = "JWT Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                Reference = new OpenApiReference
                {
                    Id = JwtBearerDefaults.AuthenticationScheme,
                    Type = ReferenceType.SecurityScheme
                }
            };
            x.AddSecurityDefinition(securityScheme.Reference.Id, securityScheme);
            x.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                { securityScheme, new string[] { } }
            });
        });

        return services;
    }

    private static IServiceCollection AddJwtAuthenticacion(this IServiceCollection services)
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = "C4rg0P4y",
                    ValidAudience = "C4rg0P4y",
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("p2l+HnBQzJ6RQKHcJv3cVJTR1qOpe45i3Qf1tD+xE6XzYp2W1X5PrRjNw3Z5VgPvO93IvP7RHF2HZ/wXlg5HtQ=="))
                };
            });

        return services;
    }
}