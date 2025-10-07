using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Chirp.Razor.Migrations
{
    /// <inheritdoc />
    public partial class changesToAttributeNames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cheeps_Authors_Authoruserame",
                table: "Cheeps");

            migrationBuilder.RenameColumn(
                name: "Authoruserame",
                table: "Cheeps",
                newName: "Authorusername");

            migrationBuilder.RenameIndex(
                name: "IX_Cheeps_Authoruserame",
                table: "Cheeps",
                newName: "IX_Cheeps_Authorusername");

            migrationBuilder.RenameColumn(
                name: "userame",
                table: "Authors",
                newName: "username");

            migrationBuilder.AddForeignKey(
                name: "FK_Cheeps_Authors_Authorusername",
                table: "Cheeps",
                column: "Authorusername",
                principalTable: "Authors",
                principalColumn: "username");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cheeps_Authors_Authorusername",
                table: "Cheeps");

            migrationBuilder.RenameColumn(
                name: "Authorusername",
                table: "Cheeps",
                newName: "Authoruserame");

            migrationBuilder.RenameIndex(
                name: "IX_Cheeps_Authorusername",
                table: "Cheeps",
                newName: "IX_Cheeps_Authoruserame");

            migrationBuilder.RenameColumn(
                name: "username",
                table: "Authors",
                newName: "userame");

            migrationBuilder.AddForeignKey(
                name: "FK_Cheeps_Authors_Authoruserame",
                table: "Cheeps",
                column: "Authoruserame",
                principalTable: "Authors",
                principalColumn: "userame");
        }
    }
}
