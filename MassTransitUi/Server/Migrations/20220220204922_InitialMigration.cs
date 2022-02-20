using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MassTransitUi.Server.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FailedMessages",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    MessageId = table.Column<string>(type: "TEXT", nullable: false),
                    Queue = table.Column<string>(type: "TEXT", nullable: false),
                    ReceivedTsUtc = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Content = table.Column<byte[]>(type: "BLOB", nullable: false),
                    ErrorMessage = table.Column<string>(type: "TEXT", nullable: false),
                    Properties = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FailedMessages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FailedMessageHeader",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Key = table.Column<string>(type: "TEXT", nullable: false),
                    Value = table.Column<string>(type: "TEXT", nullable: false),
                    FailedMessageId = table.Column<long>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FailedMessageHeader", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FailedMessageHeader_FailedMessages_FailedMessageId",
                        column: x => x.FailedMessageId,
                        principalTable: "FailedMessages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
