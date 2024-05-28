namespace CRUDApplication.Models.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class OptionsAttribute : Attribute
    {
        public string[] Options { get; }

        public OptionsAttribute(params string[] options)
        {
            Options = options;
        }
    }
}
