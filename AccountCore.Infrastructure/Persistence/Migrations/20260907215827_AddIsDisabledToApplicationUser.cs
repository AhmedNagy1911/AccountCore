using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AccountCore.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddIsDisabledToApplicationUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Disabled",
                table: "AspNetUsers",
                newName: "IsDisabled");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsDisabled",
                table: "AspNetUsers",
                newName: "Disabled");
        }
    }
}
