using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Chirp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class LikedDislikedCheeps : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Authors_Cheeps_CheepId",
                table: "Authors");

            migrationBuilder.DropForeignKey(
                name: "FK_Authors_Cheeps_CheepId1",
                table: "Authors");

            migrationBuilder.DropIndex(
                name: "IX_Authors_CheepId",
                table: "Authors");

            migrationBuilder.DropIndex(
                name: "IX_Authors_CheepId1",
                table: "Authors");

            migrationBuilder.DropColumn(
                name: "CheepId",
                table: "Authors");

            migrationBuilder.DropColumn(
                name: "CheepId1",
                table: "Authors");

            migrationBuilder.CreateTable(
                name: "CheepDislikes",
                columns: table => new
                {
                    DislikedCheepsCheepId = table.Column<int>(type: "INTEGER", nullable: false),
                    DislikesId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CheepDislikes", x => new { x.DislikedCheepsCheepId, x.DislikesId });
                    table.ForeignKey(
                        name: "FK_CheepDislikes_Authors_DislikesId",
                        column: x => x.DislikesId,
                        principalTable: "Authors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CheepDislikes_Cheeps_DislikedCheepsCheepId",
                        column: x => x.DislikedCheepsCheepId,
                        principalTable: "Cheeps",
                        principalColumn: "CheepId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CheepLikes",
                columns: table => new
                {
                    LikedCheepsCheepId = table.Column<int>(type: "INTEGER", nullable: false),
                    LikesId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CheepLikes", x => new { x.LikedCheepsCheepId, x.LikesId });
                    table.ForeignKey(
                        name: "FK_CheepLikes_Authors_LikesId",
                        column: x => x.LikesId,
                        principalTable: "Authors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CheepLikes_Cheeps_LikedCheepsCheepId",
                        column: x => x.LikedCheepsCheepId,
                        principalTable: "Cheeps",
                        principalColumn: "CheepId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CheepDislikes_DislikesId",
                table: "CheepDislikes",
                column: "DislikesId");

            migrationBuilder.CreateIndex(
                name: "IX_CheepLikes_LikesId",
                table: "CheepLikes",
                column: "LikesId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CheepDislikes");

            migrationBuilder.DropTable(
                name: "CheepLikes");

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
        }
    }
}
