namespace CRUDApplication.Models.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class InputTypeAttribute : Attribute
    {
        public CustomInputType InputType { get; }

        public InputTypeAttribute(CustomInputType inputType)
        {
            InputType = inputType;
        }
    }
}
