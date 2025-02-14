using Microsoft.AspNetCore.Mvc;
using PoultryFarmBack.Models;
using PoultryFarmBack.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PoultryFarmBack.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChikenController : ControllerBase
    {
        private readonly ChickenService _chickenService;

        public ChickenController(ChickenService chickenService)
        {
            _chickenService = chickenService;
        }

        [HttpGet]
        public IActionResult GetAllChickens()
        {
            var chickens = _chickenService.GetAllChickens();
            return Ok(chickens);
        }

        [HttpGet("{id}")]
        public IActionResult GetChickenById(int id)
        {
            var chicken = _chickenService.GetChickenById(id);
            if (chicken == null) return NotFound();
            return Ok(chicken);
        }

        [HttpPost]
        public IActionResult AddChicken(Chicken chicken)
        {
            _chickenService.AddChicken(chicken);
            return CreatedAtAction(nameof(GetChickenById), new { id = chicken.Id }, chicken);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateChicken(int id, Chicken chicken)
        {
            if (id != chicken.Id) return BadRequest();
            _chickenService.UpdateChicken(chicken);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteChicken(int id)
        {
            _chickenService.DeleteChicken(id);
            return NoContent();
        }

    }
}
