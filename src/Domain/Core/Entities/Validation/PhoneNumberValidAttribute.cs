using System.ComponentModel.DataAnnotations;

namespace Core.Entities.Validation
{
    public class PhoneNumberValidAttribute : ValidationAttribute
    {
        public override bool IsValid(object? number)
        {
            if (number == null)
                return false;
            if (((string)number).Length > 12)
                return false;
            if (!((string)number).Substring(1).All(c => char.IsDigit(c)))
                return false;
            return true;
        }
    }
}
