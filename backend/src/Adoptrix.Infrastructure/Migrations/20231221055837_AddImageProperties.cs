using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Adoptrix.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddImageProperties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileName",
                table: "AnimalImages");

            migrationBuilder.AddColumn<bool>(
                name: "HasFullSize",
                table: "AnimalImages",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "HasPreview",
                table: "AnimalImages",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "HasThumbnail",
                table: "AnimalImages",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "OriginalContentType",
                table: "AnimalImages",
                type: "varchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HasFullSize",
                table: "AnimalImages");

            migrationBuilder.DropColumn(
                name: "HasPreview",
                table: "AnimalImages");

            migrationBuilder.DropColumn(
                name: "HasThumbnail",
                table: "AnimalImages");

            migrationBuilder.DropColumn(
                name: "OriginalContentType",
                table: "AnimalImages");

            migrationBuilder.AddColumn<string>(
                name: "FileName",
                table: "AnimalImages",
                type: "varchar(40)",
                maxLength: 40,
                nullable: false,
                defaultValue: "");
        }
    }
}
