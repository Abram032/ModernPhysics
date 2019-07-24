using Microsoft.EntityFrameworkCore.Migrations;

namespace ModernPhysics.Web.Data.Migrations.Web
{
    public partial class AddedCategoryFriendlyName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FriendlyName",
                table: "Categories",
                maxLength: 64,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FriendlyName",
                table: "Categories");
        }
    }
}
