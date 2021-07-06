using Microsoft.EntityFrameworkCore.Migrations;

namespace BGFolklore.Data.Migrations
{
    public partial class CreateTownAreaIdIndex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Towns_AreaId",
                table: "Towns",
                column: "AreaId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Towns_AreaId",
                table: "Towns");
        }
    }
}
