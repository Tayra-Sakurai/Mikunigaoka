using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;

namespace Sakaishi.Validations
{
    public class StringNoWhiteSpaceValidationAttribute : ValidationAttribute
    {
        public StringNoWhiteSpaceValidationAttribute()
            : base() { }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            ResourceLoader loader = new("Sakaishi/Resources");
            string valueStr = value as string;

            if (string.IsNullOrWhiteSpace(valueStr))
                return new(loader.GetString("RequiredStringMessage"));

            return ValidationResult.Success;
        }
    }
}
