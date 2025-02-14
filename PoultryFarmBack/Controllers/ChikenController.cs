using Microsoft.AspNetCore.Mvc;
using PoultryFarmBack.Models;
using PoultryFarmBack.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PoultryFarmBack.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChickenController : ControllerBase
    {
        private readonly ChickenService _chickenService;

        public ChickenController(ChickenService chickenService)
        {
            _chickenService = chickenService;
        }

        // GET: api/chicken
        [HttpGet]
        public IActionResult GetAllChickens()
        {
            var chickens = _chickenService.GetAllChickens();
            return Ok(chickens); 
        }


        // GET: api/chicken/5
        [HttpGet("{id}")]
        public IActionResult GetChickenById(int id)
        {
            var chicken = _chickenService.GetChickenById(id);
            if (chicken == null)
            {
                return NotFound(); // Возвращаем 404, если курица не найдена
            }
            return Ok(chicken); // Возвращаем курицу с кодом 200 (OK)
        }


        // POST: api/chicken
        [HttpPost]
        public IActionResult AddChicken([FromBody] Chicken chicken)
        {
            if (chicken == null)
            {
                return BadRequest(); // Возвращаем 400, если данные неверны
            }

            _chickenService.AddChicken(chicken);
            return CreatedAtAction(nameof(GetChickenById), new { id = chicken.Id }, chicken); // Возвращаем 201 (Created)
        }

        // PUT: api/chicken/5
        [HttpPut("{id}")]
        public IActionResult UpdateChicken(int id, [FromBody] Chicken chicken)
        {
            if (chicken == null || id != chicken.Id)
            {
                return BadRequest(); // Возвращаем 400, если данные неверны
            }

            _chickenService.UpdateChicken(chicken);
            return NoContent(); // Возвращаем 204 (No Content)
        }

        // DELETE: api/chicken/5
        [HttpDelete("{id}")]
        public IActionResult DeleteChicken(int id)
        {
            _chickenService.DeleteChicken(id);
            return NoContent(); // Возвращаем 204 (No Content)
        }

    }
}
