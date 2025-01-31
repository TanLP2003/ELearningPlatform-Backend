using EventBus;
using Microsoft.EntityFrameworkCore;
using WishList.API.Application.Services;
using WishList.API.Protos;
using WishList.API.Repository;

namespace WishList.API.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureServiceDependency(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMessageBroker(configuration, typeof(Program).Assembly);
            services.AddDbContext<WishListContext>(option =>
            {
                option.UseNpgsql(configuration.GetConnectionString("Postgres"));
            });
            services.AddScoped<ILoveListRepository, LoveListRepository>();
            services.AddScoped<ILoveListService, LoveListService>();
            services.AddGrpcClient<CourseManagerProtoService.CourseManagerProtoServiceClient>(option =>
            {
                option.Address = new Uri(configuration["GrpcSettings:CourseUri"]);
            });
                //.ConfigurePrimaryHttpMessageHandler(() =>
                //{
                //    var handler = new HttpClientHandler
                //    {
                //        ServerCertificateCustomValidationCallback =
                //             HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
                //    };
                //    return handler;
                //});
        }
    }
}
