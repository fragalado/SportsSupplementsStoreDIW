using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class v62 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rel_Orden_Carritos_Carritos_carritoid_carrito",
                table: "Rel_Orden_Carritos");

            migrationBuilder.DropIndex(
                name: "IX_Rel_Orden_Carritos_carritoid_carrito",
                table: "Rel_Orden_Carritos");

            migrationBuilder.DropColumn(
                name: "carritoid_carrito",
                table: "Rel_Orden_Carritos");

            migrationBuilder.CreateIndex(
                name: "IX_Rel_Orden_Carritos_id_carrito",
                table: "Rel_Orden_Carritos",
                column: "id_carrito");

            migrationBuilder.AddForeignKey(
                name: "FK_Rel_Orden_Carritos_Carritos_id_carrito",
                table: "Rel_Orden_Carritos",
                column: "id_carrito",
                principalTable: "Carritos",
                principalColumn: "id_carrito",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rel_Orden_Carritos_Carritos_id_carrito",
                table: "Rel_Orden_Carritos");

            migrationBuilder.DropIndex(
                name: "IX_Rel_Orden_Carritos_id_carrito",
                table: "Rel_Orden_Carritos");

            migrationBuilder.AddColumn<long>(
                name: "carritoid_carrito",
                table: "Rel_Orden_Carritos",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Rel_Orden_Carritos_carritoid_carrito",
                table: "Rel_Orden_Carritos",
                column: "carritoid_carrito");

            migrationBuilder.AddForeignKey(
                name: "FK_Rel_Orden_Carritos_Carritos_carritoid_carrito",
                table: "Rel_Orden_Carritos",
                column: "carritoid_carrito",
                principalTable: "Carritos",
                principalColumn: "id_carrito");
        }
    }
}
