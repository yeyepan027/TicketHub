using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TicketHub.Migrations
{
    /// <inheritdoc />
    public partial class RemovePhotoPath : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PhotoPath",
                table: "Show");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PhotoPath",
                table: "Show",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
