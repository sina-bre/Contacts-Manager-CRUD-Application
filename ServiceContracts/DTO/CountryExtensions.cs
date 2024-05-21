using Entities;

namespace ServiceContracts.DTO
{
    public static class CountryExtensions
    {
        public static CountryResponse ToCountryRespone(this Country country)
        {
            return new CountryResponse
            {
                CountryID = country.Id,
                CountryName = country.Name,
            };
        }
    }
}
