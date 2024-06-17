using Microsoft.EntityFrameworkCore;
using TestApp.Models;

var builder = WebApplication.CreateBuilder(args);

// Ajout des services au conteneur.
builder.Services.AddControllers();

// Enregistrement du contexte de l'application
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<PersonnesAppContext>(options =>
    options.UseSqlServer(connectionString));

// Configuration CORS pour permettre les appels depuis une source externe spécifique
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowVueApp",
        builder =>
        {
            builder.WithOrigins("http://localhost:8080")
                   .AllowAnyHeader()
                   .AllowAnyMethod();
        });
});

// Configuration de Swagger/OpenAPI pour la documentation de l'API
// Pour en savoir plus sur la configuration de Swagger/OpenAPI, voir https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configuration du pipeline des requêtes HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Application de la politique CORS
app.UseCors("AllowVueApp");

app.UseAuthorization();

app.MapControllers();

app.Run();
