using EventBus;
using PaymentService.API.Applications.Services;
using PaymentService.API.InterbankSubsystem.VNPay;
using System.Reflection;

namespace PaymentService.API.Extensions
{
    public static class ServiceExtensions
    {
        public static void AddServiceDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder =>
                    builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
            });
            services.AddScoped<IPaymentService, VNPayService>();
            services.AddGrpc();
            services.AddMessageBroker(configuration, Assembly.GetExecutingAssembly());
        }
    }
}
