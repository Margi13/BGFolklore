using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

namespace BGFolklore.Data.Migrations
{
    public partial class CreateEthnoAreasImagesAndVideos : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EthnographicAreas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    AreaName = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    MapImageFileName = table.Column<string>(type: "text", nullable: false),
                    ImagesPath = table.Column<string>(type: "text", nullable: false),
                    ImagesDescription = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: false),
                    VideosDescription = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EthnographicAreas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Images",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    EthnoAreaId = table.Column<int>(type: "int", nullable: false),
                    CostumeType = table.Column<int>(type: "int", nullable: false),
                    FileName = table.Column<string>(type: "text", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Images", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Images_EthnographicAreas_EthnoAreaId",
                        column: x => x.EthnoAreaId,
                        principalTable: "EthnographicAreas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Videos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    EthnoAreaId = table.Column<int>(type: "int", nullable: false),
                    YouTubeId = table.Column<string>(type: "text", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Videos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Videos_EthnographicAreas_EthnoAreaId",
                        column: x => x.EthnoAreaId,
                        principalTable: "EthnographicAreas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Images_EthnoAreaId",
                table: "Images",
                column: "EthnoAreaId");

            migrationBuilder.CreateIndex(
                name: "IX_Videos_EthnoAreaId",
                table: "Videos",
                column: "EthnoAreaId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Images");

            migrationBuilder.DropTable(
                name: "Videos");

            migrationBuilder.DropTable(
                name: "EthnographicAreas");
        }
    }
}
