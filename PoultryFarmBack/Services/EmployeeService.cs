using PoultryFarmBack.Models;
using PoultryFarmBack.Services;
using System.Collections.Generic;
using System.Linq;

public class EmployeeService
{
    private readonly JsonFileService _jsonFileService;
    private List<Employee> _employees;

    public EmployeeService(JsonFileService jsonFileService)
    {
        _jsonFileService = jsonFileService;
        LoadData();
    }

    private void LoadData()
    {
        _employees = _jsonFileService.ReadFromFile<Employee>("employee");

        foreach (var employee in _employees)
            {
                Console.WriteLine($"Name: {employee.FullName}");
            }
    }

    public List<Employee> GetAll()
    {
        return _employees;
    }

    public Employee GetById(int id)
    {
        return _employees.FirstOrDefault(c => c.Id == id);
    }

    

}
