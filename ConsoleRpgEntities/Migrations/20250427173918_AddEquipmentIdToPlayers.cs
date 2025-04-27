using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ConsoleRpgEntities.Migrations
{
    public partial class AddEquipmentIdToPlayer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Add the EquipmentId column to the Players table
            migrationBuilder.AddColumn<int>(
                name: "EquipmentId",
                table: "Players",
                type: "int",
                nullable: false,
                defaultValue: 0);

            // Create a unique index on the EquipmentId column
            migrationBuilder.CreateIndex(
                name: "IX_Players_EquipmentId",
                table: "Players",
                column: "EquipmentId",
                unique: true);

            // Add a foreign key constraint between Players.EquipmentId and Equipment.Id
            migrationBuilder.AddForeignKey(
                name: "FK_Players_Equipment_EquipmentId",
                table: "Players",
                column: "EquipmentId",
                principalTable: "Equipment",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Remove the foreign key constraint
            migrationBuilder.DropForeignKey(
                name: "FK_Players_Equipment_EquipmentId",
                table: "Players");

            // Remove the unique index on the EquipmentId column
            migrationBuilder.DropIndex(
                name: "IX_Players_EquipmentId",
                table: "Players");

            // Remove the EquipmentId column from the Players table
            migrationBuilder.DropColumn(
                name: "EquipmentId",
                table: "Players");
        }
    }
}
