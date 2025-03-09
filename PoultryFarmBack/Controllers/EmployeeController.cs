using Microsoft.AspNetCore.Mvc;
using PoultryFarmBack.Models;
using PoultryFarmBack.Services;
using System.Collections.Generic;

namespace PoultryFarmBack.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly EmployeeService _employeeService;

        public EmployeeController(EmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        // GET: api/employee
        [HttpGet]
        public IActionResult GetAllEmployees()
        {
            var employees = _employeeService.GetAll();
            return Ok(employees);
        }

        // GET: api/employee/{id}
        [HttpGet("{id}")]
        public IActionResult GetEmployeeById(int id)
        {
            var employee = _employeeService.GetById(id);
            if (employee == null)
            {
                return NotFound(new { message = "Сотрудник не найден." });
            }
            return Ok(employee);
        }

        // POST: api/employee
        [HttpPost]
        public IActionResult CreateEmployee([FromBody] Employee employee)
        {
            if (employee == null)
            {
                return BadRequest(new { message = "Некорректные данные." });
            }

            if (_employeeService.Create(employee, out var errors))
            {
                return CreatedAtAction(nameof(GetEmployeeById), new { id = employee.Id }, employee);
            }

            return UnprocessableEntity(errors);
        }

        // PUT: api/employee/{id}
        [HttpPut("{id}")]
        public IActionResult UpdateEmployee(int id, [FromBody] Employee updatedEmployee)
        {
            if (updatedEmployee == null || updatedEmployee.Id != id)
            {
                return BadRequest(new { message = "Некорректные данные." });
            }

            if (_employeeService.Update(updatedEmployee, out var errors))
            {
                return Ok(updatedEmployee);
            }

            return UnprocessableEntity(errors);
        }

        // DELETE: api/employee/{id}
        [HttpDelete("{id}")]
        public IActionResult DeleteEmployee(int id)
        {
            if (_employeeService.Delete(id, out var errors))
            {
                return NoContent();
            }

            return NotFound(errors);
        }
    }
}
