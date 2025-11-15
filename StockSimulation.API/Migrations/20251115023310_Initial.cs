using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StockSimulation.API.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Companies",
                columns: table => new
                {
                    Symbol = table.Column<string>(type: "TEXT", nullable: false),
                    ExchangeSymbol = table.Column<string>(type: "TEXT", nullable: false),
                    CompanyName = table.Column<string>(type: "TEXT", nullable: false),
                    Currency = table.Column<string>(type: "TEXT", nullable: false),
                    ExchangeName = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Companies", x => new { x.Symbol, x.ExchangeSymbol });
                });

            migrationBuilder.CreateTable(
                name: "StockPrice",
                columns: table => new
                {
                    StockSymbol = table.Column<string>(type: "TEXT", nullable: false),
                    ExchangeSymbol = table.Column<string>(type: "TEXT", nullable: false),
                    Date = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    Price = table.Column<float>(type: "REAL", nullable: false),
                    CompanyExchangeSymbol = table.Column<string>(type: "TEXT", nullable: true),
                    CompanySymbol = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockPrice", x => new { x.StockSymbol, x.ExchangeSymbol, x.Date });
                    table.ForeignKey(
                        name: "FK_StockPrice_Companies_CompanySymbol_CompanyExchangeSymbol",
                        columns: x => new { x.CompanySymbol, x.CompanyExchangeSymbol },
                        principalTable: "Companies",
                        principalColumns: new[] { "Symbol", "ExchangeSymbol" });
                });

            migrationBuilder.CreateIndex(
                name: "IX_StockPrice_CompanySymbol_CompanyExchangeSymbol",
                table: "StockPrice",
                columns: new[] { "CompanySymbol", "CompanyExchangeSymbol" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StockPrice");

            migrationBuilder.DropTable(
                name: "Companies");
        }
    }
}
