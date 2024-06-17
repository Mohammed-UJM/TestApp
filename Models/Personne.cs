using System.ComponentModel.DataAnnotations;

namespace TestApp.Models
{
    public class Personne
    {
        // Identifiant unique de la personne
        public long Id { get; set; }
        // Nom de la personne
        public string? Nom { get; set; }
        // Prénom de la personne
        public string? Prenom { get; set; }
        // Date de naissance de la personne
        public DateTime DateNaissance { get; set; }
        // Liste des emplois associés à la personne
        public List<Emploi>? Emplois { get; set; } = new List<Emploi>();
    }
}
