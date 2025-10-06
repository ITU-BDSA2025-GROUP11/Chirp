using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Chirp.Razor.Migrations
{
    /// <inheritdoc />
    public partial class InitialDbSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Authors",
                columns: table => new
                {
                    userame = table.Column<string>(type: "TEXT", nullable: false),
                    email = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Authors", x => x.userame);
                });

            migrationBuilder.CreateTable(
                name: "Cheeps",
                columns: table => new
                {
                    timeStamp = table.Column<DateTime>(type: "TEXT", nullable: false),
                    text = table.Column<string>(type: "TEXT", nullable: false),
                    username = table.Column<string>(type: "TEXT", nullable: false),
                    Authoruserame = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cheeps", x => x.timeStamp);
                    table.ForeignKey(
                        name: "FK_Cheeps_Authors_Authoruserame",
                        column: x => x.Authoruserame,
                        principalTable: "Authors",
                        principalColumn: "userame");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cheeps_Authoruserame",
                table: "Cheeps",
                column: "Authoruserame");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Cheeps");

            migrationBuilder.DropTable(
                name: "Authors");
        }
    }
}
