using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebBuilder2.Server.Migrations
{
    /// <inheritdoc />
    public partial class AddNameColumnToScriptTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Script",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Script");
        }
    }
}
