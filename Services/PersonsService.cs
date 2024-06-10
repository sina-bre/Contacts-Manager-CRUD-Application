using CsvHelper;
using Entities;
using Microsoft.EntityFrameworkCore;
using ServiceContracts.DTO.Enums;
using ServiceContracts.DTO.PersonDTO;
using ServiceContracts.Interfaces;
using Services.Helpers;
using System.Globalization;
using System.Reflection;

namespace Services
{
    public class PersonsService : IPersonsService
    {
        private readonly PersonsDBContext _dbContext;
        private readonly ICountriesService _countriesService;


        public PersonsService(PersonsDBContext personsDBContext, ICountriesService countriesService)
        {
            _dbContext = personsDBContext;
            _countriesService = countriesService;
        }

        public async Task<PersonResponse> AddPerson(PersonAddRequest? personAddRequest)
        {
            if (personAddRequest is null)
                throw new ArgumentNullException(nameof(personAddRequest));

            //Model Valiadtion
            ValidationHelper.MedelValiadtion(personAddRequest);

            //convert personAddrequest into Person type
            Person person = personAddRequest.ToPerson();

            //generate PersonID
            person.ID = Ulid.NewUlid();

            //add person object to persons list
            _dbContext.Persons.Add(person);
            await _dbContext.SaveChangesAsync();
            //_dbContext.sp_InsertPerson(person);

            //convert the Person object into PersonResponse type
            return person.ToPerosnResponse();
        }

        public async Task<List<PersonResponse>> GetAllPersons()
        {
            var persons = await _dbContext.Persons.Include("Country").ToListAsync();
            return persons.Select(temp => temp.ToPerosnResponse()).ToList();

            //return _dbContext.sp_GetAllPersons().Select(temp => temp.ToPersonResponse()).ToList();
        }

        public async Task<PersonResponse?> GetPersonByPersonId(Ulid? personID)
        {
            if (personID is null)
                return null;

            Person? matchedPerson = await _dbContext.Persons.Include("Country").FirstOrDefaultAsync(temp => temp.ID == personID);

            if (matchedPerson is null)
                return null;

            return matchedPerson.ToPerosnResponse();
        }

        public async Task<List<PersonResponse>> GetFilteredPersons(string searchBy, string? searchString)
        {
            List<PersonResponse> allPersons = await GetAllPersons();
            List<PersonResponse> matchingPersons = allPersons;

            if (string.IsNullOrEmpty(searchBy) || string.IsNullOrEmpty(searchString))
                return matchingPersons;

            PropertyInfo? property = typeof(PersonResponse).GetProperty(searchBy);
            if (property == null)
                return allPersons;
            return matchingPersons.Where(temp =>
            {
                object? value = property.GetValue(temp);
                if (value is null)
                    return false;

                string valueString = value is DateTime dateTime ? dateTime.ToString("dd MMMM yyyy") : value.ToString() ?? string.Empty;


                if (searchBy.Equals("Gender", StringComparison.OrdinalIgnoreCase))
                {
                    return valueString.Equals(searchString, StringComparison.OrdinalIgnoreCase);
                }
                else
                {
                    return valueString.Contains(searchString, StringComparison.OrdinalIgnoreCase);
                }
            }).ToList();
        }

        public async Task<List<PersonResponse>> GetSortedPersons(List<PersonResponse> allPersons, string sortBy, SortOrderOptions sortOrder)
        {
            if (string.IsNullOrEmpty(sortBy))
                return allPersons;

            PropertyInfo? property = typeof(PersonResponse).GetProperty(sortBy);

            return sortOrder switch
            {
                SortOrderOptions.ASC => allPersons.OrderBy(item => property?.GetValue(item)).ToList(),
                SortOrderOptions.DESC => allPersons.OrderByDescending(item => property?.GetValue(item)).ToList(),
                _ => allPersons
            };
        }

        public async Task<PersonResponse> UpdatePerson(PersonUpdateRequest? personUpdateRequest)
        {
            if (personUpdateRequest is null)
                throw new ArgumentNullException(nameof(personUpdateRequest));

            ValidationHelper.MedelValiadtion(personUpdateRequest);

            //get matching person object to update
            Person? matchingPerson = await _dbContext.Persons.FindAsync(personUpdateRequest.PersonID);

            var personType = typeof(Person);
            var updateRequestType = typeof(PersonUpdateRequest);
            if (matchingPerson is null)
                throw new ArgumentException();
            foreach (var property in updateRequestType.GetProperties())
            {
                var personProperty = personType.GetProperty(property.Name);
                var value = property.GetValue(personUpdateRequest);

                if (value is not null && personProperty is not null)
                {
                    if (property.Name == nameof(Person.Gender))
                    {
                        // Convert enum value to string
                        var enumValueAsString = value.ToString();
                        personProperty.SetValue(matchingPerson, enumValueAsString);
                    }
                    else
                    {
                        personProperty.SetValue(matchingPerson, value);
                    }
                }
            }
            await _dbContext.SaveChangesAsync();
            return matchingPerson.ToPerosnResponse();
        }

        public async Task<bool> DeletePerson(Ulid? personID)
        {
            if (personID is null)
                throw new ArgumentNullException(nameof(personID));

            Person? person = await _dbContext.Persons.FindAsync(personID);

            if (person is null)
                return false;

            _dbContext.Persons.Remove(_dbContext.Persons.First(temp => temp.ID == person.ID));
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<MemoryStream> GetPersonsCSV()
        {
            MemoryStream memoryStream = new();
            StreamWriter streamWriter = new(memoryStream);
            CsvWriter csvWriter = new(streamWriter, CultureInfo.InvariantCulture, leaveOpen: true);
            csvWriter.WriteHeader<PersonResponse>();
            csvWriter.NextRecord();
            List<PersonResponse> persons = await _dbContext.Persons.Include("Country").Select(temp => temp.ToPerosnResponse()).ToListAsync();

            await csvWriter.WriteRecordsAsync(persons);

            memoryStream.Position = 0;

            return memoryStream;
        }
    }
}
