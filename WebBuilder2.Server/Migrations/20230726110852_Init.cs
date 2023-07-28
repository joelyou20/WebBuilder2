using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebBuilder2.Server.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Repository",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SiteId = table.Column<long>(type: "bigint", nullable: false),
                    ExternalId = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RepoName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Visibility = table.Column<int>(type: "int", nullable: false),
                    IsPrivate = table.Column<bool>(type: "bit", nullable: false),
                    IsTemplate = table.Column<bool>(type: "bit", nullable: false),
                    AllowAutoMerge = table.Column<bool>(type: "bit", nullable: false),
                    AllowMergeCommit = table.Column<bool>(type: "bit", nullable: false),
                    AllowRebaseMerge = table.Column<bool>(type: "bit", nullable: false),
                    AllowSquashMerge = table.Column<bool>(type: "bit", nullable: false),
                    AutoInit = table.Column<bool>(type: "bit", nullable: false),
                    DeleteBranchOnMerge = table.Column<bool>(type: "bit", nullable: false),
                    GitIgnoreTemplate = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HasDownloads = table.Column<bool>(type: "bit", nullable: false),
                    HasIssues = table.Column<bool>(type: "bit", nullable: false),
                    HasProjects = table.Column<bool>(type: "bit", nullable: false),
                    HasWiki = table.Column<bool>(type: "bit", nullable: false),
                    Homepage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LicenseTemplate = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TeamId = table.Column<int>(type: "int", nullable: true),
                    UseSquashPrTitleAsDefault = table.Column<bool>(type: "bit", nullable: false),
                    HtmlUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GitUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedDateTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Repository", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Site",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SiteId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedDateTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Site", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Site_Repository_SiteId",
                        column: x => x.SiteId,
                        principalTable: "Repository",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Site_SiteId",
                table: "Site",
                column: "SiteId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Site");

            migrationBuilder.DropTable(
                name: "Repository");
        }
    }
}
