using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class v6 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "gtp_usuarios");

            migrationBuilder.RenameTable(
                name: "Usuarios",
                newName: "Usuarios",
                newSchema: "gtp_usuarios");

            migrationBuilder.RenameTable(
                name: "Tokens",
                newName: "Tokens",
                newSchema: "gtp_usuarios");

            migrationBuilder.RenameTable(
                name: "Accesos",
                newName: "Accesos",
                newSchema: "gtp_usuarios");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "Usuarios",
                schema: "gtp_usuarios",
                newName: "Usuarios");

            migrationBuilder.RenameTable(
                name: "Tokens",
                schema: "gtp_usuarios",
                newName: "Tokens");

            migrationBuilder.RenameTable(
                name: "Accesos",
                schema: "gtp_usuarios",
                newName: "Accesos");
        }
    }
}
