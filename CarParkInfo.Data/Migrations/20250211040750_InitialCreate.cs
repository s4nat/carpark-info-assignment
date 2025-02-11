using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarParkInfo.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CarParks",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    CarParkNo = table.Column<string>(type: "TEXT", nullable: false),
                    Address = table.Column<string>(type: "TEXT", nullable: false),
                    XCoord = table.Column<float>(type: "REAL", nullable: false),
                    YCoord = table.Column<float>(type: "REAL", nullable: false),
                    CarParkType = table.Column<string>(type: "TEXT", nullable: false),
                    TypeOfParkingSystem = table.Column<string>(type: "TEXT", nullable: false),
                    ShortTermParking = table.Column<string>(type: "TEXT", nullable: false),
                    FreeParking = table.Column<string>(type: "TEXT", nullable: false),
                    NightParking = table.Column<string>(type: "TEXT", nullable: false),
                    CarParkDecks = table.Column<int>(type: "INTEGER", nullable: false),
                    GantryHeight = table.Column<float>(type: "REAL", nullable: false),
                    CarParkBasement = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarParks", x => x.Id);
                    table.UniqueConstraint("AK_CarParks_CarParkNo", x => x.CarParkNo);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    Username = table.Column<string>(type: "TEXT", nullable: false),
                    PasswordHash = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    LastLoginAt = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserFavorites",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    CarParkNo = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserFavorites", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserFavorites_CarParks_CarParkNo",
                        column: x => x.CarParkNo,
                        principalTable: "CarParks",
                        principalColumn: "CarParkNo",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserFavorites_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CarParks_CarParkNo",
                table: "CarParks",
                column: "CarParkNo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CarParks_FreeParking",
                table: "CarParks",
                column: "FreeParking");

            migrationBuilder.CreateIndex(
                name: "IX_CarParks_GantryHeight",
                table: "CarParks",
                column: "GantryHeight");

            migrationBuilder.CreateIndex(
                name: "IX_CarParks_NightParking",
                table: "CarParks",
                column: "NightParking");

            migrationBuilder.CreateIndex(
                name: "IX_UserFavorites_CarParkNo",
                table: "UserFavorites",
                column: "CarParkNo");

            migrationBuilder.CreateIndex(
                name: "IX_UserFavorites_UserId_CarParkNo",
                table: "UserFavorites",
                columns: new[] { "UserId", "CarParkNo" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserFavorites");

            migrationBuilder.DropTable(
                name: "CarParks");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
