using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entities.Migrations
{
    public partial class GetPersons_StoredProcedure : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string sp_GetAllPersons = @"
                IF NOT EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'GetAllPersons')
                BEGIN
                    EXEC('
                    CREATE PROCEDURE [dbo].[GetAllPersons]
                    AS BEGIN
                        SELECT ID, PersonName, Email, DateOfBirth, Gender, CountryID, Address, ReceiveNewsLetters FROM [dbo].[Persons]
                    END
                    ')
                END
            ";
            migrationBuilder.Sql(sp_GetAllPersons);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            string sp_GetAllPersons = @"
                IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'GetAllPersons')
                BEGIN
                    DROP PROCEDURE [dbo].[GetAllPersons]
                END
            ";
            migrationBuilder.Sql(sp_GetAllPersons);
        }
    }
}
