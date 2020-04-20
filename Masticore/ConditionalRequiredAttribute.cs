using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Masticore
{
    /// <summary>
    /// Attribute for enforcing a "Required" field on an item, which is conditionally disregarded if:
    /// The boolean value of the property at IgnoreFlagName is equal to IgnoreFlagValue
    /// </summary>
    [System.AttributeUsage(System.AttributeTargets.Property)]
    public class ConditionalRequiredAttribute : RequiredAttribute
    {
        /// <summary>
        /// Name of the property that will be conditionally checked
        /// </summary>
        public string IgnoreFlagName { get; set; }

        /// <summary>
        /// Value of the ignore property that enables bypassing required
        /// </summary>
        public bool IgnoreFlagValue { get; set; }

        /// <summary>
        /// Implements validation logic
        /// </summary>
        /// <param name="value"></param>
        /// <param name="validationContext"></param>
        /// <returns></returns>
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var instance = validationContext.ObjectInstance;
            var type = validationContext.ObjectType;
            var prop = type.GetProperty(IgnoreFlagName);
            var flag = (bool)prop.GetValue(instance);

            if (flag == IgnoreFlagValue)
                return null;
            else
            {
                // If the flag is not set for ignore, then more ahead with Required attribute's logic
                return base.IsValid(value, validationContext);
            }
        }
    }
}