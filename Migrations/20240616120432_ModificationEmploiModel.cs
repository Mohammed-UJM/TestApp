using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TestApp.Migrations
{
    /// <inheritdoc />
    public partial class ModificationEmploiModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Emplois_Personnes_PersonneId1",
                table: "Emplois");

            migrationBuilder.DropIndex(
                name: "IX_Emplois_PersonneId1",
                table: "Emplois");

            migrationBuilder.DropColumn(
                name: "PersonneId1",
                table: "Emplois");

            migrationBuilder.AlterColumn<long>(
                name: "PersonneId",
                table: "Emplois",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_Emplois_PersonneId",
                table: "Emplois",
                column: "PersonneId");

            migrationBuilder.AddForeignKey(
                name: "FK_Emplois_Personnes_PersonneId",
                table: "Emplois",
                column: "PersonneId",
                principalTable: "Personnes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Emplois_Personnes_PersonneId",
                table: "Emplois");

            migrationBuilder.DropIndex(
                name: "IX_Emplois_PersonneId",
                table: "Emplois");

            migrationBuilder.AlterColumn<int>(
                name: "PersonneId",
                table: "Emplois",
                type: "int",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddColumn<long>(
                name: "PersonneId1",
                table: "Emplois",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Emplois_PersonneId1",
                table: "Emplois",
                column: "PersonneId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Emplois_Personnes_PersonneId1",
                table: "Emplois",
                column: "PersonneId1",
                principalTable: "Personnes",
                principalColumn: "Id");
        }
    }
}
