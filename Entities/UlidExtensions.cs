namespace Entities
{
    public class UlidExtensions
    {
        public Ulid Empty { get; } = new Ulid(new byte[16]);
    }
}
