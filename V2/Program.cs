using Microsoft.OpenApi.Models;
using V2.Services;

var builder = WebApplication.CreateBuilder(args);

//Вывод лога в консоль
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "DefaultApiTemplate",
        Version = "v1"
    });
});

//Singelton... для имитации бд...
builder.Services.AddSingleton<IDoSomethingService, DoSomethingService>();

builder.Services.AddHostedService<DoSomethingService>();

var app = builder.Build();

// swagger/index.html
app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "DefaultApiTemplate v1"));


app.UseSwagger();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseAuthorization();

app.MapControllers();

app.Run();
