using Microsoft.Windows.ApplicationModel.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sakaishi.Validations
{
    public class BalanceValidationAttribute : ValidationAttribute
    {
        public string PropertyName { get; }

        public BalanceValidationAttribute(string propertyName)
        {
            ArgumentNullException.ThrowIfNull(propertyName);

            PropertyName = propertyName;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            object instance = validationContext.ObjectInstance;
            object otherValue = instance.GetType().GetProperty(PropertyName).GetValue(instance);

            if (otherValue is not double numericOtherValue)
                return ValidationResult.Success;

            if (value is not double numericValue)
                return ValidationResult.Success;

            if (numericValue < 0 || numericOtherValue < 0)
                return new("The fields must be as large as 0 and one must be greater than 0 and the other must equal to 0.");

            if (numericOtherValue > 0 && numericValue != 0)
                return new("The fields must be as large as 0 and one must be greater than 0 and the other must equal to 0.");

            return ValidationResult.Success;
        }
    }
}
