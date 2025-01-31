using Auth.API.Infrastructure;
using Auth.API.Infrastructure.Repositories;
using Auth.API.Services;
using Auth.API.Utils;
using EventBus;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Reflection;
using System.Text;

namespace Auth.API.Extensions
{
    public static class ServiceExtensions
    {
        public static void AddServicesDependency(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder =>
                     builder.AllowAnyOrigin()
                             .AllowAnyMethod()
                             .AllowAnyHeader());
            });

            services.AddDbContext<ApplicationContext>(option =>
            {
                option.UseNpgsql(configuration.GetConnectionString("Postgres"));
            });

            services.AddScoped<IAuthRepo, AuthRepo>();

            var secretKey = configuration.GetSection("Security")["SecretKey"];
            var encodedKey = Encoding.UTF8.GetBytes(secretKey);
            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(options =>
                {
                    options.Events = new JwtBearerEvents
                    {
                        OnAuthenticationFailed = context =>
                        {
                            if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                            {
                                context.Response.Headers.Add("IS_TOKEN_EXPIRED", "true");
                            }
                            return Task.CompletedTask;  
                        },
                    };
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateAudience = false,
                        ValidateIssuer = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(encodedKey)
                    };
                });

            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IAuthRepo, AuthRepo>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddMessageBroker(configuration, Assembly.GetExecutingAssembly());
        }

        public static void UseSerilogExtensions(this IHostBuilder builder)
        {
            builder.UseSerilog((context, configuration) =>
            {
                //configuration.WriteTo.Console();
                configuration.WriteTo.Http("http://logstash:5044", 50000000);
                configuration.ReadFrom.Configuration(context.Configuration);
            });
        }
    }
}
