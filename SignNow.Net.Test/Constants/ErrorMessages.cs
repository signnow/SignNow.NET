using System;

namespace SignNow.Net.Test.Constants
{
    /// <summary>
    /// String constants and error messages for unit-tests.
    /// </summary>
    public static class ErrorMessages
    {
        public static readonly string InvalidFileType = "Invalid file type.";
        public static readonly string CannotCreateSigningLinksOnDocumentsWithNoFields = "Cannot create signing links on documents with no fields";
        public static readonly string TheDocumentIdShouldHave40Characters = "The document id should have exactly 40 characters.";
        public static readonly string BadRequest = "Bad Request";

        /// <summary>
        /// Invalid format of ID {param}...
        /// </summary>
        public static readonly string InvalidFormatOfId =
            "Invalid format of ID <{0}>. The required format: 40 characters long, case-sensitive, letters and numbers, underscore allowed.";

        /// <summary>
        /// Invalid format of email {param}...
        /// </summary>
        public static readonly string InvalidFormatOfEmail =
            "Invalid format of email <{0}>. The required format: valid email address (e.g john+1@gmail.com or john123@gmail.com).";

        /// <summary>
        /// Starts string message for Exceptions with nullable param: `Value cannot be null.`
        /// </summary>
        public static readonly string ValueCannotBeNull = "Value cannot be null.";
    }
}
