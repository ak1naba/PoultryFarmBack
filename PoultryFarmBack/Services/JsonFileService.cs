using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace PoultryFarmBack.Services
{
    public class JsonFileService
    {
        private readonly string _filePath;

        public JsonFileService(string filePath)
        {
            _filePath = filePath;
        }

        // Метод для чтения данных из файла и десериализации конкретных сущностей
        public List<T> ReadFromFile<T>(string entityName)
        {
            if (!File.Exists(_filePath))
            {
                return new List<T>();
            }

            try
            {
                var json = File.ReadAllText(_filePath);
                var jsonObject = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(json);

                if (jsonObject != null && jsonObject.ContainsKey(entityName))
                {
                    var entityJson = jsonObject[entityName].ToString();
                    return JsonSerializer.Deserialize<List<T>>(entityJson) ?? new List<T>();
                }

                return new List<T>();
            }
            catch (JsonException ex)
            {
                // Логирование ошибок десериализации
                Console.WriteLine($"Ошибка десериализации: {ex.Message}");
                return new List<T>();
            }
        }

        // Метод для записи данных в файл
        public void WriteToFile<T>(List<T> data, string entityName)
        {
            Dictionary<string, object> allEntities;

            if (File.Exists(_filePath))
            {
                var json = File.ReadAllText(_filePath);
                allEntities = JsonSerializer.Deserialize<Dictionary<string, object>>(json) ?? new Dictionary<string, object>();
            }
            else
            {
                allEntities = new Dictionary<string, object>();
            }

            // Добавляем/обновляем сущность
            allEntities[entityName] = data;

            var options = new JsonSerializerOptions { WriteIndented = true };
            var jsonToWrite = JsonSerializer.Serialize(allEntities, options);
            File.WriteAllText(_filePath, jsonToWrite);
        }
    }
}
