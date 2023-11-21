using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Adoptrix.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ConvertToSpeciesCode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Species",
                table: "Animals",
                newName: "SpeciesCode");

            migrationBuilder.AlterColumn<string>(
                name: "SpeciesCode",
                table: "Animals",
                type: "char(2)",
                maxLength: 2,
                nullable: false,
                oldClrType: typeof(byte),
                oldType: "tinyint");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SpeciesCode",
                table: "Animals",
                newName: "Species");

            migrationBuilder.AlterColumn<byte>(
                name: "Species",
                table: "Animals",
                type: "tinyint",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "char(2)",
                oldMaxLength: 2);
        }
    }
}
