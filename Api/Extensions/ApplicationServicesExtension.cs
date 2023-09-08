using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Api.Helpers;
using Api.Services;
using Aplicacion.UnitOfWork;
using Dominio.Entities;
using Dominio.Interface;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace Api.Extensions;
    public static class ApplicationServicesExtension
    {
        public static void ConfigureCors(this IServiceCollection services) =>
    services.AddCors(options =>
    {
        options.AddPolicy("CorsPolicy", builder => 
        builder.AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader());
    });
    public static void AddAplicacionServices(this IServiceCollection services)
    {
        services.AddScoped<IPasswordHasher<User>,PasswordHasher<User>>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IAuthorizationHandler, GlobalVerbRoleHandler>();
    }
    //METODOS DE EXTENCION, INYECTADO EN EL CONTENEDOR DE DEPEMDENCIAS 
    // ESTE TENDRA LAS CREDENCIALES Y LLAVES PARA EL TOKEN, DEPENDENCIAS DEL TOKEN

    public static void AddJwt(this IServiceCollection services, IConfiguration configuration)
    {
        //configuracion de los appsetting
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(o =>
        {
            o.RequireHttpsMetadata = false;
            o.SaveToken = false;
            o.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                ValidateIssuer= true,
                ValidateAudience= true,
                ValidateLifetime = true,
                ClockSkew= TimeSpan.Zero,
                ValidIssuer = configuration["JWT: Issuer"],
                ValidAudience = configuration["JWT: Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT: Key"]))

            };
        });
    }


    // public static void ConfigureRateLimiting(this IServiceCollection services)
    // {
    //     services.AddMemoryCache();
    //     services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
    //     services.AddInMemoryRateLimiting();
    //     services.Configure<IpRateLimitOptions>(options =>
    //     {
    //         options.EnableEndpointRateLimiting = true;
    //         options.StackBlockedRequests = true;
    //         options.HttpStatusCode =429;
    //         options.RealIpHeader = "X-real-ip";
    //         options.GeneralRules = new List<RateLimitRule>
    //         {
    //             new RateLimitRule
    //             {
    //                 Endpoint = "*",
    //                 Period = "10s",
    //                 Limit = 2
    //             }
    //         };
    //     });
    // }
    // public static void ConfigureApiVersioning(this IServiceCollection services)
    // {
    //     services.AddApiVersioning(options =>
    //     {
    //         options.DefaultApiVersion = new ApiVersion(1, 0);
    //         options.AssumeDefaultVersionWhenUnspecified = true;
    //         options.ApiVersionReader = new QueryStringApiVersionReader("ver");
    //     });
    // }
    }
