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
    [Route("/api/[controller]")]
    [ApiController]
    public class CityController : Controller
    {
        private readonly WorldContext _context;

        public CityController(WorldContext context)
        {
            _context = context;
        }

        [HttpGet]
        // GET: /api/City
        public async Task<IActionResult> Index()
        {
            return Ok(await _context.Cities.ToListAsync());
        }

        [HttpGet("Details/{id}")]
        // GET: /api/City/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return BadRequest("El ID no puede ser nulo.");
            }

            var city = await _context.Cities.FirstOrDefaultAsync(m => m.Id == id);

            if (city == null)
            {
                return NotFound();
            }

            return Ok(city);
        }

        // POST: /api/City/Create
        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] City city)
        {
            if (!ModelState.IsValid)
            {
                // Devolver BadRequest si el modelo no es válido
                return BadRequest(ModelState);
            }

            _context.Add(city);
            await _context.SaveChangesAsync();

            // Devolver 201 Created con la URI para obtener el detalle del recurso
            return CreatedAtAction(nameof(Details), new { id = city.Id }, city);
        }

        // PUT: /api/City/Edit/5
        [HttpPut("Edit/{id}")]
        public async Task<IActionResult> Edit(int id, [FromBody] City city)
        {
            if (id != city.Id)
            {
                return BadRequest("El ID de la URL no coincide con el ID del objeto.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Se verifica si el recurso existe
            if (!CityExists(id))
            {
                return NotFound();
            }

            try
            {
                _context.Update(city);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return Ok(city); // Devolver objeto actualizado
        }

        // DELETE: /api/City/Delete/5
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var city = await _context.Cities.FindAsync(id);
            if (city == null)
            {
                return NotFound();
            }
            
            _context.Cities.Remove(city);
            await _context.SaveChangesAsync();

            // Devolver 204 No Content para indicar que la eliminación fue exitosa
            return NoContent();
        }

        private bool CityExists(int id)
        {
            return _context.Cities.Any(e => e.Id == id);
        }
    }
}
