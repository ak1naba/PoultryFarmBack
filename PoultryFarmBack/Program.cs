using PoultryFarmBack.Services;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Swagger/OpenAPI настройка
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Пример явной регистрации ChickenService
builder.Services.AddScoped<ChickenService>();
builder.Services.AddScoped<CageService>();
builder.Services.AddScoped<BreedService>();

// Явная регистрация JsonFileService с передачей строки
builder.Services.AddSingleton<JsonFileService>(provider =>
    new JsonFileService("/app/Data/farm-data.json"));

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
