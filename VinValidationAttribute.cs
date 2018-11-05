using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using System.Collections;


namespace shapemetrics.VinValidation
{

    /// <summary>
    /// Usage is [VinValidation] tag on a property/field of a model
    /// </summary>

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class VinValidation : ValidationAttribute
    {
        private readonly int[] intWeights = { 8, 7, 6, 5, 4, 3, 2, 10, 0, 9, 8, 7, 6, 5, 4, 3, 2 };
        private readonly Dictionary<char, int> replaceValues;
        private readonly Dictionary<char, int> yearValues;

        public VinValidation()
        {
            replaceValues = new Dictionary<char, int>();
            replaceValues.Add('A', 1);
            replaceValues.Add('B', 2);
            replaceValues.Add('C', 3);
            replaceValues.Add('D', 4);
            replaceValues.Add('E', 5);
            replaceValues.Add('F', 6);
            replaceValues.Add('G', 7);
            replaceValues.Add('H', 8);
            replaceValues.Add('J', 1);
            replaceValues.Add('K', 2);
            replaceValues.Add('L', 3);
            replaceValues.Add('M', 4);
            replaceValues.Add('N', 5);
            replaceValues.Add('P', 7);
            replaceValues.Add('R', 9);
            replaceValues.Add('S', 2);
            replaceValues.Add('T', 3);
            replaceValues.Add('U', 4);
            replaceValues.Add('V', 5);
            replaceValues.Add('W', 6);
            replaceValues.Add('X', 7);
            replaceValues.Add('Y', 8);
            replaceValues.Add('Z', 9);
            replaceValues.Add('1', 1);
            replaceValues.Add('2', 2);
            replaceValues.Add('3', 3);
            replaceValues.Add('4', 4);
            replaceValues.Add('5', 5);
            replaceValues.Add('6', 6);
            replaceValues.Add('7', 7);
            replaceValues.Add('8', 8);
            replaceValues.Add('9', 9);
            replaceValues.Add('0', 0);

            yearValues = new Dictionary<char, int>(replaceValues);
            yearValues.Remove('0');
            yearValues.Remove('Z');
            yearValues.Remove('U');

        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {

            string p_strVin = (value == null ? "":((string)value).ToUpper().Trim());

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
                return new ValidationResult(String.Format("{0} contains an invalid check digit {1}", validationContext.DisplayName, check));
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
