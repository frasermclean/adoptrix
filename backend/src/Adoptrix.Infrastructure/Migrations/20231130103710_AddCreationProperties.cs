using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Adoptrix.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCreationProperties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Species",
                type: "datetime2(2)",
                precision: 2,
                nullable: false,
                defaultValueSql: "getutcdate()");

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                table: "Species",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Breeds",
                type: "datetime2(2)",
                precision: 2,
                nullable: false,
                defaultValueSql: "getutcdate()");

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                table: "Breeds",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Animals",
                type: "datetime2(2)",
                precision: 2,
                nullable: false,
                defaultValueSql: "getutcdate()");

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                table: "Animals",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.UpdateData(
                table: "Breeds",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedBy",
                value: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.UpdateData(
                table: "Breeds",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedBy",
                value: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.UpdateData(
                table: "Breeds",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedBy",
                value: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.UpdateData(
                table: "Species",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedBy",
                value: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.UpdateData(
                table: "Species",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedBy",
                value: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.UpdateData(
                table: "Species",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedBy",
                value: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Species");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Species");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Breeds");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Breeds");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Animals");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Animals");
        }
    }
}
