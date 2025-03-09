using PoultryFarmBack.Models;
using PoultryFarmBack.Services;
using System.Collections.Generic;
using System.Linq;

public class CageService
{
    private readonly JsonFileService _jsonFileService;
    private readonly EmployeeService _employeeService;

    private List<Cage> _cages;
    private List<Employee> _employees;

    public CageService(JsonFileService jsonFileService, EmployeeService employeeService)
    {
        _jsonFileService = jsonFileService;
        _employeeService = employeeService;
        LoadData();
    }

    private void LoadData()
    {
        _cages = _jsonFileService.ReadFromFile<Cage>("cages");
        _employees = _employeeService.GetAll();

        foreach (var cage in _cages)
        {
            var foundEmployee = _employees.FirstOrDefault(b => b.Id == cage.EmployeeId);
            
            cage.Employee = foundEmployee;
        }

    }

    private void SaveData()
    {
        var cagesToSave = _cages.Select(cage => new
        {
            cage.Id,
            cage.IsOccupied,
            cage.ChickenId,
            cage.EmployeeId,
        }).ToList();

        // Сохраняем этот список в файл
        _jsonFileService.WriteToFile(cagesToSave, "cages");
    }

    public List<Cage> GetAll()
    {
        return _cages;
    }

    public Cage GetById(int id)
    {
        return _cages.FirstOrDefault(c => c.Id == id);
    }

    public void AssignChickenToCage(int cageId, int chickenId)
    {
        var cage = _cages.FirstOrDefault(c => c.Id == cageId);
        if (cage != null && !cage.IsOccupied)
        {
            cage.ChickenId = chickenId;
            cage.IsOccupied = true;
            cage.EmployeeId = cage.EmployeeId;
            SaveData();
        }
    }

    public void RemoveChickenFromCage(int cageId)
    {
        var cage = _cages.FirstOrDefault(c => c.Id == cageId);
        if (cage != null && cage.IsOccupied)
        {
            cage.ChickenId = null;
            cage.IsOccupied = false;
            cage.EmployeeId = cage.EmployeeId;
            SaveData();
        }
    }

    public Dictionary<string, string> ValidateCage(Cage cage)
    {
        var errors = new Dictionary<string, string>();

        if (!_employees.Any(e => e.Id == cage.EmployeeId))
        {
            errors["employeeId"] = "Указанный сотрудник не существует.";
        }

        return errors;
    }

    public bool AddCage(Cage cage, out Dictionary<string, string> errors){
        errors = ValidateCage(cage);
        if (errors.Count > 0)
            return false;
        
        cage.Id = _cages.Any() ? _cages.Max(c => c.Id) + 1 : 1;
        cage.IsOccupied = false;
        cage.ChickenId = null;

        _cages.Add(cage);

        SaveData();
        return true;
    }

    public bool UpdateCage(Cage cage, out Dictionary<string, string> errors)
    {
        var existingCage = _cages.FirstOrDefault(c => c.Id == cage.Id);
        if (existingCage == null)
        {
            errors = new Dictionary<string, string> { { "id", "Клетка с таким ID не найдена." } };
            return false;
        }

        errors = ValidateCage(cage);
        if (errors.Count > 0)
            return false;

        existingCage.EmployeeId = cage.EmployeeId;

        SaveData();
        return true;
    }
    
    public bool DeleteCage(int id, out Dictionary<string, string> errors)
    {
        errors = new Dictionary<string, string>();
        var cage = _cages.FirstOrDefault(c => c.Id == id);
        if (cage != null)
        {
            if (!cage.IsOccupied)
            {
                _cages.Remove(cage);
                SaveData();
                return true;
            }
            else
            {
                errors["id"] = "Клетка занята и не может быть удалена.";
                return false;
            }
        }
        else
        {
            errors["id"] = "Клетка с таким ID не найдена.";
            return false;
        }
    }


}
