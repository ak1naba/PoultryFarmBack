using PoultryFarmBack.Models;
using System.Collections.Generic;
using System.Linq;

namespace PoultryFarmBack.Services
{
    public class ChickenService
    {
        private readonly JsonFileService _jsonFileService;
        private List<Chicken> _chickens;

        public ChickenService(JsonFileService jsonFileService)
        {
            _jsonFileService = jsonFileService;
            LoadData();
        }

        // Загрузка данных из файла
        private void LoadData()
        {
            _chickens = _jsonFileService.ReadFromFile<Chicken>("chickens");
        }

        // Сохранение данных в файл
        private void SaveData()
        {
            _jsonFileService.WriteToFile(_chickens, "chickens");
        }

        public List<Chicken> GetAllChickens()
        {
            return _chickens;
        }

        public Chicken GetChickenById(int id)
        {
            return _chickens.FirstOrDefault(c => c.Id == id);
        }

        public void AddChicken(Chicken chicken)
        {
            chicken.Id = _chickens.Any() ? _chickens.Max(c => c.Id) + 1 : 1;
            _chickens.Add(chicken);
            SaveData(); // Сохраняем данные после добавления
        }

        public void UpdateChicken(Chicken chicken)
        {
            var existingChicken = _chickens.FirstOrDefault(c => c.Id == chicken.Id);
            if (existingChicken != null)
            {
                existingChicken.Weight = chicken.Weight;
                existingChicken.Age = chicken.Age;
                existingChicken.EggsPerMonth = chicken.EggsPerMonth;
                existingChicken.BreedId = chicken.BreedId;
                existingChicken.CageId = chicken.CageId;
                SaveData(); // Сохраняем данные после обновления
            }
        }

        public void DeleteChicken(int id)
        {
            var chicken = _chickens.FirstOrDefault(c => c.Id == id);
            if (chicken != null)
            {
                _chickens.Remove(chicken);
                SaveData(); // Сохраняем данные после удаления
            }
        }
    }
}
