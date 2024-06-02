﻿using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Text.Json;
namespace Entities
{
    public class PersonsDBContext : DbContext
    {

        public PersonsDBContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Person> Persons { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            var ulidConverter = new UlidToStringConverter();

            modelBuilder.Entity<Country>()
           .Property(country => country.ID)
           .HasConversion(ulidConverter);
            modelBuilder.Entity<Country>().ToTable("Countries");

            modelBuilder.Entity<Person>()
          .Property(person => person.ID)
          .HasConversion(ulidConverter);

            modelBuilder.Entity<Person>()
         .Property(person => person.CountryID)
         .HasConversion(ulidConverter);


            modelBuilder.Entity<Person>().ToTable("Persons");


            string assemblyLocation = Assembly.GetExecutingAssembly().Location;
            string? assemblyDirectory = Path.GetDirectoryName(assemblyLocation);
            if (assemblyDirectory is null)
                throw new ArgumentNullException(nameof(assemblyDirectory));
            DirectoryInfo? projectDirectory = Directory.GetParent(assemblyDirectory)?.Parent?.Parent?.Parent;
            if (projectDirectory is null)
            {
                throw new DirectoryNotFoundException($"Project directory was not found.");
            }
            string countriesFilePath = Path.Combine(projectDirectory.FullName, "Entities", "mockData", "Countries.json");
            if (!File.Exists(countriesFilePath))
            {
                throw new FileNotFoundException($"The file '{countriesFilePath}' was not found.");
            }
            string countriesJson = File.ReadAllText(countriesFilePath);
            List<Country>? countries =
           JsonSerializer.Deserialize<List<Country>>(countriesJson);

            if (countries is not null)
                foreach (Country country in countries)
                    modelBuilder.Entity<Country>().HasData(country);


            string personsFilePath = Path.Combine(projectDirectory.FullName, "Entities", "mockData", "Persons.json");
            if (!File.Exists(personsFilePath))
            {
                throw new FileNotFoundException($"The file '{personsFilePath}' was not found.");
            }
            string personsJson = File.ReadAllText(personsFilePath);
            List<Person>? persons =
           JsonSerializer.Deserialize<List<Person>>(personsJson);

            if (persons is not null)
                foreach (Person person in persons)
                    modelBuilder.Entity<Person>().HasData(person);
        }
        public List<Person> sp_GetAllPersons()
        {
            return Persons.FromSqlRaw("EXECUTE [dbo].[GetAllPersons]").ToList();
        }
        public void CleanupDatabase()
        {
            this.Database.ExecuteSqlRaw("IF OBJECT_ID('dbo.Countries', 'U') IS NOT NULL DROP TABLE dbo.Countries;");
            this.Database.ExecuteSqlRaw("IF OBJECT_ID('dbo.Persons', 'U') IS NOT NULL DROP TABLE dbo.Persons;");
            this.Database.ExecuteSqlRaw("DELETE FROM dbo.__EFMigrationsHistory;");
        }
    }
}
