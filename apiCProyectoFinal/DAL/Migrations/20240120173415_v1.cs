using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class v1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Accesos",
                columns: table => new
                {
                    id_acceso = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    cod_acceso = table.Column<string>(type: "text", nullable: false),
                    desc_acceso = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accesos", x => x.id_acceso);
                });

            migrationBuilder.CreateTable(
                name: "Suplementos",
                columns: table => new
                {
                    id_suplemento = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nombre_suplemento = table.Column<string>(type: "text", nullable: false),
                    desc_suplemento = table.Column<string>(type: "text", nullable: false),
                    precio_suplemento = table.Column<long>(type: "bigint", nullable: false),
                    tipo_suplemento = table.Column<string>(type: "text", nullable: false),
                    marca_suplemento = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Suplementos", x => x.id_suplemento);
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    id_usuario = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nombre_usuario = table.Column<string>(type: "text", nullable: false),
                    tlf_usuario = table.Column<string>(type: "text", nullable: false),
                    email_usuario = table.Column<string>(type: "text", nullable: false),
                    psswd_usuario = table.Column<string>(type: "text", nullable: false),
                    id_acceso = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.id_usuario);
                    table.ForeignKey(
                        name: "FK_Usuarios_Accesos_id_acceso",
                        column: x => x.id_acceso,
                        principalTable: "Accesos",
                        principalColumn: "id_acceso",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Carritos",
                columns: table => new
                {
                    id_carrito = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    id_usuario = table.Column<long>(type: "bigint", nullable: false),
                    id_suplemento = table.Column<long>(type: "bigint", nullable: false),
                    cantidad = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Carritos", x => x.id_carrito);
                    table.ForeignKey(
                        name: "FK_Carritos_Suplementos_id_suplemento",
                        column: x => x.id_suplemento,
                        principalTable: "Suplementos",
                        principalColumn: "id_suplemento",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Carritos_Usuarios_id_usuario",
                        column: x => x.id_usuario,
                        principalTable: "Usuarios",
                        principalColumn: "id_usuario",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Ordenes",
                columns: table => new
                {
                    id_orden = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    id_usuario = table.Column<long>(type: "bigint", nullable: false),
                    precio_orden = table.Column<float>(type: "real", nullable: false),
                    fch_orden = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ordenes", x => x.id_orden);
                    table.ForeignKey(
                        name: "FK_Ordenes_Usuarios_id_usuario",
                        column: x => x.id_usuario,
                        principalTable: "Usuarios",
                        principalColumn: "id_usuario",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tokens",
                columns: table => new
                {
                    id_token = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    cod_token = table.Column<string>(type: "text", nullable: false),
                    fch_fin_token = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    id_usuario = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tokens", x => x.id_token);
                    table.ForeignKey(
                        name: "FK_Tokens_Usuarios_id_usuario",
                        column: x => x.id_usuario,
                        principalTable: "Usuarios",
                        principalColumn: "id_usuario",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Rel_Orden_Carritos",
                columns: table => new
                {
                    id_rel_orden_carrito = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    id_orden = table.Column<long>(type: "bigint", nullable: false),
                    id_carrito = table.Column<long>(type: "bigint", nullable: false),
                    carritoid_carrito = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rel_Orden_Carritos", x => x.id_rel_orden_carrito);
                    table.ForeignKey(
                        name: "FK_Rel_Orden_Carritos_Carritos_carritoid_carrito",
                        column: x => x.carritoid_carrito,
                        principalTable: "Carritos",
                        principalColumn: "id_carrito");
                    table.ForeignKey(
                        name: "FK_Rel_Orden_Carritos_Ordenes_id_orden",
                        column: x => x.id_orden,
                        principalTable: "Ordenes",
                        principalColumn: "id_orden",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Carritos_id_suplemento",
                table: "Carritos",
                column: "id_suplemento");

            migrationBuilder.CreateIndex(
                name: "IX_Carritos_id_usuario",
                table: "Carritos",
                column: "id_usuario");

            migrationBuilder.CreateIndex(
                name: "IX_Ordenes_id_usuario",
                table: "Ordenes",
                column: "id_usuario");

            migrationBuilder.CreateIndex(
                name: "IX_Rel_Orden_Carritos_carritoid_carrito",
                table: "Rel_Orden_Carritos",
                column: "carritoid_carrito");

            migrationBuilder.CreateIndex(
                name: "IX_Rel_Orden_Carritos_id_orden",
                table: "Rel_Orden_Carritos",
                column: "id_orden");

            migrationBuilder.CreateIndex(
                name: "IX_Tokens_id_usuario",
                table: "Tokens",
                column: "id_usuario");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_id_acceso",
                table: "Usuarios",
                column: "id_acceso");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Rel_Orden_Carritos");

            migrationBuilder.DropTable(
                name: "Tokens");

            migrationBuilder.DropTable(
                name: "Carritos");

            migrationBuilder.DropTable(
                name: "Ordenes");

            migrationBuilder.DropTable(
                name: "Suplementos");

            migrationBuilder.DropTable(
                name: "Usuarios");

            migrationBuilder.DropTable(
                name: "Accesos");
        }
    }
}
