using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovieShop.Migrations
{
    public partial class DatabaseSetup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Movies",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreationDate",
                value: new DateTime(2024, 3, 31, 12, 56, 28, 268, DateTimeKind.Local).AddTicks(3490));

            migrationBuilder.UpdateData(
                table: "Movies",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreationDate",
                value: new DateTime(2024, 3, 31, 12, 56, 28, 269, DateTimeKind.Local).AddTicks(7943));

            migrationBuilder.UpdateData(
                table: "Movies",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreationDate",
                value: new DateTime(2024, 3, 31, 12, 56, 28, 270, DateTimeKind.Local).AddTicks(5438));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Movies",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreationDate",
                value: new DateTime(2024, 3, 28, 13, 16, 59, 50, DateTimeKind.Local).AddTicks(6545));

            migrationBuilder.UpdateData(
                table: "Movies",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreationDate",
                value: new DateTime(2024, 3, 28, 13, 16, 59, 51, DateTimeKind.Local).AddTicks(9818));

            migrationBuilder.UpdateData(
                table: "Movies",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreationDate",
                value: new DateTime(2024, 3, 28, 13, 16, 59, 52, DateTimeKind.Local).AddTicks(6971));
        }
    }
}
