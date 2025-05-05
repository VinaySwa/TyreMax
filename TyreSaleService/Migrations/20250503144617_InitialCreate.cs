using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TyreSaleService.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TyreCompanies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TyreCompanies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TyreModels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    CompanyId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TyreModels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TyreModels_TyreCompanies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "TyreCompanies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tyres",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Dimensions_Width = table.Column<int>(type: "INTEGER", nullable: false),
                    Dimensions_Profile = table.Column<int>(type: "INTEGER", nullable: false),
                    Dimensions_RimSize = table.Column<int>(type: "INTEGER", nullable: false),
                    ModelId = table.Column<int>(type: "INTEGER", nullable: false),
                    Price = table.Column<decimal>(type: "TEXT", nullable: false),
                    DiscountPercentage = table.Column<double>(type: "REAL", nullable: false),
                    LoadIndex = table.Column<int>(type: "INTEGER", nullable: false),
                    SpeedIndex = table.Column<string>(type: "TEXT", nullable: false),
                    Availability = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tyres", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tyres_TyreModels_ModelId",
                        column: x => x.ModelId,
                        principalTable: "TyreModels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TyreModels_CompanyId",
                table: "TyreModels",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Tyres_ModelId",
                table: "Tyres",
                column: "ModelId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tyres");

            migrationBuilder.DropTable(
                name: "TyreModels");

            migrationBuilder.DropTable(
                name: "TyreCompanies");
        }
    }
}
