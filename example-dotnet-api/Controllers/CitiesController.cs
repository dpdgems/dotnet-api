using example_dotnet_api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace example_dotnet_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CitiesController : ControllerBase
    {
        //private readonly ILogger<CitiesController> _logger;
        private readonly WorldContext _context;

        public CitiesController(ILogger<CitiesController> logger, WorldContext context)
        {
            //_logger = logger;
            _context = context;
        }

        // GET: api/<CitiesController>
        [HttpGet]
        public IActionResult GetAllCities()
        {
            try
            {
                List<City> listCity = _context.Cities.ToList();
                return Ok(new { status = true, data = listCity, message = "" });
            }
            catch (Exception)
            {
                return Problem();
            }
        }

        // GET api/<CitiesController>/3320
        [HttpGet("{id}")]
        public IActionResult GetCitiesById(int id)
        {
            try
            {
                var city = _context.Cities.SingleOrDefault(s => s.Id == id);
                if (city is null)
                {
                    return BadRequest(new { status = false, message = $"This ID: {id} doesn't exist!" });
                }

                return Ok(new { status = true, data = city, message = "" });
            }
            catch (Exception)
            {
                return Problem();
            }
        }

        // POST api/<CitiesController>
        [HttpPost]
        public IActionResult AddCities([FromBody] City values)
        {
            // FormBody = application/json
            // FromFrom = multipart/form-data
            try
            {
                _context.Cities.Add(values);
                var result = _context.SaveChanges();
                if (result == 0)
                {
                    return BadRequest(new { status = false, message = "Failed to add!" });
                }

                return Ok(new { status = true, message = $"Add ${values.Name} successfully" });
            }
            catch (Exception)
            {
                return Problem();
            }
        }

        // PUT api/<CitiesController>/4082
        [HttpPut("{id}")]
        public IActionResult UpdateCities(int id, [FromBody] City values)
        {
            // FormBody = application/json
            // FromFrom = multipart/form-data
            try
            {
                var city = _context.Cities.FirstOrDefault(f => f.Id == id);
                if (city == null)
                {
                    return BadRequest(new { status = false, message = $"This ID: {id} doesn't exist!" });
                }

                city.Name = values.Name ?? city.Name;
                city.CountryCode = values.CountryCode ?? city.CountryCode;
                city.District = values.District ?? city.District;
                city.Population = values.Population != 0 ? values.Population : city.Population;
                _context.SaveChanges();

                return Ok(new { status = true, message = $"Update ID: {id} successfully" });
            }
            catch (Exception)
            {
                return Problem();
            }
        }

        // DELETE api/<CitiesController>/4082
        [HttpDelete("{id}")]
        public IActionResult DeleteCities(int id)
        {
            try
            {
                var city = _context.Cities.FirstOrDefault(f => f.Id == id);
                if (city is null)
                {
                    return BadRequest(new { status = false, message = $"This ID: {id} doesn't exist!" });
                }

                _context.Cities.Remove(city);
                _context.SaveChanges();

                return Ok(new { status = true, message = $"Delete ID: {id} successfully" });
            }
            catch (Exception)
            {
                return Problem();
            }
        }
    }
}
