using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Chirp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Mopper9 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cheeps_Authors_AuthorId",
                table: "Cheeps");

            migrationBuilder.AlterColumn<string>(
                name: "AuthorId",
                table: "Cheeps",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AddColumn<string>(
                name: "AuthorId1",
                table: "Cheeps",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "CheepId",
                table: "Authors",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CheepId1",
                table: "Authors",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Cheeps_AuthorId1",
                table: "Cheeps",
                column: "AuthorId1");

            migrationBuilder.CreateIndex(
                name: "IX_Authors_CheepId",
                table: "Authors",
                column: "CheepId");

            migrationBuilder.CreateIndex(
                name: "IX_Authors_CheepId1",
                table: "Authors",
                column: "CheepId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Authors_Cheeps_CheepId",
                table: "Authors",
                column: "CheepId",
                principalTable: "Cheeps",
                principalColumn: "CheepId");

            migrationBuilder.AddForeignKey(
                name: "FK_Authors_Cheeps_CheepId1",
                table: "Authors",
                column: "CheepId1",
                principalTable: "Cheeps",
                principalColumn: "CheepId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cheeps_Authors_AuthorId",
                table: "Cheeps",
                column: "AuthorId",
                principalTable: "Authors",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Cheeps_Authors_AuthorId1",
                table: "Cheeps",
                column: "AuthorId1",
                principalTable: "Authors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Authors_Cheeps_CheepId",
                table: "Authors");

            migrationBuilder.DropForeignKey(
                name: "FK_Authors_Cheeps_CheepId1",
                table: "Authors");

            migrationBuilder.DropForeignKey(
                name: "FK_Cheeps_Authors_AuthorId",
                table: "Cheeps");

            migrationBuilder.DropForeignKey(
                name: "FK_Cheeps_Authors_AuthorId1",
                table: "Cheeps");

            migrationBuilder.DropIndex(
                name: "IX_Cheeps_AuthorId1",
                table: "Cheeps");

            migrationBuilder.DropIndex(
                name: "IX_Authors_CheepId",
                table: "Authors");

            migrationBuilder.DropIndex(
                name: "IX_Authors_CheepId1",
                table: "Authors");

            migrationBuilder.DropColumn(
                name: "AuthorId1",
                table: "Cheeps");

            migrationBuilder.DropColumn(
                name: "CheepId",
                table: "Authors");

            migrationBuilder.DropColumn(
                name: "CheepId1",
                table: "Authors");

            migrationBuilder.AlterColumn<string>(
                name: "AuthorId",
                table: "Cheeps",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Cheeps_Authors_AuthorId",
                table: "Cheeps",
                column: "AuthorId",
                principalTable: "Authors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
