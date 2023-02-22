using V2.Services;

var builder = WebApplication.CreateBuilder(args);

//Вывод лога в консоль
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Singelton... для имитации бд...
builder.Services.AddSingleton<IDoSomethingService, DoSomethingService>();

builder.Services.AddHostedService<DoSomethingService>();

var app = builder.Build();

// swagger/index.html
app.UseSwaggerUI();
app.UseSwagger();


app.UseSwagger(x => x.SerializeAsV2 = true);

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseAuthorization();

app.MapControllers();

app.Run();
