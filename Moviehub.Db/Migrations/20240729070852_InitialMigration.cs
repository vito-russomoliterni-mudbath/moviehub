﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Moviehub.Db.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cinema",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    name = table.Column<string>(type: "varchar(64)", nullable: false),
                    location = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cinema", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Movie",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    title = table.Column<string>(type: "varchar(128)", nullable: false),
                    releaseDate = table.Column<DateTime>(type: "date", nullable: false),
                    genre = table.Column<string>(type: "varchar(64)", nullable: false),
                    runtime = table.Column<int>(type: "INTEGER", nullable: false),
                    synopsis = table.Column<string>(type: "text", nullable: false),
                    director = table.Column<string>(type: "varchar(64)", nullable: false),
                    rating = table.Column<string>(type: "varchar(8)", nullable: false),
                    princessTheatreMovieId = table.Column<string>(type: "varchar(16)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Movie", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "MovieCinema",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    movieId = table.Column<int>(type: "INTEGER", nullable: false),
                    cinemaId = table.Column<int>(type: "INTEGER", nullable: false),
                    showtime = table.Column<DateTime>(type: "date", nullable: false),
                    ticketPrice = table.Column<decimal>(type: "decimal", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MovieCinema", x => x.id);
                    table.ForeignKey(
                        name: "FK_MovieCinema_Cinema_cinemaId",
                        column: x => x.cinemaId,
                        principalTable: "Cinema",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MovieCinema_Movie_movieId",
                        column: x => x.movieId,
                        principalTable: "Movie",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MovieCinema_cinemaId",
                table: "MovieCinema",
                column: "cinemaId");

            migrationBuilder.CreateIndex(
                name: "IX_MovieCinema_movieId",
                table: "MovieCinema",
                column: "movieId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MovieCinema");

            migrationBuilder.DropTable(
                name: "Cinema");

            migrationBuilder.DropTable(
                name: "Movie");
        }
    }
}
