using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ariza.Migrations
{
    /// <inheritdoc />
    public partial class deletedurilg : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Durum",
                table: "Taleps");

            migrationBuilder.DropColumn(
                name: "Ilgilenen",
                table: "Taleps");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Durum",
                table: "Taleps",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Ilgilenen",
                table: "Taleps",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
