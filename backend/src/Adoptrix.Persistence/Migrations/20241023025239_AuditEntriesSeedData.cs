using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Adoptrix.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AuditEntriesSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AuditEntries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OperationName = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    StartTimeUtc = table.Column<DateTime>(type: "datetime2(2)", precision: 2, nullable: false),
                    EndTimeUtc = table.Column<DateTime>(type: "datetime2(2)", precision: 2, nullable: false),
                    WasSuccessful = table.Column<bool>(type: "bit", nullable: false),
                    ErrorMessage = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditEntries", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Species",
                columns: new[] { "Id", "LastModifiedBy", "Name" },
                values: new object[,]
                {
                    { 1, null, "Dog" },
                    { 2, null, "Cat" },
                    { 3, null, "Bird" }
                });

            migrationBuilder.InsertData(
                table: "Breeds",
                columns: new[] { "Id", "LastModifiedBy", "Name", "SpeciesId" },
                values: new object[,]
                {
                    { 1, null, "French Bulldog", 1 },
                    { 2, null, "Labrador retriever", 1 },
                    { 3, null, "Golden Retriever", 1 },
                    { 4, null, "German shepherd", 1 },
                    { 14, null, "Domestic Shorthair", 2 },
                    { 18, null, "African Grey Parrot", 3 }
                });

            migrationBuilder.InsertData(
                table: "Animals",
                columns: new[] { "Id", "BreedId", "DateOfBirth", "Description", "LastModifiedBy", "Name", "Sex", "Slug" },
                values: new object[,]
                {
                    { new Guid("5edcfb8c-296a-434f-913f-967c58073ca2"), 2, new DateOnly(2024, 2, 14), "Meet Alberto, a delightful Labrador puppy searching for his forever home. With a golden coat that's as soft as his heart, Alberto's playful spirit is infectious. From chasing butterflies to fetching balls, his days are filled with joy and curiosity. This lovable pup dreams of a family to call his own, where he can share his boundless love and enthusiasm. Could you be the one to open your heart and home to Alberto, making his dreams come true? Adopt this charming ball of fur, and let the adventure of a lifetime begin! 🐾 #AdoptAlberto", null, "Alberto", "M", "alberto-2024-02-14" },
                    { new Guid("7e92871b-7219-495f-945d-fc8b5ba78829"), 18, new DateOnly(2017, 4, 11), "Meet Percy, a charming African Grey Parrot with a personality as colorful as his feathers.", null, "Percy", "F", "percy-2017-04-11" },
                    { new Guid("a8a4897a-6594-4796-9b25-f54ca71bbbc7"), 14, new DateOnly(2022, 9, 30), "Introducing Ginger, a beautiful, captivating feline with a coat as fiery as her playful spirit. This adorable cat enchants everyone with her graceful moves and amber-colored eyes. From chasing sunbeams to batting at toys, Ginger's days are a whimsical blend of elegance and mischief. This charming kitty yearns for a loving home, where she can curl up on a cozy spot and purr her way into your heart. Are you ready to add a touch of warmth and whimsy to your life? Consider adopting Ginger, and let the purr-fect companionship begin! 🐾 #AdoptGinger", null, "Ginger", "F", "ginger-2022-09-30" },
                    { new Guid("c387d7dc-18f0-4ecb-bc8e-10c6c9ba9e1f"), 4, new DateOnly(2020, 4, 19), "Meet Barry, a majestic German Shepherd with a heart as loyal as his gaze. At four years old, Barry embodies both strength and gentleness in equal measure. His rich, dark coat gleams in the sunlight as he explores the world with curiosity and confidence. From romping through fields to standing guard with unwavering vigilance, Barry is the epitome of loyalty and companionship. This noble canine seeks a forever home where he can shower his family with unconditional love and protection. Ready to welcome a steadfast friend into your life? Consider adopting Barry, and embark on a journey of trust, devotion, and endless adventure! 🐾 #AdoptBarry", null, "Barry", "M", "barry-2020-04-19" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AuditEntries_OperationName",
                table: "AuditEntries",
                column: "OperationName");

            migrationBuilder.CreateIndex(
                name: "IX_AuditEntries_UserId",
                table: "AuditEntries",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuditEntries");

            migrationBuilder.DeleteData(
                table: "Animals",
                keyColumn: "Id",
                keyValue: new Guid("5edcfb8c-296a-434f-913f-967c58073ca2"));

            migrationBuilder.DeleteData(
                table: "Animals",
                keyColumn: "Id",
                keyValue: new Guid("7e92871b-7219-495f-945d-fc8b5ba78829"));

            migrationBuilder.DeleteData(
                table: "Animals",
                keyColumn: "Id",
                keyValue: new Guid("a8a4897a-6594-4796-9b25-f54ca71bbbc7"));

            migrationBuilder.DeleteData(
                table: "Animals",
                keyColumn: "Id",
                keyValue: new Guid("c387d7dc-18f0-4ecb-bc8e-10c6c9ba9e1f"));

            migrationBuilder.DeleteData(
                table: "Breeds",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Breeds",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Breeds",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Breeds",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Breeds",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Breeds",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Species",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Species",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Species",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
