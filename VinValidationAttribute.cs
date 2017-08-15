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
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {

            string p_strVin = "";
            if (value != null)
            {
                p_strVin = ((string)value).ToUpper().Trim();
            }


            int intValue = 0;
            //Multiplier weights for each position.
            int[] intWeights = { 8, 7, 6, 5, 4, 3, 2, 10, 0, 9, 8, 7, 6, 5, 4, 3, 2 };


            if (string.IsNullOrEmpty(p_strVin) || p_strVin.Length != 17)
            {
                return new ValidationResult(String.Format("Invalid length for {0}", validationContext.DisplayName));
            }


            //Default CheckDigitValue (in numeric format)
            int intCheckValue = 0;
            //Get the Check digit from VIN
            char check = p_strVin[8];
            //Get the Year from the VIN
            char year = p_strVin[9];
            //Ensure the check digit is 0-9 or X
            if (!(char.IsDigit(check) && check == 'X'))
            {
                return new ValidationResult(String.Format("Check Digit is invalid {0}", validationContext.DisplayName));
            }
            //Get the numeric value of the check digit, Has to be numeric here 
            else if (check != 'X')
            {
                char[] d = new char[] { check };
                intCheckValue = int.Parse(Encoding.ASCII.GetString(Encoding.ASCII.GetBytes(d)));
            }
            else
            {
                intCheckValue = 10;
            }

            //Hash table of the character replacement values
            Hashtable replaceValues = new Hashtable();
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


            //Make sure it is a Valid Year - Created the next 4 lines to correct U, Z & 0 from being in the list
            Hashtable yearValues = (Hashtable)replaceValues.Clone(); //Get a shallow copy of values
            yearValues.Remove('0');
            yearValues.Remove('Z');
            yearValues.Remove('U');
            if (!yearValues.Contains(year))
            {
                return new ValidationResult(String.Format("Year is invalid {0}", validationContext.DisplayName)); 
            }


            //Make sure characters valid values. 
            for (int i = 0; i < p_strVin.Length; i++)
            {
                if (!replaceValues.Contains(p_strVin[i]))
                {
                    return new ValidationResult(String.Format("Invalid Character for {0} at position {1}", validationContext.DisplayName, i + 1)); ;
                }
                intValue += (intWeights[i] * ((int)replaceValues[p_strVin[i]]));
            }


            if ((intValue % 11) == intCheckValue)
            {
                return ValidationResult.Success;
            }

            return new ValidationResult(String.Format("{0} contains an invalid Check digit {1}", validationContext.DisplayName, check)); ;
        }
    }

}
