using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BGFolklore.Data.Migrations
{
    public partial class CreateRating : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Rating",
                columns: table => new
                {
                    Id = table.Column<byte[]>(type: "varbinary(16)", nullable: false),
                    Rate = table.Column<int>(type: "int", nullable: false),
                    OwnerId = table.Column<string>(type: "varchar(36)", nullable: false),
                    EventId = table.Column<byte[]>(type: "varbinary(16)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rating", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Rating_AspNetUsers_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Rating_PublicEvents_EventId",
                        column: x => x.EventId,
                        principalTable: "PublicEvents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Rating_EventId",
                table: "Rating",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_Rating_OwnerId",
                table: "Rating",
                column: "OwnerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Rating");
        }
    }
}
