using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RecommendationAlgo.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "VideoStats",
                columns: table => new
                {
                    VideoId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    VideoLength = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    Category = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VideoStats", x => x.VideoId);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "WatchHistories",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    VideoId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    WatchedTime = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Liked = table.Column<int>(type: "int", nullable: false),
                    FullyWatched = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WatchHistories", x => new { x.UserId, x.VideoId });
                    table.ForeignKey(
                        name: "FK_WatchHistories_VideoStats_VideoId",
                        column: x => x.VideoId,
                        principalTable: "VideoStats",
                        principalColumn: "VideoId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_WatchHistories_VideoId",
                table: "WatchHistories",
                column: "VideoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WatchHistories");

            migrationBuilder.DropTable(
                name: "VideoStats");
        }
    }
}
