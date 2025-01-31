using Microsoft.AspNetCore.Server.Kestrel.Core;
using PaymentService.API.Applications.GrpcServices;
using PaymentService.API.Extensions;
using System.Net;
using VNPAY.NET;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddServiceDependencies(builder.Configuration);
builder.Services.AddSingleton<IVnpay, Vnpay>();

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
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("CorsPolicy");
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapGrpcService<PaymentServiceGrpc>();
app.MapControllers();

app.Run();
