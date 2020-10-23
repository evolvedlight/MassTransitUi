using Microsoft.EntityFrameworkCore.Migrations;

namespace MassTransitUi.Migrations
{
    public partial class AddFaultMessage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ErrorMessage",
                table: "FailedMessages",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ErrorMessage",
                table: "FailedMessages");
        }
    }
}
