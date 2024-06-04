using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entities.Migrations
{
    public partial class InsertPerson_StoredProcedure : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string sp_InsertPerson = @"
                CREATE PROCEDURE [dbo].[InsertPerson]
                (@ID nvarchar(26), @PersonName nvarchar(50), @Email nvarchar(50), @DateOfBirth datetime2(7), @Gender nvarchar(30), @CountryID nvarchar(26), @Address nvarchar(200), @ReceiveNewsLetters bit)
                AS BEGIN
                    INSERT INTO [dbo].[Persons](ID, PersonName, Email, DateOfBirth, Gender, CountryID, Address, ReceiveNewsLetters) VALUES (@ID, @PersonName, @Email, @DateOfBirth, @Gender, @CountryID, @Address, @ReceiveNewsLetters)
                END
            ";
            migrationBuilder.Sql(sp_InsertPerson);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            string sp_InsertPerson = @"
                BEGIN
                    DROP PROCEDURE [dbo].[InsertPerson]
                END
            ";
            migrationBuilder.Sql(sp_InsertPerson);
        }
    }
}
