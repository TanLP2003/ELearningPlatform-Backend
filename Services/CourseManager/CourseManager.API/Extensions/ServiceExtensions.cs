using CourseManager.Domain.Contracts;
using CourseManager.Infrastructure.Repositories;
using CourseManager.Infrastructure;
using Microsoft.EntityFrameworkCore;
using EventBus;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using CourseManager.API.Protos;

namespace CourseManager.API.Extensions;

public static class ServiceExtensions
{
    public static void ConfigureServiceDependency(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("CorsPolicy", builder =>
                 builder.AllowAnyOrigin()
                        //.AllowCredentials()
                        .AllowAnyHeader()
                        .AllowAnyMethod());
        });
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                var secretKey = configuration.GetSection("Security")["SecretKey"];
                var encodedKey = Encoding.UTF8.GetBytes(secretKey);
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
        var assembly = typeof(Program).Assembly;
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(assembly);
        });
        //services.AddDbContext<CourseDbContext>(options =>
        //{
        //    options.UseNpgsql(configuration.GetConnectionString("Postgres"));
        //});
        services.AddInfrastructureService(configuration);
        services.AddAutoMapper(assembly);
        services.AddMessageBroker(configuration, assembly);
        services.AddGrpc();
        services.AddGrpcClient<VideoManagerProtoService.VideoManagerProtoServiceClient>(options =>
        {
            options.Address = new Uri(configuration["GrpcSettings:VideoManagerUri"]);
        });
        services.AddGrpcClient<LearningServiceProtoGrpc.LearningServiceProtoGrpcClient>(options =>
        {
            options.Address = new Uri(configuration["GrpcSettings:LearningServiceUri"]);
        });
    }
}