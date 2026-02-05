using System;
using System.Globalization;
using System.Text.RegularExpressions;
using SignNow.Net.Exceptions;

namespace SignNow.Net.Extensions
{
    /// <summary>
    /// Extension methods for validating common SignNow data types.
    /// </summary>
    public static class ValidatorExtensions
    {
        /// <summary>
        /// Pattern for signNow identity (Document, invite...)
        /// The required format: 40 characters long, case-sensitive, letters and numbers, underscore allowed.
        /// </summary>
        private const string IdPattern = @"^[a-zA-Z0-9_]{40,40}$";

        /// <summary>
        /// Pattern for Email address validation
        /// The required valid email address: e.g john+1@gmail.com or john123@gmail.com
        /// </summary>
        private const string EmailPattern = @"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$";

        /// <summary>
        /// Validates signNow ID for documents, invites, etc...
        /// </summary>
        /// <param name="id">Identity of the document or invite.</param>
        /// <returns>True if ID is valid, false otherwise.</returns>
        public static bool IsValidId(this string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return false;

            var regex = new Regex(IdPattern, RegexOptions.None, TimeSpan.FromMilliseconds(100));
            return regex.IsMatch(id);
        }

        /// <summary>
        /// Validates signNow ID for documents, invites, etc...
        /// </summary>
        /// <param name="id">Identity of the document or invite.</param>
        /// <exception cref="ArgumentException">Invalid format of ID.</exception>
        public static string ValidateId(this string id)
        {
            var regex = new Regex(IdPattern, RegexOptions.None, TimeSpan.FromMilliseconds(100));

            if (regex.IsMatch(id) && !string.IsNullOrWhiteSpace(id)) return id;

            throw new ArgumentException(
                string.Format(CultureInfo.CurrentCulture, ExceptionMessages.InvalidFormatOfId, id), id);
        }

        /// <summary>
        /// Validates email addresses.
        /// </summary>
        /// <param name="email">Email address.</param>
        /// <returns>True if email is valid, false otherwise.</returns>
        public static bool IsValidEmail(this string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            var regex = new Regex(EmailPattern, RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(100));
            return regex.IsMatch(email);
        }

        /// <summary>
        /// Validates email addresses.
        /// </summary>
        /// <param name="email">Email address.</param>
        /// <returns>Valid email address.</returns>
        /// <exception cref="ArgumentException">if email address is not valid.</exception>
        public static string ValidateEmail(this string email)
        {
            var regex = new Regex(EmailPattern, RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(100));

            if (regex.IsMatch(email) && !string.IsNullOrWhiteSpace(email)) return email;

            throw new ArgumentException(
                string.Format(CultureInfo.CurrentCulture, ExceptionMessages.InvalidFormatOfEmail, email), email);
        }
    }
}
