using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VideoLibrary.Migrations
{
    /// <inheritdoc />
    public partial class verhuurId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "VerhuurdId",
                table: "Verhuringen",
                newName: "VerhuurId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "VerhuurId",
                table: "Verhuringen",
                newName: "VerhuurdId");
        }
    }
}
