using System;

namespace SignNow.Net.Test.Constants
{
    /// <summary>
    /// String constants and error messages for unit-tests.
    /// </summary>
    public static class ErrorMessages
    {
        public const string InvalidFileType = "Invalid file type.";
        public const string CannotCreateSigningLinksOnDocumentsWithNoFields = "Cannot create signing links on documents with no fields";
        public const string TheDocumentIdShouldHave40Characters = "The document id should have exactly 40 characters.";
        public const string BadRequest = "Bad Request";

        /// <summary>
        /// Invalid format of ID {param}...
        /// </summary>
        public const string InvalidFormatOfId =
            "Invalid format of ID <{0}>. The required format: 40 characters long, case-sensitive, letters and numbers, underscore allowed.";

        /// <summary>
        /// Invalid format of email {param}...
        /// </summary>
        public const string InvalidFormatOfEmail =
            "Invalid format of email <{0}>. The required format: valid email address (e.g john+1@gmail.com or john123@gmail.com).";

        /// <summary>
        /// Starts string message for Exceptions with nullable param: `Value cannot be null.`
        /// </summary>
        public static readonly string ValueCannotBeNull = "Value cannot be null.";
    }
}
