using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HouseRentingSystem.Data.Migrations
{
    public partial class AddIsActiveColumnForSoftDeletingHouses : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Houses",
                keyColumn: "Id",
                keyValue: new Guid("81930f58-7f54-453c-b083-501bf0e0b058"));

            migrationBuilder.DeleteData(
                table: "Houses",
                keyColumn: "Id",
                keyValue: new Guid("c0a790bf-fb84-41f7-957a-60d26dcad0c3"));

            migrationBuilder.DeleteData(
                table: "Houses",
                keyColumn: "Id",
                keyValue: new Guid("ffb1be0b-2091-4b93-85ae-1ad54b8106f9"));

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Houses",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.InsertData(
                table: "Houses",
                columns: new[] { "Id", "Address", "AgentId", "CategoryId", "Description", "ImageUrl", "PricePerMonth", "RenterId", "Title" },
                values: new object[] { new Guid("c25dc5fd-be0a-4313-b286-fb2fbf58c975"), "Near the Sea Garden in Burgas, Bulgaria", new Guid("f5086448-a741-4b80-b0c7-0263eb4e3ae9"), 2, "It has the best comfort you will ever ask for. With two bedrooms, it is great for your family.", "https://cf.bstatic.com/xdata/images/hotel/max1024x768/179489660.jpg?k=2029f6d9589b49c95dcc9503a265e292c2cdfcb5277487a0050397c3f8dd545a&o=&hp=1", 1200.00m, null, "Family House Comfort" });

            migrationBuilder.InsertData(
                table: "Houses",
                columns: new[] { "Id", "Address", "AgentId", "CategoryId", "Description", "ImageUrl", "PricePerMonth", "RenterId", "Title" },
                values: new object[] { new Guid("d186de0d-ad37-4179-8095-0a92436cebd9"), "Boyana Neighbourhood, Sofia, Bulgaria", new Guid("f5086448-a741-4b80-b0c7-0263eb4e3ae9"), 2, "This luxurious house is everything you will need. It is just excellent.", "https://i.pinimg.com/originals/a6/f5/85/a6f5850a77633c56e4e4ac4f867e3c00.jpg", 2000.00m, null, "Grand House" });

            migrationBuilder.InsertData(
                table: "Houses",
                columns: new[] { "Id", "Address", "AgentId", "CategoryId", "Description", "ImageUrl", "PricePerMonth", "RenterId", "Title" },
                values: new object[] { new Guid("fa20c4c4-e011-4cb3-8a05-296e33aca1d7"), "North London, UK (near the border)", new Guid("f5086448-a741-4b80-b0c7-0263eb4e3ae9"), 3, "A big house for your whole family. Don't miss to buy a house with three bedrooms.", "https://www.luxury-architecture.net/wp-content/uploads/2017/12/1513217889-7597-FAIRWAYS-010.jpg", 2100.00m, new Guid("079f3635-f031-4c1e-9750-08db740aa10f"), "Big House Marina" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Houses",
                keyColumn: "Id",
                keyValue: new Guid("c25dc5fd-be0a-4313-b286-fb2fbf58c975"));

            migrationBuilder.DeleteData(
                table: "Houses",
                keyColumn: "Id",
                keyValue: new Guid("d186de0d-ad37-4179-8095-0a92436cebd9"));

            migrationBuilder.DeleteData(
                table: "Houses",
                keyColumn: "Id",
                keyValue: new Guid("fa20c4c4-e011-4cb3-8a05-296e33aca1d7"));

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Houses");

            migrationBuilder.InsertData(
                table: "Houses",
                columns: new[] { "Id", "Address", "AgentId", "CategoryId", "CreatedOn", "Description", "ImageUrl", "PricePerMonth", "RenterId", "Title" },
                values: new object[] { new Guid("81930f58-7f54-453c-b083-501bf0e0b058"), "Boyana Neighbourhood, Sofia, Bulgaria", new Guid("f5086448-a741-4b80-b0c7-0263eb4e3ae9"), 2, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "This luxurious house is everything you will need. It is just excellent.", "https://i.pinimg.com/originals/a6/f5/85/a6f5850a77633c56e4e4ac4f867e3c00.jpg", 2000.00m, null, "Grand House" });

            migrationBuilder.InsertData(
                table: "Houses",
                columns: new[] { "Id", "Address", "AgentId", "CategoryId", "CreatedOn", "Description", "ImageUrl", "PricePerMonth", "RenterId", "Title" },
                values: new object[] { new Guid("c0a790bf-fb84-41f7-957a-60d26dcad0c3"), "North London, UK (near the border)", new Guid("f5086448-a741-4b80-b0c7-0263eb4e3ae9"), 3, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "A big house for your whole family. Don't miss to buy a house with three bedrooms.", "https://www.luxury-architecture.net/wp-content/uploads/2017/12/1513217889-7597-FAIRWAYS-010.jpg", 2100.00m, new Guid("079f3635-f031-4c1e-9750-08db740aa10f"), "Big House Marina" });

            migrationBuilder.InsertData(
                table: "Houses",
                columns: new[] { "Id", "Address", "AgentId", "CategoryId", "CreatedOn", "Description", "ImageUrl", "PricePerMonth", "RenterId", "Title" },
                values: new object[] { new Guid("ffb1be0b-2091-4b93-85ae-1ad54b8106f9"), "Near the Sea Garden in Burgas, Bulgaria", new Guid("f5086448-a741-4b80-b0c7-0263eb4e3ae9"), 2, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "It has the best comfort you will ever ask for. With two bedrooms, it is great for your family.", "https://cf.bstatic.com/xdata/images/hotel/max1024x768/179489660.jpg?k=2029f6d9589b49c95dcc9503a265e292c2cdfcb5277487a0050397c3f8dd545a&o=&hp=1", 1200.00m, null, "Family House Comfort" });
        }
    }
}
