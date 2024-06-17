using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TestApp.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Personnes",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nom = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Prenom = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateNaissance = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Personnes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Emplois",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NomEntreprise = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Poste = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateDebut = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateFin = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EstActuel = table.Column<bool>(type: "bit", nullable: false),
                    PersonneId = table.Column<int>(type: "int", nullable: false),
                    PersonneId1 = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Emplois", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Emplois_Personnes_PersonneId1",
                        column: x => x.PersonneId1,
                        principalTable: "Personnes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Emplois_PersonneId1",
                table: "Emplois",
                column: "PersonneId1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Emplois");

            migrationBuilder.DropTable(
                name: "Personnes");
        }
    }
}
