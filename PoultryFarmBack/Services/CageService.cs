using PoultryFarmBack.Models;
using PoultryFarmBack.Services;
using System.Collections.Generic;
using System.Linq;

public class CageService
{
    private readonly JsonFileService _jsonFileService;
    private List<Cage> _cages;

    public CageService(JsonFileService jsonFileService)
    {
        _jsonFileService = jsonFileService;
        LoadData();
    }

    private void LoadData()
    {
        _cages = _jsonFileService.ReadFromFile<Cage>("cages");
    }

    private void SaveData()
    {
        _jsonFileService.WriteToFile(_cages, "cages");
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
