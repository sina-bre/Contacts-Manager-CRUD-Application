﻿using ServiceContracts.DTO.Enums;

namespace ServiceContracts.DTO.PersonDTO
{
    /// <summary>
    /// Represents DTo class thst is used as return type of mosrt methods of Persons Service 
    /// </summary>
    public class PersonResponse
    {
        public Ulid PersonID { get; set; }
        public string? PersonName { get; set; }
        public string? Email { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Gender { get; set; }
        public Ulid? CountryID { get; set; }
        public string? CountryName { get; set; }
        public string? Address { get; set; }
        public bool ReceiveNewsLetters { get; set; }
        public double? Age { get; set; }

        /// <summary>
        /// Compares the current object data with parameter object 
        /// </summary>
        /// <param name="obj">The PersonResponse object to compare</param>
        /// <returns>True or False, indicating whether all person details are matched with the specified parameter object</returns>
        public override bool Equals(object? obj)
        {
            if (obj is null) return false;

            if (obj.GetType() != typeof(PersonResponse))
            {
                return false;
            }
            PersonResponse personToCompare = (PersonResponse)obj;
            return PersonID == personToCompare.PersonID && PersonName == personToCompare.PersonName && Email == personToCompare.Email && DateOfBirth == personToCompare.DateOfBirth && Gender == personToCompare.Gender && CountryID == personToCompare.CountryID && Address == personToCompare.Address && ReceiveNewsLetters == personToCompare.ReceiveNewsLetters;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        public override string ToString()
        {
            return $"PersonID: {PersonID}, " +
                   $"PersonName: {PersonName ?? "N/A"}, " +
                   $"Email: {Email ?? "N/A"}, " +
                   $"DateOfBirth: {DateOfBirth?.ToString("yyyy-MM-dd") ?? "N/A"}, " +
                   $"Gender: {Gender ?? "N/A"}, " +
                   $"CountryID: {CountryID?.ToString() ?? "N/A"}, " +
                   $"CountryName: {CountryName ?? "N/A"}, " +
                   $"Address: {Address ?? "N/A"}, " +
                   $"ReceiveNewsLetters: {ReceiveNewsLetters}, " +
                   $"Age: {Age?.ToString() ?? "N/A"}";
        }
        public PersonUpdateRequest ToPersonUpdateRequest()
        {
            return new PersonUpdateRequest()
            {
                PersonID = PersonID,
                PersonName = PersonName,
                Email = Email,
                DateOfBirth = DateOfBirth,
                Gender = (GenderOptions)Enum.Parse(typeof(GenderOptions), Gender!, true),
                Address = Address,
                CountryID = CountryID,
                ReceiveNewsLetters = ReceiveNewsLetters
            };
        }
    }
}

