using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MVCCRUD.Migrations
{
    /// <inheritdoc />
    public partial class addbookstate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BookState",
                table: "Reservations",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BookState",
                table: "Reservations");
        }
    }
}
