using Microsoft.EntityFrameworkCore.Migrations;

namespace ModernPhysics.Web.Data.Migrations.Web
{
    public partial class AddedPageShortcut : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Shortcut",
                table: "Pages",
                maxLength: 500,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Shortcut",
                table: "Pages");
        }
    }
}
