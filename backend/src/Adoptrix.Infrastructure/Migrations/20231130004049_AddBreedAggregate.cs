using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Adoptrix.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddBreedAggregate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BreedId",
                table: "Animals",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Breeds",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    SpeciesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Breeds", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Breeds_Species_SpeciesId",
                        column: x => x.SpeciesId,
                        principalTable: "Species",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Breeds",
                columns: new[] { "Id", "Name", "SpeciesId" },
                values: new object[,]
                {
                    { 1, "Labrador Retriever", 1 },
                    { 2, "German Shepherd", 1 },
                    { 3, "Golden Retriever", 1 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Animals_BreedId",
                table: "Animals",
                column: "BreedId");

            migrationBuilder.CreateIndex(
                name: "IX_Breeds_Name",
                table: "Breeds",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Breeds_SpeciesId",
                table: "Breeds",
                column: "SpeciesId");

            migrationBuilder.AddForeignKey(
                name: "FK_Animals_Breeds_BreedId",
                table: "Animals",
                column: "BreedId",
                principalTable: "Breeds",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Animals_Breeds_BreedId",
                table: "Animals");

            migrationBuilder.DropTable(
                name: "Breeds");

            migrationBuilder.DropIndex(
                name: "IX_Animals_BreedId",
                table: "Animals");

            migrationBuilder.DropColumn(
                name: "BreedId",
                table: "Animals");
        }
    }
}
