using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace Masticore
{
    /// <summary>
    /// Validation property that confirms a given string
    /// </summary>
    [System.AttributeUsage(System.AttributeTargets.Property)]
    public class DateTimeStringValidationAttribute : ValidationAttribute
    {
        /// <summary>
        /// Gets or sets the expected format for this DateTime string
        /// </summary>
        public string Format { get; set; }

        /// <summary>
        /// Default constructor, sets the format to null
        /// </summary>
        public DateTimeStringValidationAttribute() 
        {
            Format = null;
        }

        /// <summary>
        /// Checks if the given value is a parse-able DateTime
        /// NOTE: This returns true if the value is null
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public override bool IsValid(object value)
        {
            try
            {
                ErrorMessage = "Unable to Convert Date";

                // We do not enforce null, that is the job of another attribute
                if (value == null)
                    return true;

                // Otherwise, extract the value and continue
                var val = value.ToString();

                // If we don't have a format, then just natural parse
                if (string.IsNullOrEmpty(Format))
                {
                    DateTime.Parse(val);
                }
                else
                {
                    // If we do have a format, then parse using that
                    var provider = CultureInfo.InvariantCulture;
                    DateTime.ParseExact(val, Format, provider);
                }

                // If no exceptions, then it must be good
                return true;
            }
            catch
            {
                // If we got an exception, it must be bad
                return false;
            }
        }

        /// <summary>
        /// Checks if the given value is parseable
        /// </summary>
        /// <param name="value"></param>
        /// <param name="validationContext"></param>
        /// <returns></returns>
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            return base.IsValid(value, validationContext);
        }
    }
}