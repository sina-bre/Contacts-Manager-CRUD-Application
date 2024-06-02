using Microsoft.EntityFrameworkCore.Storage.ValueConversion;


public class UlidToStringConverter : ValueConverter<Ulid, string>
{
    public UlidToStringConverter() : base(
        ulid => ulid.ToString(),
        str => Ulid.Parse(str))
    {
    }
}
