using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ASPNetCoreApp.Contexts;
using ASPNetCoreApp.Models;

namespace ASPNetCoreApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CountryController : Controller
    {
        private readonly WorldContext _context;

        public CountryController(WorldContext context)
        {
            _context = context;
        }

        [HttpGet]
        // GET: /api/Country/
        public async Task<IActionResult> Index()
        {
            return Ok(await _context.Countries.ToListAsync());
        }

        [HttpGet("Details/{id}")]
        // GET: /api/Country/Details/ABW
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return BadRequest("Code no puede ser nulo.");
            }

            var country = await _context.Countries.FirstOrDefaultAsync(m => m.Code == id);

            if (country == null)
            {
                return NotFound();
            }

            return Ok(country);
        }

        // POST: /api/Country/Create
        [HttpPost("Create")]
        // [ValidateAntiForgeryToken] // Se debe usar cuando se recibe la información solo a través de formulario
        public async Task<IActionResult> Create([FromBody] Country country)
        {
            if (!ModelState.IsValid)
            {
                // Devolver BadRequest si el modelo no es válido
                return BadRequest(ModelState);
            }

            _context.Add(country);
            await _context.SaveChangesAsync();

            // Devolver 201 Created con la URI para obtener el detalle del recurso
            return CreatedAtAction(nameof(Details), new { id = country.Code }, country);
        }

        // PUT: /api/Country/Edit/ABW
        [HttpPut("Edit/{id}")]
        public async Task<IActionResult> Edit(string id, [FromBody]Country country)
        {
            if (id != country.Code)
            {
                return BadRequest("El Code de la URL no coincide con el Code del objeto.");
            }

            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Se verifica si el recurso existe
            if(!CountryExists(id))
            {
                return NotFound();
            }

            try
            {
                _context.Update(country);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return Ok(country); // Devolver objeto actualizado
        }

        // DELETE: /api/Country/Delete/ABW
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var country = await _context.Countries.FindAsync(id);
            if (country == null)
            {
                return NotFound();
            }

            _context.Countries.Remove(country);
            await _context.SaveChangesAsync();

            // Devolver 204 No Content para indicar que la eliminación fue exitosa
            return NoContent();
        }

        private bool CountryExists(string id)
        {
            return _context.Countries.Any(e => e.Code == id);
        }
    }
}
