namespace SpiceHouse.Data.Models.Attributes
{
    using System.Collections;
    using System.ComponentModel.DataAnnotations;

    public class RequiredCollectionAttribute : RequiredAttribute
    {
        public override bool IsValid(object value)
        {
            var isValid = base.IsValid(value);

            if (isValid)
            {
                if (value is ICollection collection)
                {
                    isValid = collection.Count != 0;
                }
            }

            return isValid;
        }
    }
}
