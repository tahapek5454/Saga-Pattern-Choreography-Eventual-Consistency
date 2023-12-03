using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Stock.API.Migrations
{
    /// <inheritdoc />
    public partial class mig2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Stocks",
                columns: new[] { "Id", "Count", "ProductId" },
                values: new object[,]
                {
                    { new Guid("15aa2d9c-e5f3-441d-be44-d28b5b3db862"), 10, new Guid("540dba8c-5970-45a3-ab56-969c26677921") },
                    { new Guid("15d80b0f-16cc-4ad3-be79-cf367d935764"), 10, new Guid("fbf11afa-c125-4fa8-a046-5fe927d792c7") },
                    { new Guid("5425173e-fcae-4820-a5a1-c25cf249c14c"), 10, new Guid("b23cfb37-a81c-492d-9d99-5c4b68ece80b") },
                    { new Guid("654c2128-776e-4979-8940-b334a554b515"), 10, new Guid("12a54947-1543-4473-b7e4-58e2c99a8f90") },
                    { new Guid("7e2f2b5a-848b-4d6e-b246-9308d1f69f22"), 10, new Guid("74688be0-2c9a-4193-be98-3ea5e906a1f3") }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Stocks",
                keyColumn: "Id",
                keyValue: new Guid("15aa2d9c-e5f3-441d-be44-d28b5b3db862"));

            migrationBuilder.DeleteData(
                table: "Stocks",
                keyColumn: "Id",
                keyValue: new Guid("15d80b0f-16cc-4ad3-be79-cf367d935764"));

            migrationBuilder.DeleteData(
                table: "Stocks",
                keyColumn: "Id",
                keyValue: new Guid("5425173e-fcae-4820-a5a1-c25cf249c14c"));

            migrationBuilder.DeleteData(
                table: "Stocks",
                keyColumn: "Id",
                keyValue: new Guid("654c2128-776e-4979-8940-b334a554b515"));

            migrationBuilder.DeleteData(
                table: "Stocks",
                keyColumn: "Id",
                keyValue: new Guid("7e2f2b5a-848b-4d6e-b246-9308d1f69f22"));
        }
    }
}
