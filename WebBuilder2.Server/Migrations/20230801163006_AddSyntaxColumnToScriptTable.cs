using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebBuilder2.Server.Migrations
{
    /// <inheritdoc />
    public partial class AddSyntaxColumnToScriptTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Syntax",
                table: "Script",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Syntax",
                table: "Script");
        }
    }
}
