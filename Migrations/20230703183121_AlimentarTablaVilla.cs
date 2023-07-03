using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MagicVilla_API.Migrations
{
    /// <inheritdoc />
    public partial class AlimentarTablaVilla : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Villas",
                columns: new[] { "Id", "Amenidad", "Detalle", "FechaActualizacion", "FechaCreacion", "ImagenUrl", "MetrosCuadrados", "Nombre", "Ocupantes", "Tarifa" },
                values: new object[,]
                {
                    { 1, "", "Detalle de la villa", new DateTime(2023, 7, 3, 12, 31, 21, 109, DateTimeKind.Local).AddTicks(8500), new DateTime(2023, 7, 3, 12, 31, 21, 109, DateTimeKind.Local).AddTicks(8514), "", 50.0, "Villa Real", 5, 200.0 },
                    { 2, "", "Detalle de la villa premium", new DateTime(2023, 7, 3, 12, 31, 21, 109, DateTimeKind.Local).AddTicks(8517), new DateTime(2023, 7, 3, 12, 31, 21, 109, DateTimeKind.Local).AddTicks(8517), "", 150.0, "Villa Premiun", 4, 1200.0 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
