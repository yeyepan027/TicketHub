using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TicketHub.Migrations
{
    /// <inheritdoc />
    public partial class AddImageFilenameToShow : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageFilename",
                table: "Show",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageFilename",
                table: "Show");
        }
    }
}
