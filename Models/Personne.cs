using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace TestApp.Models
{
    public class Personne
    {
        // Identifiant unique de la personne
        public long Id { get; set; }

        // Nom de la personne
        [Required]
        public string? Nom { get; set; }

        // Prénom de la personne
        [Required]
        public string? Prenom { get; set; }

        // Date de naissance de la personne
        [Required]
        [CustomValidation(typeof(Personne), nameof(ValidateDateNaissance))]
        public DateTime? DateNaissance { get; set; }

        // Age calculé (non stocké en base)
        [NotMapped]
        public int Age => CalculateAge(DateNaissance);

        // Liste des emplois associés à la personne
        public List<Emploi> Emplois { get; set; } = new List<Emploi>();

        // Validation de la date de naissance
        public static ValidationResult ValidateDateNaissance(DateTime? dateNaissance, ValidationContext context)
        {
            if (dateNaissance.HasValue)
            {

                if (dateNaissance.Value.AddYears(150) < DateTime.Now)
                {
                    return new ValidationResult("La date de naissance ne peut pas dépasser 150 ans.");
                }
            }

            return ValidationResult.Success!;
        }

        // Calcul de l'âge
        private static int CalculateAge(DateTime? dateNaissance)
        {
            if (dateNaissance.HasValue)
            {
                var age = DateTime.Now.Year - dateNaissance.Value.Year;
                if (DateTime.Now.DayOfYear < dateNaissance.Value.DayOfYear)
                {
                    age--;
                }
                return age;
            }
            else
            {
                return -1;
            }
        }
    }
}
