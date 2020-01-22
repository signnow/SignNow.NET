using System;

namespace SignNow.Net.Test.Constants
{
    /// <summary>
    /// String constants and error messages for unit-tests.
    /// </summary>
    static class ErrorMessages
    {
        public const string InvalidFileType = "Invalid file type.";
        public const string CannotCreateSigningLinksOnDocumentsWithNoFields = "Cannot create signing links on documents with no fields";
        public const string TheDocumentIdShouldHave40Characters = "The document id should have exactly 40 characters.";
        public const string BadRequest = "Bad Request";

        /// <summary>
        /// Invalid format of Document Id {0}...
        /// </summary>
        public const string InvalidDocumentId =
            "Invalid format of Document Id <{0}>. The required format: 40 characters long, case-sensitive, letters and numbers, underscore allowed.";

        /// <summary>
        /// Value cannot be null. Parameter name: {0}
        /// </summary>
        public static string ValueCannotBeNull = "Value cannot be null." + Environment.NewLine + "Parameter name: {0}";
    }
}
