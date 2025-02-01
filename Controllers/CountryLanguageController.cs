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
    public class CountryLanguageController : Controller
    {
        private readonly WorldContext _context;

        public CountryLanguageController(WorldContext context)
        {
            _context = context;
        }

        [HttpGet]
        // GET: /api/CountryLanguage
        public async Task<IActionResult> Index()
        {
            return Ok(await _context.Countrylanguages.ToListAsync());
        }

        [HttpGet("Details/{countryCode}/{language}")]
        // GET: /api/CountryLanguage/Details/ABW/Spanish
        public async Task<IActionResult> Details(string countryCode, string language)
        {
            if (countryCode == null || language == null)
            {
                return BadRequest("CountryCode y Language no pueden ser nulos.");
            }

            Countrylanguage? countryLanguage = await _context.Countrylanguages
                .FromSqlInterpolated($"SELECT * FROM CountryLanguage WHERE CountryCode = {countryCode} AND Language = {language}")
                .FirstOrDefaultAsync();

            /* Equivalente usando LINQ (Language Integrated Query)
            var countryLanguage = await _context.Countrylanguages
                .FirstOrDefaultAsync(m => m.CountryCode == countryCode && m.Language == language);
            */

            if (countryLanguage == null)
            {
                return NotFound();
            }

            return Ok(countryLanguage);
        }

        // POST: /api/CountryLanguage/Create
        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] Countrylanguage countrylanguage)
        {
            if (!ModelState.IsValid)
            {
                // Devolver BadRequest si el modelo no es válido
                return BadRequest(ModelState);
            }

            _context.Add(countrylanguage);
            await _context.SaveChangesAsync();

            // Devolver 201 Created con la URI para obtener el detalle del recurso
            return CreatedAtAction(nameof(Details), new { countryCode = countrylanguage.CountryCode, language = countrylanguage.Language }, countrylanguage);
        }

        // PUT: /api/CountryLanguage/Edit/ABW/Spaish
        [HttpPut("Edit/{countryCode}/{language}")]
        public async Task<IActionResult> Edit(string countryCode, String language, [FromBody]Countrylanguage countrylanguage)
        {
            if (countryCode != countrylanguage.CountryCode || language != countrylanguage.Language)
            {
                return BadRequest("CountryCode y/o Language de la URL no coincide con el del objeto.");
            }

            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Se verifica si el recurso existe
            if(!CountrylanguageExists(countryCode, language))
            {
                return NotFound();
            }

            try
            {
                _context.Update(countrylanguage);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return Ok(countrylanguage); // Devolver objeto actualizado
        }

        // DELETE: /api/CountryLanguage/Delete/ABW/Spanish
        [HttpDelete("Delete/{countryCode}/{language}")]
        public async Task<IActionResult> DeleteConfirmed(string countryCode, string language)
        {
            // var countrylanguage = await _context.Countrylanguages.FirstOrDefaultAsync(m => m.CountryCode == countryCode && m.Language == language);
            var countrylanguage = await _context.Countrylanguages.FindAsync(countryCode, language);
            if(countrylanguage == null) {
                return NotFound();
            }
            
            _context.Countrylanguages.Remove(countrylanguage);
            await _context.SaveChangesAsync();

            // Devolver 204 No Content para indicar que la eliminación fue exitosa
            return NoContent();
        }

        private bool CountrylanguageExists(string countryCode, string language)
        {
            return _context.Countrylanguages.Any(e => e.CountryCode == countryCode && e.Language == language);
        }
    }
}
