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
                return BadRequest("Le parametre 'id' ne correspond pas à l id de l objet en parametre 'emploi' !");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var personne = await _context.Personnes.FindAsync(emploi.PersonneId);
            if (personne == null)
            {
                return NotFound("Aucune personne avec l ID fourni n'existe en base de données !");
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

            // Retournez l'emploi modifié avec un message de succès
            return Ok(new { message = "Modification réussie !", emploi });
        }

        // Ajout d'un nouvel emploi
        // Pour protéger contre les attaques de surpostage, voir https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Emploi>> PostEmploi(Emploi emploi)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var personne = await _context.Personnes.FindAsync(emploi.PersonneId);
            if (personne == null)
            {
                return NotFound("Aucune personne avec l ID fourni n'existe en base de données !");
            }

            _context.Emplois.Add(emploi);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetEmploi), new { id = emploi.Id }, emploi);
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

            // Retournez l'emploi supprimé avec un message de succès
            return Ok(new { message = "Suppression réussie !", emploi });
        }

        [HttpGet("{id}/emplois")]
        public async Task<ActionResult> GetEmploisEntreDates(long id, DateTime? dateDebut, DateTime? dateFin)
        {
            // Définir les dates par défaut si elles ne sont pas fournies
            dateDebut ??= DateTime.Now.AddYears(-150);
            dateFin ??= DateTime.Now;

            // Vérification si la date de début est inférieure ou égale à la date de fin
            if (dateDebut.Value > dateFin.Value)
            {
                return BadRequest("La date de début doit être inférieure ou égale à la date de fin.");
            }

            // Récupérer la personne avec ses emplois
            var personne = await _context.Personnes
                .Include(p => p.Emplois)
                .FirstOrDefaultAsync(p => p.Id == id);

            // Vérification si la personne existe
            if (personne == null)
            {
                return NotFound($"Aucune personne avec le id fourni {id} n'existe en base de données !");
            }

            // Filtrer les emplois selon les critères spécifiés
            var emploisFiltres = personne.Emplois
                .Where(e =>
                    // Condition pour les emplois ayant une date de début ou de fin à l'intérieur de la plage
                    (e.DateDebut >= dateDebut && e.DateDebut <= dateFin) ||
                    (e.DateFin >= dateDebut && e.DateFin <= dateFin) ||

                    // Condition pour les emplois ayant une date de début et de fin à l'extérieur de la plage
                    (e.DateDebut <= dateDebut && e.DateFin >= dateFin))
                .ToList();

            // Retourner les emplois filtrés
            return Ok(emploisFiltres);
        }


        // Vérification de l'existence d'un emploi par ID
        private bool EmploiExiste(long id)
        {
            return _context.Emplois.Any(e => e.Id == id);
        }
    }
}
