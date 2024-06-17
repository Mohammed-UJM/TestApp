using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestApp.Models;

namespace TestApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonnesController : ControllerBase
    {
        private readonly PersonnesAppContext _context;

        public PersonnesController(PersonnesAppContext context)
        {
            _context = context;
        }

        // Récupération de la liste des personnes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Personne>>> GetPersonnes()
        {
            return await _context.Personnes
                .Include(p => p.Emplois)
                .ToListAsync();
        }

        // Récupération d'une personne par ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Personne>> GetPersonne(long id)
        {
            var personne = await _context.Personnes
                .Include(p => p.Emplois) // Inclure les emplois liés à la personne
                .FirstOrDefaultAsync(p => p.Id == id);

            if (personne == null)
            {
                return NotFound();
            }

            return personne;
        }

        // Mise à jour d'une personne par ID
        // Pour protéger contre les attaques de surpostage, voir https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPersonne(long id, Personne personne)
        {
            if (id != personne.Id)
            {
                return BadRequest();
            }

            _context.Entry(personne).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PersonneExiste(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // Ajout d'une nouvelle personne
        // Pour protéger contre les attaques de surpostage, voir https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Personne>> PostPersonne(Personne personne)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Personnes.Add(personne);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPersonne), new { id = personne.Id }, personne);
        }

        // Suppression d'une personne par ID
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePersonne(long id)
        {
            var personne = await _context.Personnes.FindAsync(id);
            if (personne == null)
            {
                return NotFound();
            }

            _context.Personnes.Remove(personne);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // Vérification de l'existence d'une personne par ID
        private bool PersonneExiste(long id)
        {
            return _context.Personnes.Any(e => e.Id == id);
        }
    }
}
