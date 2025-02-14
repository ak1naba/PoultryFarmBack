using PoultryFarmBack.Models;
using PoultryFarmBack.Services;

public class ChickenService
{
    private readonly JsonFileService _jsonFileService;
    private List<Chicken> _chickens;
    private List<Breed> _breeds;
    private List<Cage> _cages;

    public ChickenService(JsonFileService jsonFileService)
    {
        _jsonFileService = jsonFileService;
        LoadData();
    }

    public Dictionary<string, string> ValidateChicken(Chicken chicken)
    {
        var errors = new Dictionary<string, string>();

        if (chicken.Weight <= 0)
            errors["weight"] = "Вес курицы должен быть положительным числом.";
        if (chicken.Age < 0)
            errors["age"] = "Возраст курицы не может быть отрицательным.";
        if (chicken.EggsPerMonth < 0)
            errors["eggsPerMonth"] = "Количество яиц в месяц не может быть отрицательным.";
        if (!_breeds.Any(b => b.Id == chicken.BreedId))
            errors["breedId"] = "Указанная порода не существует.";
        if (!_cages.Any(c => c.Id == chicken.CageId))
            errors["cageId"] = "Указанная клетка не существует.";

        return errors;
    }

    // Загрузка данных из файла
    private void LoadData()
    {
        _chickens = _jsonFileService.ReadFromFile<Chicken>("chickens");
        _breeds = _jsonFileService.ReadFromFile<Breed>("breeds");
        _cages = _jsonFileService.ReadFromFile<Cage>("cages");

        // Связываем кур с их породами и клетками, но не сохраняем в файл их объекты
        foreach (var chicken in _chickens)
        {
            // Вместо сохранения полного объекта, загружаем только ID для Breed и Cage
            chicken.Breed = _breeds.FirstOrDefault(b => b.Id == chicken.BreedId);
            chicken.Cage = _cages.FirstOrDefault(c => c.Id == chicken.CageId);
        }
    }

    // Сохранение данных в файл
    private void SaveData()
    {
        var chickensToSave = _chickens.Select(chicken => new
        {
            chicken.Id,
            chicken.Weight,
            chicken.Age,
            chicken.EggsPerMonth,
            chicken.BreedId,  // Только идентификатор породы
            chicken.CageId    // Только идентификатор клетки
        }).ToList();

        // Сохраняем этот список в файл
        _jsonFileService.WriteToFile(chickensToSave, "chickens");
    }

    public List<Chicken> GetAllChickens()
    {
        return _chickens;
    }

    public Chicken GetChickenById(int id)
    {
        return _chickens.FirstOrDefault(c => c.Id == id);
    }

    public bool AddChicken(Chicken chicken, out Dictionary<string, string> errors)
    {
        errors = ValidateChicken(chicken);
        if (errors.Count > 0)
            return false;

        // Заполняем полные объекты Breed и Cage перед добавлением
        chicken.Breed = _breedService.GetById(chicken.BreedId);
        chicken.Cage = _cageService.GetById(chicken.CageId);

        chicken.Id = _chickens.Any() ? _chickens.Max(c => c.Id) + 1 : 1;
        _chickens.Add(chicken);
        SaveData();
        return true;
    }


    public bool UpdateChicken(Chicken chicken, out Dictionary<string, string> errors)
    {
        var existingChicken = _chickens.FirstOrDefault(c => c.Id == chicken.Id);
        if (existingChicken == null)
        {
            errors = new Dictionary<string, string> { { "id", "Курица с таким ID не найдена." } };
            return false;
        }

        errors = ValidateChicken(chicken);
        if (errors.Count > 0)
            return false;

        existingChicken.Weight = chicken.Weight;
        existingChicken.Age = chicken.Age;
        existingChicken.EggsPerMonth = chicken.EggsPerMonth;
        existingChicken.BreedId = chicken.BreedId;
        existingChicken.CageId = chicken.CageId;
        chicken.Breed = _breedService.GetById(chicken.BreedId);
        chicken.Cage = _cageService.GetById(chicken.CageId);
        SaveData();
        return true;
    }

    public void DeleteChicken(int id)
    {
        var chicken = _chickens.FirstOrDefault(c => c.Id == id);
        if (chicken != null)
        {
            _chickens.Remove(chicken);
            SaveData();
        }
    }
}
