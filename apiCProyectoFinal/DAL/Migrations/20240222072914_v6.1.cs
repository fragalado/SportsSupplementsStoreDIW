using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class v61 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "gtp_usuarios_c");

            migrationBuilder.RenameTable(
                name: "Usuarios",
                schema: "gtp_usuarios",
                newName: "Usuarios",
                newSchema: "gtp_usuarios_c");

            migrationBuilder.RenameTable(
                name: "Tokens",
                schema: "gtp_usuarios",
                newName: "Tokens",
                newSchema: "gtp_usuarios_c");

            migrationBuilder.RenameTable(
                name: "Accesos",
                schema: "gtp_usuarios",
                newName: "Accesos",
                newSchema: "gtp_usuarios_c");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "gtp_usuarios");

            migrationBuilder.RenameTable(
                name: "Usuarios",
                schema: "gtp_usuarios_c",
                newName: "Usuarios",
                newSchema: "gtp_usuarios");

            migrationBuilder.RenameTable(
                name: "Tokens",
                schema: "gtp_usuarios_c",
                newName: "Tokens",
                newSchema: "gtp_usuarios");

            migrationBuilder.RenameTable(
                name: "Accesos",
                schema: "gtp_usuarios_c",
                newName: "Accesos",
                newSchema: "gtp_usuarios");
        }
    }
}
