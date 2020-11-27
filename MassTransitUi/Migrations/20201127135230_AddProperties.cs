using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MassTransitUi.Migrations
{
    public partial class AddProperties : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FailedMessageHeader_FailedMessages_FailedMessageId",
                table: "FailedMessageHeader");

            migrationBuilder.AddColumn<byte[]>(
                name: "Properties",
                table: "FailedMessages",
                type: "BLOB",
                nullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "FailedMessageId",
                table: "FailedMessageHeader",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_FailedMessageHeader_FailedMessages_FailedMessageId",
                table: "FailedMessageHeader",
                column: "FailedMessageId",
                principalTable: "FailedMessages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FailedMessageHeader_FailedMessages_FailedMessageId",
                table: "FailedMessageHeader");

            migrationBuilder.DropColumn(
                name: "Properties",
                table: "FailedMessages");

            migrationBuilder.AlterColumn<long>(
                name: "FailedMessageId",
                table: "FailedMessageHeader",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "INTEGER");

            migrationBuilder.AddForeignKey(
                name: "FK_FailedMessageHeader_FailedMessages_FailedMessageId",
                table: "FailedMessageHeader",
                column: "FailedMessageId",
                principalTable: "FailedMessages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
