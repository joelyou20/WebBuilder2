using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebBuilder2.Server.Migrations
{
    /// <inheritdoc />
    public partial class InitMigrations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Repositories",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
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
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GitUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedDateTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Repositories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sites",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedDateTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sites", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Repositories");

            migrationBuilder.DropTable(
                name: "Sites");
        }
    }
}
