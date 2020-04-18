using System;
using System.Globalization;
using System.Text.RegularExpressions;
using SignNow.Net.Exceptions;

namespace SignNow.Net.Internal.Extensions
{
    static class ValidatorExtensions
    {
        /// <summary>
        /// Pattern for SignNow identity (Document, invite...)
        /// The required format: 40 characters long, case-sensitive, letters and numbers, underscore allowed.
        /// </summary>
        private const string IdPattern = @"^[a-zA-Z0-9_]{40,40}$";

        /// <summary>
        /// Pattern for Email address validation
        /// The required valid email address: e.g john+1@gmail.com or john123@gmail.com
        /// </summary>
        private const string EmailPattern = @"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$";

        /// <summary>
        /// Validates SignNow ID for documents, invites, etc...
        /// </summary>
        /// <param name="id">Identity of the document or invite.</param>
        /// <exception cref="ArgumentException">Invalid format of ID.</exception>
        public static string ValidateId(this string id)
        {
            var regex = new Regex(IdPattern);

            if (regex.IsMatch(id) && !string.IsNullOrWhiteSpace(id)) return id;

            throw new ArgumentException(
                string.Format(CultureInfo.CurrentCulture, ExceptionMessages.InvalidFormatOfId, id));
        }

        /// <summary>
        /// Validates email addresses.
        /// </summary>
        /// <param name="email">Email address.</param>
        /// <returns>Valid email address.</returns>
        /// <exception cref="ArgumentException">if email address is not valid.</exception>
        public static string ValidateEmail(this string email)
        {
            var regex = new Regex(EmailPattern, RegexOptions.IgnoreCase);

            if (regex.IsMatch(email) && !string.IsNullOrWhiteSpace(email)) return email;

            throw new ArgumentException(
                string.Format(CultureInfo.CurrentCulture, ExceptionMessages.InvalidFormatOfEmail, email));
        }
    }
}
