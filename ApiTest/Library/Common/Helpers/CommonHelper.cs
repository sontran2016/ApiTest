using System;
using System.Text.RegularExpressions;

namespace Common.Helpers
{
    public static class HelperRemoveApostrophes
    {
        /// <summary>
        /// Remove apostrophe (') in string
        /// </summary>
        /// <param name="str">string</param>
        /// <returns></returns>
        public static string RemoveApostrophe(this string str)
        {
            return !string.IsNullOrEmpty(str) ? str.Replace("'", "").Replace(";", "") : str;
        }
    }

    public class CommonHelper
    {
        /// <summary>
        /// Verifies that a string is in valid e-mail format
        /// </summary>
        /// <param name="email">Email to verify</param>
        /// <returns>true if the string is a valid e-mail address and false if it's not</returns>
        public static bool IsValidEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
                return false;

            email = email.Trim();
            var result = Regex.IsMatch(email,
                "^(?:[\\w\\!\\#\\$\\%\\&\\'\\*\\+\\-\\/\\=\\?\\^\\`\\{\\|\\}\\~]+\\.)*[\\w\\!\\#\\$\\%\\&\\'\\*\\+\\-\\/\\=\\?\\^\\`\\{\\|\\}\\~]+@(?:(?:(?:[a-zA-Z0-9](?:[a-zA-Z0-9\\-](?!\\.)){0,61}[a-zA-Z0-9]?\\.)+[a-zA-Z0-9](?:[a-zA-Z0-9\\-](?!$)){0,61}[a-zA-Z0-9]?)|(?:\\[(?:(?:[01]?\\d{1,2}|2[0-4]\\d|25[0-5])\\.){3}(?:[01]?\\d{1,2}|2[0-4]\\d|25[0-5])\\]))$",
                RegexOptions.IgnoreCase);
            return result;
        }

        /// <summary>
        /// Generate random password
        /// </summary>
        /// <param name="passwordLength"></param>
        /// <returns></returns>
        public static string RandomPassword(int passwordLength)
        {
            string allowedChars = "abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ0123456789!@$?_-";
            char[] chars = new char[passwordLength];
            Random rd = new Random();

            for (int i = 0; i < passwordLength; i++)
            {
                chars[i] = allowedChars[rd.Next(0, allowedChars.Length)];
            }

            return new string(chars);
        }

        /// <summary>
        /// Remove single quote
        /// </summary>
        /// <param name="sqlQuery"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static string NormalizeSqlQuery(string sqlQuery, params object[] parameters)
        {
            sqlQuery = sqlQuery.Replace("'", "\"");
            sqlQuery = string.Format(sqlQuery, parameters);
            sqlQuery = sqlQuery.Replace("'", "''");
            sqlQuery = sqlQuery.Replace("\"", "'");
            return sqlQuery;
        }
    }

    public class TaskType
    {
        public static string ScheduleTask => "ScheduleTask";
        public static string SmsSafetyService => "SmsSafetyService";

    }
}
