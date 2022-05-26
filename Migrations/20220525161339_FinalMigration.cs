using Microsoft.EntityFrameworkCore.Migrations;

namespace DisneyChallengeV2.Migrations
{
    public partial class FinalMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Generos",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Generos",
                keyColumn: "Id",
                keyValue: 2);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Generos",
                columns: new[] { "Id", "Imagen", "Nombre" },
                values: new object[] { 1, "https://i0.wp.com/imagenesparapeques.com/wp-content/uploads/2016/11/mickey-and-the-roadster-racers-png-mickey-aventura-sobre-ruedas-imagenes-mickey-sobre-ruedas.png?w=454&ssl=1", "Aventura" });

            migrationBuilder.InsertData(
                table: "Generos",
                columns: new[] { "Id", "Imagen", "Nombre" },
                values: new object[] { 2, "https://i0.wp.com/imagenesparapeques.com/wp-content/uploads/2016/11/mickey-and-the-roadster-racers-png-mickey-aventura-sobre-ruedas-imagenes-mickey-sobre-ruedas.png?w=454&ssl=1", "Familiar" });
        }
    }
}
