using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        // Récupération de la liste des personnes triées par nom
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Personne>>> GetPersonnes()
        {
            return await _context.Personnes
                .Include(p => p.Emplois.Where(e => e.EstActuel == true))
                .OrderBy(p => p.Nom)
                .ToListAsync();
        }

        // Récupération d'une personne par ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Personne>> GetPersonne(long id)
        {
            var personne = await _context.Personnes
                .Include(p => p.Emplois)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (personne == null)
            {
                return NotFound();
            }

            return Ok(new
            {
                personne.Id,
                personne.Nom,
                personne.Prenom,
                personne.DateNaissance,
                personne.Age,
                personne.Emplois,
                EmploisActuels = personne.Emplois.Where(e => e.EstActuel == true)
            });
        }

        // Mise à jour d'une personne par ID
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPersonne(long id, Personne personne)
        {
            if (id != personne.Id)
            {
                return BadRequest("Le parametre 'id' ne correspond pas à l id de l objet en parametre 'personne' !");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
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

            // Retournez la personne modifiée avec un message de succès
            return Ok(new { message = "Modification réussie !", personne });
        }

        // Ajout d'une nouvelle personne
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

            // Retournez la personne supprimée avec un message de succès
            return Ok(new { message = "Suppression réussie !", personne });
        }

        // Récupération de toutes les personnes ayant travaillé pour une entreprise donnée
        [HttpGet("by_entreprise/{nomEntreprise}")]
        public async Task<ActionResult<IEnumerable<Personne>>> GetPersonnesByCompany(string nomEntreprise)
        {
            if (string.IsNullOrWhiteSpace(nomEntreprise))
            {
                return BadRequest("Le nom de l'entreprise doit être fourni.");
            }

            var personnes = await _context.Personnes
                .Include(p => p.Emplois.Where(e => e.EstActuel == true))
                .Where(p => p.Emplois.Any(e => e.NomEntreprise == nomEntreprise))
                .ToListAsync();

            return Ok(personnes);
        }

        // Vérification de l'existence d'une personne par ID
        private bool PersonneExiste(long id)
        {
            return _context.Personnes.Any(e => e.Id == id);
        }
    }
}
