using Microsoft.AspNetCore.Mvc;
using PoultryFarmBack.Models;
using PoultryFarmBack.Services;

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
        public IActionResult GetAllCages()
        {
            var employees = _employeeService.GetAll();
            return Ok(employees); 
        }

        

    }
}