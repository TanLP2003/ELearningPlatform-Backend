using CourseManager.API.Applications.GrpcService;
using CourseManager.API.Extensions;
using CourseManager.Infrastructure;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using System.Net;
using Infrastructure.Extensions;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.ConfigureServiceDependency(builder.Configuration);
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
app.MigrateDatabase<CourseDbContext>();
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapGrpcService<CourseGrpcService>();

app.Run();
