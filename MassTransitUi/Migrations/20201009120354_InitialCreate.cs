using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MassTransitUi.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FailedMessages",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    MessageId = table.Column<string>(nullable: true),
                    Queue = table.Column<string>(nullable: true),
                    RecievedTsUtc = table.Column<DateTime>(nullable: false),
                    Content = table.Column<byte[]>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FailedMessages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FailedMessageHeader",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Key = table.Column<string>(nullable: true),
                    Value = table.Column<string>(nullable: true),
                    FailedMessageId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FailedMessageHeader", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FailedMessageHeader_FailedMessages_FailedMessageId",
                        column: x => x.FailedMessageId,
                        principalTable: "FailedMessages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FailedMessageHeader_FailedMessageId",
                table: "FailedMessageHeader",
                column: "FailedMessageId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FailedMessageHeader");

            migrationBuilder.DropTable(
                name: "FailedMessages");
        }
    }
}
