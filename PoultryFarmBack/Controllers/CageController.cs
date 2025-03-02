using Microsoft.AspNetCore.Mvc;
using PoultryFarmBack.Models;
using PoultryFarmBack.Services;

namespace PoultryFarmBack.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class CageController : ControllerBase
    {
        private readonly CageService _cageService;

        public CageController(CageService cageService)
        {
            _cageService = cageService;
        }

        // GET: api/cage
        [HttpGet]
        public IActionResult GetAllCages()
        {
            var chickens = _cageService.GetAll();
            return Ok(chickens); 
        }

        

    }
}