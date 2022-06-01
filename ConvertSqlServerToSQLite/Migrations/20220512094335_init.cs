using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ConvertSqlServerToSQLite.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "personne",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    name = table.Column<string>(type: "TEXT", fixedLength: true, maxLength: 20, nullable: false),
                    country = table.Column<string>(type: "TEXT", fixedLength: true, maxLength: 20, nullable: false),
                    old = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_personne", x => x.id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "personne");
        }
    }
}
