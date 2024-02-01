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
    public class ValidTitle : ValidationRule
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
        public int Min { get; set; }
        public int Max { get; set; }
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            try
            {
                string symbols = "#?$!-_~";
                string text = value.ToString();
                if (text.Length < Min)
                    throw new Exception("UserName too short");
                if (text.Length > Max)
                    throw new Exception("UserName too long");
                if (text.IndexOf(" ") != -1)
                    throw new Exception("UserName can not include space");

                for (int i = 0; i < text.Length; i++)
                {
                    if (!Char.IsLetter(text[i]) && !Char.IsDigit(text[i]) && symbols.IndexOf(text[i]) == -1)
                    {
                        throw new Exception($"What symbol did you find? only digit, letter and [{symbols}]");
                    }
                }
            }
            catch (Exception ex)
            {
                return new ValidationResult(false, ex.Message);
            }
            return ValidationResult.ValidResult;
        }
    }

    public class ValidPassword : ValidationRule
    {
        public int Min { get; set; }
        public int Max { get; set; }
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            bool upperExist = false, lowerExist = false, numExist = false, symbolExist = false;
            char current = ' ', prev = 'o', prev2 = ' ';
            try
            {
                string text = value.ToString();
                if (text.Length < Min)
                    throw new Exception("Password too short");
                if (text.Length > Max)
                    throw new Exception("Password too long");

                for (int i = 0; i < text.Length; i++)
                {
                    if (Char.IsUpper(text[i])) upperExist = true;
                    else if (Char.IsLower(text[i])) lowerExist = true;
                    else if (Char.IsNumber(text[i])) numExist = true;
                    else symbolExist = true;
                    prev2 = prev;
                    prev = current;
                    current = text[i];
                    if (prev == current && current == prev2)
                    {
                        throw new Exception("dont create a row of 3 of the same letters;");
                    }
                    if (Char.IsNumber(current) && Char.IsNumber(prev) && Char.IsNumber(prev2))
                    {
                        if ((current + 1 == prev && current + 2 == prev2) || (current - 1 == prev && current - 2 == prev2))
                        {
                            throw new Exception("there are 3 numbers in order");
                        }
                    }
                }
                if (!(upperExist && lowerExist && numExist && symbolExist))
                    throw new Exception("password must contain a capital letter, a regular letterm, a symbol and a number");
            }
            catch (Exception ex)
            {
                return new ValidationResult(false, ex.Message);
            }
            return ValidationResult.ValidResult;
        }
    }
    public class ValidPhone : ValidationRule
    {
        public int Len { get; set; }
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            try
            {
                string text = value.ToString();
                if (text.Length < Len)
                    return new ValidationResult(false, "Phone too short");
                if (text.Length > Len)
                    return new ValidationResult(false, "Phone too long");
                if (text.IndexOf(" ") != -1)
                    return new ValidationResult(false, "Phone must not include space");

                for (int i = 0; i < text.Length; i++)
                {
                    if (!Char.IsDigit(text[i]))
                    {
                        return new ValidationResult(false, "only digits...");
                    }
                }
            }
            catch (Exception ex)
            {
                return new ValidationResult(false, ex.Message);
            }
            return ValidationResult.ValidResult;
        }
    }
    public class ValidEmail : ValidationRule

    {

        public override ValidationResult Validate(object value,

            CultureInfo cultureInfo)

        {

            string email = value.ToString().Trim();

            Regex regex = new Regex(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$");

            bool res = regex.IsMatch(email);


            if (!res)

            {

                return new ValidationResult(res, "Email not valid");

            }

            return ValidationResult.ValidResult;

        }

    }
}
