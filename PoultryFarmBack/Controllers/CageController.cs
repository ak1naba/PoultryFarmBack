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


        // GET: api/cage/5
        [HttpGet("{id}")]
        public IActionResult GetCageById(int id)
        {
            var cage = _cageService.GetById(id);
            if (cage == null)
            {
                return NotFound(); // Возвращаем 404, если курица не найдена
            }
            return Ok(cage); // Возвращаем курицу с кодом 200 (OK)
        }

        //POST: api/cage
         [HttpPost]
        public IActionResult AddCage([FromBody] Cage cage)
        {
            if (cage == null)
                return BadRequest(new { message = "Некорректные данные." });

            if(_cageService.AddCage(cage, out Dictionary<string, string> errors))
                return CreatedAtAction(nameof(GetCageById), new { id = cage.Id }, cage);

            return UnprocessableEntity(new { errors });
        }

        // PUT: api/cage/5
        [HttpPut("{id}")]
        public IActionResult UpdateCage(int id, [FromBody] Cage cage)
        {
            if (cage == null || id != cage.Id)
                return BadRequest(new { message = "Некорректные данные." });

            if(_cageService.UpdateCage(cage, out Dictionary<string, string> errors))
            {
                var updateCage = _cageService.GetById(id);

                if(updateCage == null)
                    return NotFound(new { message = "Обновленный объект не найден." });

                return Ok(updateCage);
            }

            return UnprocessableEntity(new { errors });
        }

        // DELETE: api/cage/5
        [HttpDelete("{id}")]
        public IActionResult DeleteCage(int id)
        {
            if (_cageService.DeleteCage(id, out Dictionary<string, string> errors))
            {
                return NoContent(); // Возвращаем 204 (No Content)
            }
            else
            {
                return UnprocessableEntity(new { errors });
            }
        }
    }
}