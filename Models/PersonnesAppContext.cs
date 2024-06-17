using Microsoft.EntityFrameworkCore;

namespace TestApp.Models
{
    public class PersonnesAppContext : DbContext
    {
        // Constructeur de la classe avec des options DbContext
        public PersonnesAppContext(DbContextOptions<PersonnesAppContext> options)
            : base(options)
        {
        }

        // Ensemble de données pour les personnes
        public DbSet<Personne> Personnes { get; set; } = null!;

        // Ensemble de données pour les emplois
        public DbSet<Emploi> Emplois { get; set; } = null!;
    }
}
