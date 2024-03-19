using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebBuilder2.Server.Migrations
{
    /// <inheritdoc />
    public partial class AddSiteRepositoryTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SiteId",
                table: "Repository",
                newName: "SiteRepositoryId");

            migrationBuilder.CreateTable(
                name: "SiteRepository",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SiteId = table.Column<long>(type: "bigint", nullable: false),
                    RepositoryId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedDateTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SiteRepository", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SiteRepository_Repository_RepositoryId",
                        column: x => x.RepositoryId,
                        principalTable: "Repository",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SiteRepository_Site_SiteId",
                        column: x => x.SiteId,
                        principalTable: "Site",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SiteRepository_RepositoryId",
                table: "SiteRepository",
                column: "RepositoryId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SiteRepository_SiteId",
                table: "SiteRepository",
                column: "SiteId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SiteRepository");

            migrationBuilder.RenameColumn(
                name: "SiteRepositoryId",
                table: "Repository",
                newName: "SiteId");

            migrationBuilder.CreateIndex(
                name: "IX_Repository_SiteId",
                table: "Repository",
                column: "SiteId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Repository_Site_SiteId",
                table: "Repository",
                column: "SiteId",
                principalTable: "Site",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
