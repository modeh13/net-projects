using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EjercicioEventos
{
    public class Validations
    {
        public enum ValidationType
        {
            User,
            Pass,
            Code
        }

        public static bool ValidateInput(string text, ValidationType type)
        {
            bool result = false;
            string validUserChars = "abcdefghijklmnñopqrstuwxyz1234567890_";
            string validPassChars = "abcdefghijklmnñopqrstuwxyz1234567890*$%&/()?¿!¡";
            string validCodeChars = "1234567890";

            switch (type)
            {
                case ValidationType.User:
                    result = text.ToLower().All(c => validUserChars.Contains(c));
                    break;

                case ValidationType.Pass:
                    result = text.ToLower().All(c => validPassChars.Contains(c));
                    break;

                case ValidationType.Code:
                    result = text.ToLower().All(c => validCodeChars.Contains(c));
                    break;
            }

            return result;
        }
    }
}