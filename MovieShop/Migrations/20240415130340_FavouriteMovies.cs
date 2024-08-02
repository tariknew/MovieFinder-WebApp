using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovieShop.Migrations
{
    public partial class FavouriteMovies : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FavouriteMovies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    MovieId = table.Column<int>(type: "int", nullable: false),
                    DateAdded = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FavouriteMovies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FavouriteMovies_Movies_MovieId",
                        column: x => x.MovieId,
                        principalTable: "Movies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FavouriteMovies_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Movies",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreationDate",
                value: new DateTime(2024, 4, 15, 15, 3, 39, 655, DateTimeKind.Local).AddTicks(4577));

            migrationBuilder.UpdateData(
                table: "Movies",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreationDate",
                value: new DateTime(2024, 4, 15, 15, 3, 39, 656, DateTimeKind.Local).AddTicks(7534));

            migrationBuilder.UpdateData(
                table: "Movies",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreationDate",
                value: new DateTime(2024, 4, 15, 15, 3, 39, 657, DateTimeKind.Local).AddTicks(4485));

            migrationBuilder.CreateIndex(
                name: "IX_FavouriteMovies_MovieId",
                table: "FavouriteMovies",
                column: "MovieId");

            migrationBuilder.CreateIndex(
                name: "IX_FavouriteMovies_UserId",
                table: "FavouriteMovies",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FavouriteMovies");

            migrationBuilder.UpdateData(
                table: "Movies",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreationDate",
                value: new DateTime(2024, 3, 31, 15, 30, 28, 15, DateTimeKind.Local).AddTicks(1230));

            migrationBuilder.UpdateData(
                table: "Movies",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreationDate",
                value: new DateTime(2024, 3, 31, 15, 30, 28, 16, DateTimeKind.Local).AddTicks(4496));

            migrationBuilder.UpdateData(
                table: "Movies",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreationDate",
                value: new DateTime(2024, 3, 31, 15, 30, 28, 17, DateTimeKind.Local).AddTicks(1627));
        }
    }
}
