using ApiGateway.Configurations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Yarp.ReverseProxy.Transforms;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var secretKey = builder.Configuration.GetSection("Security")["SecretKey"];
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

builder.Services.AddReverseProxy()
    //.LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"))
    .LoadFromMemory(YarpConfig.GetRoutes(builder.Configuration), YarpConfig.GetClusters(builder.Configuration))
    .AddTransforms(builderContext =>
    {
        if (!string.IsNullOrEmpty(builderContext.Route.AuthorizationPolicy))
        {
            builderContext.AddRequestTransform(async transformContext =>
            {
                var context = transformContext.HttpContext;
                if (context.User.Identity.IsAuthenticated)
                {
                    var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                    Console.WriteLine($"From api gateway: {userId}");
                    transformContext.ProxyRequest.Headers.Add("X-User-Id", userId);
                    //Console.WriteLine($"{JsonConvert.SerializeObject(transformContext.ProxyRequest.Headers, Formatting.Indented)}");
                    //Console.WriteLine($"HttpContext: {JsonConvert.SerializeObject(context, Formatting.Indented)}");
                    //Console.WriteLine($"ProxyRequest: {JsonConvert.SerializeObject(transformContext.ProxyRequest, Formatting.Indented)}");
                }
            });
        }
    });

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", builder =>
         builder.AllowAnyOrigin()
                 .AllowAnyMethod()
                 .AllowAnyHeader());
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("CorsPolicy");
//app.UseHttpLogging();
app.UseAuthentication();
app.UseAuthorization();

app.MapReverseProxy();

//app.Use(async (context, next) =>
//{
//    var options = new JsonSerializerOptions
//    {
//        ReferenceHandler = ReferenceHandler.Preserve
//    };
//    var userJson = JsonSerializer.Serialize(context.User, options);
//    Console.WriteLine(userJson);
//    Console.WriteLine(context.User.Identity.IsAuthenticated);
//    if (context.User.Identity.IsAuthenticated)
//    {
//        var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
//        Console.WriteLine($"From api gateway : {userId}");
//        if (!string.IsNullOrEmpty(userId))
//        {
//            context.Request.Headers["X-User-Id"] = userId;
//        }
//        Console.WriteLine(context.Request.Headers);
//    }

//    await next.Invoke();
//});

app.MapGet("/hello", () =>
{
    return Results.Ok("Hello, World!");
})
.WithName("HelloEndpoint")
.WithOpenApi();

app.Run();