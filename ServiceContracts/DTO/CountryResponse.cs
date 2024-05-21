namespace ServiceContracts.DTO
{
    /// <summary>
    /// DTO class that is used as return type for most of CountriesService methods
    /// </summary>
    public class CountryResponse
    {
        public Ulid CountryID { get; set; }
        public string? CountryName { get; set; }
    }
}
