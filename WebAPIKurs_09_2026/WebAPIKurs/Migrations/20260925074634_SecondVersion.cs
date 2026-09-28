using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebAPIKurs.Migrations
{
    /// <inheritdoc />
    public partial class SecondVersion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Year",
                table: "Movie",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Year",
                table: "Movie");
        }
    }
}
