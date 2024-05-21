using Entities;
namespace ServiceContracts.DTO
{
    internal class CountryAddRequest
    {
        /// <summary>
        /// DTO class for adding a new country
        /// </summary>
        public string? CountryName { get; set; }

        public Country ToCountry()
        {
            return new Country()
            {
                Name = CountryName,
            };
        }
    }
}
