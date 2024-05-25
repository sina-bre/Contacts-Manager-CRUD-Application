using Entities;

namespace ServiceContracts.DTO.CountryDTO
{
    public static class CountryExtensions
    {
        //Converts from Country object to CountryResponse object
        public static CountryResponse ToCountryRespone(this Country country)
        {
            return new CountryResponse
            {
                CountryID = country.ID,
                CountryName = country.Name,
            };
        }
    }
}
