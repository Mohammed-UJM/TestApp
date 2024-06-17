using System.ComponentModel.DataAnnotations;

namespace TestApp.Models
{
    public class Emploi
    {
        // Identifiant unique de l'emploi
        public long Id { get; set; }
        // Nom de l'entreprise
        public string? NomEntreprise { get; set; }
        // Poste occupé
        public string? Poste { get; set; }
        // Date de début de l'emploi
        public DateTime DateDebut { get; set; }
        // Date de fin de l'emploi (peut être nulle si l'emploi est actuel)
        public DateTime? DateFin { get; set; }
        // Indicateur si l'emploi est actuel
        public bool EstActuel { get; set; }
        // Identifiant de la personne liée à cet emploi
        public long PersonneId { get; set; }
    }
}
