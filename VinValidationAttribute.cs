using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Collections;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace shapemetrics.VinValidation
{

    /// <summary>
    /// Usage is [VinValidation] tag on a property/field of a model
    /// </summary>

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class VinValidation : ValidationAttribute, IClientModelValidator
    {
        private readonly int[] intWeights = { 8, 7, 6, 5, 4, 3, 2, 10, 0, 9, 8, 7, 6, 5, 4, 3, 2 };
        private readonly Dictionary<char, int> replaceValues;
        private readonly Dictionary<char, int> yearValues;

        public VinValidation()
        {
            replaceValues = new Dictionary<char, int>()
            {
                    {'A', 1},
                    {'B', 2},
                    {'C', 3},
                    {'D', 4},
                    {'E', 5},
                    {'F', 6},
                    {'G', 7},
                    {'H', 8},
                    {'J', 1},
                    {'K', 2},
                    {'L', 3},
                    {'M', 4},
                    {'N', 5},
                    {'P', 7},
                    {'R', 9},
                    {'S', 2},
                    {'T', 3},
                    {'U', 4},
                    {'V', 5},
                    {'W', 6},
                    {'X', 7},
                    {'Y', 8},
                    {'Z', 9},
                    {'1', 1},
                    {'2', 2},
                    {'3', 3},
                    {'4', 4},
                    {'5', 5},
                    {'6', 6},
                    {'7', 7},
                    {'8', 8},
                    {'9', 9},
                    {'0', 0}
            };

            yearValues = new Dictionary<char, int>(replaceValues);
            yearValues.Remove('0');
            yearValues.Remove('Z');
            yearValues.Remove('U');

        }

        private string displayName;

        public void AddValidation(ClientModelValidationContext context)
        {

            context.Attributes.TryAdd("data-val", "true");

            context.Attributes.TryAdd("data-val-vinlength", $"{displayName} has an invalid length");

            context.Attributes.TryAdd("data-val-vincheckdigit", $"{displayName} contains an invalid check digit value");


            context.Attributes.TryAdd("data-val-vinyear", $"{displayName} contains an invalid value for year");

            context.Attributes.TryAdd("data-val-vinchars", $"{displayName} contains an invalid value(s)");

            context.Attributes.TryAdd("data-val-vinvalidation", $"{displayName} contains an invalid check digit");


        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {

            displayName = validationContext.DisplayName;
            string p_strVin = (value == null ? "" : ((string)value).ToUpper().Trim());

            if (string.IsNullOrEmpty(p_strVin) || p_strVin.Length != 17)
            {
                return new ValidationResult(String.Format("{0} has an invalid length", validationContext.DisplayName));
            }

            //Default CheckDigitValue (in numeric format) 
            int intCheckValue = 0;
            //Get the Check digit from VIN
            char check = p_strVin[8];
            //Get the Year from the VIN
            char year = p_strVin[9];
            //Ensure the check digit is 0-9 or X
            if (!(char.IsDigit(check) || check == 'X'))
            {
                return new ValidationResult(String.Format("{0} contains an invalid check digit value", validationContext.DisplayName, check));
            }
            else
            {
                intCheckValue = (check != 'X' ? ((int)char.GetNumericValue(check)) : 10);
            }

            if (!yearValues.ContainsKey(year))
            {
                return new ValidationResult(String.Format("{0} contains an invalid value for year: {1}", validationContext.DisplayName, year));
            }

            //Make sure characters valid values. 
            if (p_strVin.ToCharArray().Where(x => !replaceValues.Keys.Contains(x)).Count() > 0)
            {
                return new ValidationResult(String.Format("{0} contains an invalid character", validationContext.DisplayName));
            }

            int intValue = p_strVin.ToCharArray().ToList().Select((x, index) => new { Value = (intWeights[index] * replaceValues[x]) }).Sum(x => x.Value);

            if ((intValue % 11) == intCheckValue)
            {
                return ValidationResult.Success;
            }

            return new ValidationResult(String.Format("{0} contains an invalid check digit: {1}", validationContext.DisplayName, check)); ;
        }
    }

}
