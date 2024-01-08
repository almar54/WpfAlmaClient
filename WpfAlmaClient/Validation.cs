using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace WpfAlmaClient
{
    class Validation
    {
    }
    public class ValidBirthYear : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            try
            {
                int year = int.Parse(value.ToString());
                if (year < 1900)
                    return new ValidationResult(false, "Too old");
                if (year > DateTime.Today.Year)
                    return new ValidationResult(false, "No way!");
            }
            catch (Exception ex)
            {
                return new ValidationResult(false, "Error: " + ex.Message);
            }
            return ValidationResult.ValidResult;
        }
    }

    public class ValidName : ValidationRule
    {
        public int min { get; set; }
        public int max { get; set; }
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            try
            {
                string name = value.ToString();
                if (name.Length < min) // name too short
                    return new ValidationResult(false, "Too short");
                if (name.Length > max) // name too long
                    return new ValidationResult(false, "Too long");
                foreach (char c in name)
                    if (!Char.IsLetter(c) && c != ' ')
                        return new ValidationResult(false, "Only letters and spaces");
                if (!Char.IsUpper(name[0]))
                    return new ValidationResult(false, "Name must start with capital letter");
                if (!Char.IsLetter(name[name.Length - 1]))
                    return new ValidationResult(false, "Name can't end with space");
                if (name.IndexOf("  ") != -1)
                    return new ValidationResult(false, "Name must not include more than one space at a time");
            }
            catch (Exception ex)
            {
                return new ValidationResult(false, "Error: " + ex.Message);
            }
            return ValidationResult.ValidResult;
        }
    }

    public class ValidUserName : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            try
            {
                string username = value.ToString();
                if (username.Length < 6) // username too short
                    return new ValidationResult(false, "Too short");
                if (username.Length > 15) // username too long
                    return new ValidationResult(false, "Too long");
                foreach (char c in username)
                    if (!Char.IsLetterOrDigit(c) && c != '_')
                        return new ValidationResult(false, "Only letters, numbers and underscore");
                if (!Char.IsLetter(username[0]))
                    return new ValidationResult(false, "Username must start with a letter");
            }
            catch (Exception ex)
            {
                return new ValidationResult(false, "Error: " + ex.Message);
            }
            return ValidationResult.ValidResult;
        }
    }

    public class ValidPassword : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            try
            {
                string password = value.ToString();
                bool lower = false, upper = false, number = false;
                if (password.Length < 6) // password too short
                    return new ValidationResult(false, "Too short");
                if (password.Length > 15) // password too long
                    return new ValidationResult(false, "Too long");
                for (int i = 0; i < password.Length; i++)
                {
                    if (!Char.IsLetterOrDigit(password[i]))
                        return new ValidationResult(false, "Only letters and numbers");
                    else
                    {
                        if (Char.IsLower(password[i]))
                            lower = true;
                        else if (Char.IsUpper(password[i]))
                            upper = true;
                        else
                            number = true;
                    }
                    if (i > 1)
                    {
                        if (password[i - 1].Equals(password[i]) && password[i - 2].Equals(password[i]))
                            return new ValidationResult(false, "Can't have sequence of the same letter");
                        if (password[i - 1].Equals(password[i] - 1) && password[i - 2].Equals(password[i] - 2))
                            return new ValidationResult(false, "Can't have upper sequence");
                        if (password[i - 1].Equals(password[i] + 1) && password[i - 2].Equals(password[i] + 2))
                            return new ValidationResult(false, "Can't have lower sequence");
                    }
                }
                if (!lower || !upper || !number)
                    return new ValidationResult(false, "Password must include at least one capital letter, one lower letter and number");
                if (!Char.IsLetter(password[0]))
                    return new ValidationResult(false, "Password must start with a letter");
            }
            catch (Exception ex)
            {
                return new ValidationResult(false, "Error: " + ex.Message);
            }
            return ValidationResult.ValidResult;
        }
    }
}
