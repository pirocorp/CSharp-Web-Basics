using Microsoft.EntityFrameworkCore.Migrations;

namespace SharedTrip.Migrations
{
    public partial class TablesRename : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserTrip_Trip_TripId",
                table: "UserTrip");

            migrationBuilder.DropForeignKey(
                name: "FK_UserTrip_User_UserId",
                table: "UserTrip");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserTrip",
                table: "UserTrip");

            migrationBuilder.DropPrimaryKey(
                name: "PK_User",
                table: "User");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Trip",
                table: "Trip");

            migrationBuilder.RenameTable(
                name: "UserTrip",
                newName: "UsersTrips");

            migrationBuilder.RenameTable(
                name: "User",
                newName: "Users");

            migrationBuilder.RenameTable(
                name: "Trip",
                newName: "Trips");

            migrationBuilder.RenameIndex(
                name: "IX_UserTrip_UserId",
                table: "UsersTrips",
                newName: "IX_UsersTrips_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UsersTrips",
                table: "UsersTrips",
                columns: new[] { "TripId", "UserId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                table: "Users",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Trips",
                table: "Trips",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UsersTrips_Trips_TripId",
                table: "UsersTrips",
                column: "TripId",
                principalTable: "Trips",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UsersTrips_Users_UserId",
                table: "UsersTrips",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UsersTrips_Trips_TripId",
                table: "UsersTrips");

            migrationBuilder.DropForeignKey(
                name: "FK_UsersTrips_Users_UserId",
                table: "UsersTrips");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UsersTrips",
                table: "UsersTrips");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Trips",
                table: "Trips");

            migrationBuilder.RenameTable(
                name: "UsersTrips",
                newName: "UserTrip");

            migrationBuilder.RenameTable(
                name: "Users",
                newName: "User");

            migrationBuilder.RenameTable(
                name: "Trips",
                newName: "Trip");

            migrationBuilder.RenameIndex(
                name: "IX_UsersTrips_UserId",
                table: "UserTrip",
                newName: "IX_UserTrip_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserTrip",
                table: "UserTrip",
                columns: new[] { "TripId", "UserId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_User",
                table: "User",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Trip",
                table: "Trip",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserTrip_Trip_TripId",
                table: "UserTrip",
                column: "TripId",
                principalTable: "Trip",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserTrip_User_UserId",
                table: "UserTrip",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
