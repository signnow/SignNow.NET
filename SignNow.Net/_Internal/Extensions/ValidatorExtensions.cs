using System;
using System.Text.RegularExpressions;

namespace SignNow.Net.Internal.Extensions
{
    static class ValidatorExtensions
    {
        /// <summary>
        /// Pattern for Document Identity
        /// The required format: 40 characters long, case-sensitive, letters and numbers, underscore allowed.
        /// </summary>
        private const string DocumentIdPattern = @"^[a-zA-Z0-9_]{40,40}$";

        /// <summary>
        /// Pattern for Email address validation
        /// The required valid email address: e.g john+1@gmail.com or john123@gmail.com
        /// </summary>
        private const string EmailPattern = @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\\.[a-zA-Z0-9-.]+$";

        /// <summary>
        /// Validates Document ID.
        /// </summary>
        /// <param name="documentId">Identity of the document.</param>
        /// <exception cref="ArgumentException">Invalid format of ID.</exception>
        public static string ValidateDocumentId(this string documentId)
        {
            var regex = new Regex(DocumentIdPattern);

            if (regex.IsMatch(documentId)) return documentId;

            throw new ArgumentException(
                "Invalid format of Document Id <" + documentId + ">. " +
                "The required format: 40 characters long, case-sensitive, letters and numbers, underscore allowed."
                );


        }

        /// <summary>
        /// Validates email addresses.
        /// </summary>
        /// <param name="email">Email address.</param>
        /// <returns>Valid email address.</returns>
        /// <exception cref="ArgumentException">if email address is not valid.</exception>
        public static string ValidateEmail(this string email)
        {
            var regex = new Regex(EmailPattern);

            if (regex.IsMatch(email)) return email;

            throw new ArgumentException(
                "Invalid format of email <" + email + ">. " +
                "The required format: valid email address (e.g john+1@gmail.com or john123@gmail.com)."
            );
        }
    }
}
