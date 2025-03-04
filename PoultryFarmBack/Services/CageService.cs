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
            Console.WriteLine(foundEmployee.FullName);
            
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

}
