using Infrastructure.Extensions;
using SearchService.API.Extensions;
using SearchService.API.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

//builder.AddGraphQL().AddTypes();
builder.Services.AddServiceDependency(builder.Configuration);
builder.Services
    .AddGraphQLServer()
    .AddTypes();

var app = builder.Build();
app.UseCors("CorsPolicy");
app.MigrateDatabase<ApplicationContext>();
app.MapGraphQL();

app.RunWithGraphQLCommands(args);
