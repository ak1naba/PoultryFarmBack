using PoultryFarmBack.Services;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// �������������� ����������� ���� ��������
var services = Assembly.GetExecutingAssembly()
    .GetTypes()
    .Where(t => t.IsClass && !t.IsAbstract && t.Namespace == "PoultryFarmBack.Services");

foreach (var service in services)
{
    // ����������� ���� ��������, ����� JsonFileService, ������� ������� �������������� �����������
    if (service != typeof(JsonFileService))
    {
        builder.Services.AddScoped(service);
    }
}

// ����� ����������� JsonFileService � ��������� ������
builder.Services.AddSingleton<JsonFileService>(provider =>
    new JsonFileService("Data/farm-data.json"));

// ��������� CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", policy =>
    {
        policy.AllowAnyOrigin() // ��������� ������� � ������ ���������
              .AllowAnyMethod() // ��������� ����� HTTP-������ (GET, POST, PUT � �.�.)
              .AllowAnyHeader(); // ��������� ����� ���������
    });
});

var app = builder.Build();

// ������������� CORS
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
