﻿using Entities;
using ServiceContracts.DTO.PersonDTO;
using ServiceContracts.Interfaces;
using Services.Helpers;

namespace Services
{
    public class PersonsService : IPersonsService
    {
        private readonly List<Person> _persons;
        private readonly ICountriesService _countriesService;

        public PersonsService()
        {
            _persons = new List<Person>();
            _countriesService = new CountriesService();
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
    }
}
