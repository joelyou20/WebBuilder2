using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebBuilder2.Server.Migrations
{
    /// <inheritdoc />
    public partial class UpdateForeignKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Site_Repository_SiteId",
                table: "Site");

            migrationBuilder.DropIndex(
                name: "IX_Site_SiteId",
                table: "Site");

            migrationBuilder.DropColumn(
                name: "SiteId",
                table: "Site");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Repository_Site_SiteId",
                table: "Repository");

            migrationBuilder.DropIndex(
                name: "IX_Repository_SiteId",
                table: "Repository");

            migrationBuilder.AddColumn<long>(
                name: "SiteId",
                table: "Site",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_Site_SiteId",
                table: "Site",
                column: "SiteId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Site_Repository_SiteId",
                table: "Site",
                column: "SiteId",
                principalTable: "Repository",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
