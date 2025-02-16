using PoultryFarmBack.Models;
using PoultryFarmBack.Services;
using System.Collections.Generic;
using System.Linq;

public class BreedService
{
    private readonly JsonFileService _jsonFileService;
    private List<Breed> _breeds;

    public BreedService(JsonFileService jsonFileService)
    {
        _jsonFileService = jsonFileService;
        LoadData();
    }

    private void LoadData()
    {
        _breeds = _jsonFileService.ReadFromFile<Breed>("breeds");
    }

    private void SaveData()
    {
        _jsonFileService.WriteToFile(_breeds, "breeds");
    }

    public List<Breed> GetAll()
    {
        return _breeds;
    }

    public Breed GetById(int id)
    {
        return _breeds.FirstOrDefault(b => b.Id == id);
    }
}
