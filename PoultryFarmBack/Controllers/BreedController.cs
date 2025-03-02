using Microsoft.AspNetCore.Mvc;
using PoultryFarmBack.Models;
using PoultryFarmBack.Services;

namespace PoultryFarmBack.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class BreedController : ControllerBase
    {
        private readonly BreedService _breedService;

        public BreedController(BreedService breedService)
        {
            _breedService = breedService;
        }

        // GET: api/cage
        [HttpGet]
        public IActionResult GetAllCages()
        {
            var breeds = _breedService.GetAll();
            return Ok(breeds); 
        }

        

    }
}