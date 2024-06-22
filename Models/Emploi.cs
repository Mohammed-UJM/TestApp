using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace TestApp.Models
{
    [CustomValidation(typeof(Emploi), nameof(ValidateDates))]
    public class Emploi
    {
        // Identifiant unique de l'emploi
        public long Id { get; set; }

        // Nom de l'entreprise
        [Required]
        public string? NomEntreprise { get; set; }

        // Poste occupé
        [Required]
        public string? Poste { get; set; }

        // Date de début de l'emploi
        [Required]
        public DateTime? DateDebut { get; set; }

        // Date de fin de l'emploi (peut être nulle si l'emploi est actuel)
        public DateTime? DateFin { get; set; }

        // Indicateur si l'emploi est actuel
        [Required]
        public bool? EstActuel { get; set; }

        // Identifiant de la personne liée à cet emploi
        [Required]
        public long? PersonneId { get; set; }

        // Validation des dates d'emploi
        public static ValidationResult ValidateDates(Emploi emploi, ValidationContext context)
        {
            if (emploi.EstActuel == true)
            {
                emploi.DateFin = null;
            }
            else
            {
                if (!emploi.DateFin.HasValue)
                {
                    return new ValidationResult("La date de fin est obligatoire si l'emploi n'est pas actuel.");
                }

                if (emploi.DateFin < emploi.DateDebut)
                {
                    return new ValidationResult("La date de fin doit être postérieure ou égale à la date de début.");
                }

                if (emploi.DateFin > DateTime.Now)
                {
                    return new ValidationResult("La date de fin ne peut pas être postérieure à aujourd'hui.");
                }
            }

            return ValidationResult.Success!;
        }
    }
}
