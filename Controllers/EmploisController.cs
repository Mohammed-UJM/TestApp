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
    public class EmploisController : ControllerBase
    {
        private readonly PersonnesAppContext _context;

        public EmploisController(PersonnesAppContext context)
        {
            _context = context;
        }

        // Récupération de la liste des emplois
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Emploi>>> GetEmplois()
        {
            return await _context.Emplois.ToListAsync();
        }

        // Récupération d'un emploi par ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Emploi>> GetEmploi(long id)
        {
            var emploi = await _context.Emplois.FindAsync(id);

            if (emploi == null)
            {
                return NotFound();
            }

            return emploi;
        }

        // Mise à jour d'un emploi par ID
        // Pour protéger contre les attaques de surpostage, voir https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmploi(long id, Emploi emploi)
        {
            if (id != emploi.Id)
            {
                return BadRequest();
            }

            _context.Entry(emploi).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmploiExiste(id))
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

        // Ajout d'un nouvel emploi
        // Pour protéger contre les attaques de surpostage, voir https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Emploi>> PostEmploi(Emploi emploi)
        {
            _context.Emplois.Add(emploi);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEmploi", new { id = emploi.Id }, emploi);
        }

        // Suppression d'un emploi par ID
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmploi(long id)
        {
            var emploi = await _context.Emplois.FindAsync(id);
            if (emploi == null)
            {
                return NotFound();
            }

            _context.Emplois.Remove(emploi);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // Vérification de l'existence d'un emploi par ID
        private bool EmploiExiste(long id)
        {
            return _context.Emplois.Any(e => e.Id == id);
        }
    }
}
