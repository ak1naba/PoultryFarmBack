using PoultryFarmBack.Services;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Swagger/OpenAPI ���������
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ������ ����� ����������� ChickenService
builder.Services.AddScoped<ChickenService>();
builder.Services.AddScoped<CageService>();
builder.Services.AddScoped<BreedService>();
builder.Services.AddScoped<EmployeeService>();

// ����� ����������� JsonFileService � ��������� ������
builder.Services.AddSingleton<JsonFileService>(provider =>
    new JsonFileService("/app/Data/farm-data.json"));

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
