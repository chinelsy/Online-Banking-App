using Microsoft.EntityFrameworkCore.Migrations;

namespace OnlineBanking.Domain.Migrations
{
    public partial class defaultPasswordField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "DefaultPassword",
                table: "Customers",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DefaultPassword",
                table: "Customers");
        }
    }
}
