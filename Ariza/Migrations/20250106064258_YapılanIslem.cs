using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ariza.Migrations
{
    /// <inheritdoc />
    public partial class YapılanIslem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "YapilanIslem",
                table: "Taleps",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "YapilanIslem",
                table: "Taleps");
        }
    }
}
