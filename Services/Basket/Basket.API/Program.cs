using Basket.API;
using Basket.API.Infrastructure;
using Basket.API.Protos;
using Basket.API.Repository;
using EventBus;
using Infrastructure;
using Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;
using Quartz;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//builder.Services.Configure<MongoDbSetting>(builder.Configuration.GetSection("MongoDb"));
builder.Services.AddScoped<ICartRepository, CartRepository>();
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(typeof(Program).Assembly);
});
//builder.Services.AddMessageBroker(builder.Configuration, typeof(Program).Assembly);
builder.Services.AddAutoMapper(typeof(Program).Assembly);
builder.Services.AddDbContext<CartDbContext>(option =>
{
    option.UseNpgsql(builder.Configuration.GetConnectionString("Postgres"));
});

//builder.Services.AddQuartz(config =>
//{
//    var jobKey = new JobKey(nameof(ProcessOutBoxMessageJob));
//    config.AddJob<ProcessOutBoxMessageJob>(jobKey)
//        .AddTrigger(
//            trigger => trigger.ForJob(jobKey)
//                .WithSimpleSchedule(schedule => schedule.WithIntervalInSeconds(1).RepeatForever()));
//});
//builder.Services.AddQuartzHostedService();
builder.Services.AddMessageBroker(builder.Configuration, Assembly.GetExecutingAssembly());
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", builder =>
         builder.AllowAnyOrigin()
                 .AllowAnyMethod()
                 .AllowAnyHeader());
});

builder.Services.AddGrpc();
builder.Services.AddGrpcClient<CourseManagerProtoService.CourseManagerProtoServiceClient>(options =>
{
    options.Address = new Uri(builder.Configuration["GrpcSettings:CourseUri"]);
});

builder.Services.AddGrpcClient<PaymentServiceProtoGrpc.PaymentServiceProtoGrpcClient>(options =>
{
    options.Address = new Uri(builder.Configuration["GrpcSettings:PaymentServiceUri"]);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("CorsPolicy");
app.MigrateDatabase<CartDbContext>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
