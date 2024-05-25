﻿using Entities;
using ServiceContracts.DTO.Enums;
using ServiceContracts.DTO.PersonDTO;
using ServiceContracts.Interfaces;
using Services.Helpers;
using System.Reflection;
using System.Text.Json;

namespace Services
{
    public class PersonsService : IPersonsService
    {
        private readonly List<Person> _persons;
        private readonly ICountriesService _countriesService;


        public PersonsService(bool initialize = true)
        {
            _persons = new List<Person>();
            _countriesService = new CountriesService();

            if (initialize)
            {
                string assemblyLocation = Assembly.GetExecutingAssembly().Location;
                string? assemblyDirectory = Path.GetDirectoryName(assemblyLocation);
                if (assemblyDirectory is null)
                    throw new ArgumentNullException(nameof(assemblyDirectory));
                DirectoryInfo? projectDirectory = Directory.GetParent(assemblyDirectory)?.Parent?.Parent?.Parent;
                if (projectDirectory is null)
                {
                    throw new DirectoryNotFoundException($"Project directory was not found.");
                }
                string filePath = Path.Combine(projectDirectory.FullName, "Services", "mockData", "Persons.json");
                if (!File.Exists(filePath))
                {
                    throw new FileNotFoundException($"The file '{filePath}' was not found.");
                }
                string json = File.ReadAllText(filePath);
                List<Person>? persons = JsonSerializer.Deserialize<List<Person>>(json);

                if (persons is not null)
                    _persons.AddRange(persons);
            }
        }
        private PersonResponse ConvertPersonToPersonResponse(Person person)
        {
            PersonResponse personResponse = person.ToPerosnResponse();
            personResponse.CountryName = _countriesService.GetCountryByCountryID(person.CountryID)?.CountryName;

            return personResponse;

        }
        public PersonResponse AddPerson(PersonAddRequest? personAddRequest)
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
            _persons.Add(person);

            //convert the Person object into PersonResponse type
            return ConvertPersonToPersonResponse(person);
        }

        public List<PersonResponse> GetAllPersons()
        {
            List<PersonResponse> personResponses = _persons.Select(temp => temp.ToPerosnResponse()).ToList();

            return personResponses;
        }

        public PersonResponse? GetPersonByPersonId(Ulid? personID)
        {
            if (personID is null)
                return null;

            Person? matchedPerson = _persons.FirstOrDefault(temp => temp.ID == personID);

            if (matchedPerson is null)
                return null;

            PersonResponse personResponse = matchedPerson.ToPerosnResponse();

            return personResponse;
        }

        public List<PersonResponse> GetFilteredPersons(string searchBy, string searchString)
        {
            List<PersonResponse> allPersons = GetAllPersons();
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
                return valueString.Contains(searchString, StringComparison.OrdinalIgnoreCase);
            }).ToList();

            #region WithSwitchCases
            /*
            switch (searchBy)
            {
                case nameof(Person.PersonName):
                    matchingPersons = allPersons.Where(temp => !string.IsNullOrEmpty(temp.PersonName) ? temp.PersonName.Contains(searchString, StringComparison.OrdinalIgnoreCase) : true).ToList();
                    break;

                case nameof(Person.Email):
                    matchingPersons = allPersons.Where(temp => !string.IsNullOrEmpty(temp.Email) ? temp.Email.Contains(searchString, StringComparison.OrdinalIgnoreCase) : true).ToList();
                    break;

                case nameof(Person.DateOfBirth):
                    matchingPersons = allPersons.Where(temp => (temp.DateOfBirth is not null) ? temp.DateOfBirth.Value.ToString("dd MMMM yyyy").Contains(searchString, StringComparison.OrdinalIgnoreCase) : true).ToList();
                    break;

                case nameof(Person.Gender):
                    matchingPersons = allPersons.Where(temp => !string.IsNullOrEmpty(temp.Gender) ? temp.Gender.Contains(searchString, StringComparison.OrdinalIgnoreCase) : true).ToList();
                    break;

                case nameof(Person.CountryID):
                    matchingPersons = allPersons.Where(temp => !string.IsNullOrEmpty(temp.CountryName) ? temp.CountryName.Contains(searchString, StringComparison.OrdinalIgnoreCase) : true).ToList();
                    break;

                case nameof(Person.Address):
                    matchingPersons = allPersons.Where(temp => !string.IsNullOrEmpty(temp.Address) ? temp.Address.Contains(searchString, StringComparison.OrdinalIgnoreCase) : true).ToList();
                    break;

                default:
                    matchingPersons = allPersons;
                    break;

            }
            return matchingPersons;
            */
            #endregion
        }

        public List<PersonResponse> GetSortedPersons(List<PersonResponse> allPersons, string sortBy, SortOrderOptions sortOrder)
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

        public PersonResponse UpdatePerson(PersonUpdateRequest? personUpdateRequest)
        {
            if (personUpdateRequest is null)
                throw new ArgumentNullException(nameof(personUpdateRequest));

            ValidationHelper.MedelValiadtion(personUpdateRequest);

            //get matching person object to update
            Person? matchingPerson = _persons.FirstOrDefault(temp => temp.ID == personUpdateRequest.PersonID);

            if (matchingPerson is null)
                throw new ArgumentException($"Given {nameof(matchingPerson)} doesn't exist");

            //update all details
            matchingPerson.PersonName = personUpdateRequest.PersonName;
            matchingPerson.Email = personUpdateRequest.Email;
            matchingPerson.DateOfBirth = personUpdateRequest.DateOfBirth;
            matchingPerson.Gender = personUpdateRequest.Gender.ToString();
            matchingPerson.CountryID = personUpdateRequest.CountryID;
            matchingPerson.Address = personUpdateRequest.Address;
            matchingPerson.ReciveNewsLetters = personUpdateRequest.ReciveNewsLetters;

            return matchingPerson.ToPerosnResponse();
        }

        public bool DeletePerson(Ulid? personID)
        {
            if (personID is null)
                throw new ArgumentNullException(nameof(personID));

            Person? person = _persons.FirstOrDefault(temp => temp.ID == personID);

            if (person is null)
                return false;

            _persons.RemoveAll(temp => temp.ID == person.ID);
            return true;
        }
    }
}
