using PoultryFarmBack.Models;
using PoultryFarmBack.Services;
using System;
using System.Collections.Generic;
using System.Linq;

public class EmployeeService
{
    private readonly JsonFileService _jsonFileService;
    private List<Employee> _employees;
    private List<Cage> _cages;


    public EmployeeService(JsonFileService jsonFileService)
    {
        _jsonFileService = jsonFileService;
        LoadData();
    }

    private void LoadData()
    {
        _employees = _jsonFileService.ReadFromFile<Employee>("employee");
        _cages = _jsonFileService.ReadFromFile<Cage>("cages");

        foreach (var employee in _employees)
        {
            employee.Cages = new List<Cage>();

            foreach (var cage in _cages)
            {
                if (cage.EmployeeId == employee.Id)
                {
                    employee.Cages.Add(cage);
                    Console.WriteLine($"Клетка {cage.Id} привязана к сотруднику {employee.Id}");
                }
                
            }
        }
    }



    private void SaveData()
    {
        _jsonFileService.WriteToFile(_employees, "employee");
    }

    public List<Employee> GetAll()
    {
        return _employees;
    }

    public Employee GetById(int id)
    {
        return _employees.FirstOrDefault(c => c.Id == id);
    }

    public bool Create(Employee employee, out Dictionary<string, string> errors)
    {
        errors = ValidateEmployee(employee);
        if (errors.Count > 0)
            return false;

        employee.Id = _employees.Any() ? _employees.Max(e => e.Id) + 1 : 1;
        _employees.Add(employee);
        SaveData();
        return true;
    }

    public bool Update(Employee updatedEmployee, out Dictionary<string, string> errors)
    {
        var existingEmployee = GetById(updatedEmployee.Id);
        if (existingEmployee == null)
        {
            errors = new Dictionary<string, string> { { "id", "Сотрудник с таким ID не найден." } };
            return false;
        }

        errors = ValidateEmployee(updatedEmployee);
        if (errors.Count > 0)
            return false;

        existingEmployee.FullName = updatedEmployee.FullName;
        existingEmployee.Salary = updatedEmployee.Salary;
        // Добавьте обновление других полей при необходимости

        SaveData();
        return true;
    }

    public bool Delete(int id, out Dictionary<string, string> errors)
    {
        errors = new Dictionary<string, string>();
        var employee = GetById(id);
        if (employee != null)
        {
            _employees.Remove(employee);
            SaveData();
            return true;
        }
        else
        {
            errors["id"] = "Сотрудник с таким ID не найден.";
            return false;
        }
    }

    public Dictionary<string, string> ValidateEmployee(Employee employee)
    {
        var errors = new Dictionary<string, string>();

        if (string.IsNullOrWhiteSpace(employee.FullName))
        {
            errors["fullName"] = "Поле 'FullName' обязательно для заполнения.";
        }

        if (employee.Salary <= 0)
        {
            errors["salary"] = "Зарплата должна быть больше 0.";
        }

        return errors;
    }
}
