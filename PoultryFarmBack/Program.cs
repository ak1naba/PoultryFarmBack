using PoultryFarmBack.Services;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Автоматическая регистрация всех сервисов
var services = Assembly.GetExecutingAssembly()
    .GetTypes()
    .Where(t => t.IsClass && !t.IsAbstract && t.Namespace == "PoultryFarmBack.Services");

foreach (var service in services)
{
    builder.Services.AddScoped(service);
}

// Настройка CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", policy =>
    {
        policy.AllowAnyOrigin() // Разрешить запросы с любого источника
              .AllowAnyMethod() // Разрешить любые HTTP-методы (GET, POST, PUT и т.д.)
              .AllowAnyHeader(); // Разрешить любые заголовки
    });
});

var app = builder.Build();

// Использование CORS
app.UseCors("AllowAllOrigins");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
