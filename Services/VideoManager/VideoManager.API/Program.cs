using Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using VideoManager.API.Application.Services;
using VideoManager.Domain.Contracts;
using VideoManager.Infrastructure;
using VideoManager.Infrastructure.Repositories;
using EventBus;
using System.Reflection;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.AspNetCore.Http.Features;
using System.Net;
using VideoManager.API.Application.GrpcServices;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//builder.Services.AddDbContext<VideoManagerContext>(opt =>
//{
//    opt.UseNpgsql(builder.Configuration.GetConnectionString("Postgres"), 
//        b => b.MigrationsAssembly(typeof(VideoManagerContext).Assembly.ToString()));
//});
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(typeof(Program).Assembly);
});
builder.Services.AddScoped<IFileDownloader, FileDownloader>();
//builder.Services.AddScoped<ILocalStorageRepository, LocalStorageRepository>();
builder.Services.AddInfrastructureService(builder.Configuration);

builder.Services.AddMessageBroker(builder.Configuration, Assembly.GetExecutingAssembly());
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", builder =>
         builder.AllowAnyOrigin()
                 .AllowAnyMethod()
                 .AllowAnyHeader()
                 .WithExposedHeaders("Content-Disposition")
                 .SetPreflightMaxAge(TimeSpan.FromMinutes(10)));
});
//builder.Services.Configure<IISServerOptions>(options =>
//{
//    options.MaxRequestBodySize = 52428800;
//});
builder.Services.Configure<KestrelServerOptions>(options =>
{
    options.Limits.MaxRequestBodySize = long.MaxValue;
    options.Limits.KeepAliveTimeout = TimeSpan.FromMinutes(15);
    options.Limits.RequestHeadersTimeout = TimeSpan.FromMinutes(10);
});
builder.Services.Configure<FormOptions>(options =>
{
    options.ValueLengthLimit = int.MaxValue;
    options.MultipartBodyLengthLimit = long.MaxValue;
});
builder.WebHost.ConfigureKestrel(options =>
{
    options.Listen(IPAddress.Any, 8082, listenOptions =>
    {
        listenOptions.Protocols = HttpProtocols.Http2;
    });
    options.Listen(IPAddress.Any, 8080, listenOptions =>
    {
        listenOptions.Protocols = HttpProtocols.Http1;
    });
});
builder.Services.AddGrpc();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("CorsPolicy");
app.MigrateDatabase<VideoManagerContext>();
app.UseHttpsRedirection();

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(builder.Environment.ContentRootPath, "storage")
    ),
    RequestPath = "/storage",
    ContentTypeProvider = new FileExtensionContentTypeProvider
    {
        Mappings =
        {
            [".m3u8"] = "application/vnd.apple.mpegurl"
        }
    }
});

app.UseAuthorization();

app.MapControllers();
app.MapGrpcService<VideoManagerRpcService>();

app.Run();
