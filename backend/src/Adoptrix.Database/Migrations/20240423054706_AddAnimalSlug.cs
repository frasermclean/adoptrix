using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Adoptrix.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddAnimalSlug : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Animals",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AddColumn<string>(
                name: "Slug",
                table: "Animals",
                type: "nvarchar(60)",
                maxLength: 60,
                nullable: false,
                defaultValueSql: "newid()");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Animals_Slug",
                table: "Animals",
                column: "Slug");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_Animals_Slug",
                table: "Animals");

            migrationBuilder.DropColumn(
                name: "Slug",
                table: "Animals");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Animals",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30);
        }
    }
}
