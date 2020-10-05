using System;
using System.IO;

namespace SignNow.Net.Test
{
    public abstract class SignNowTestBase
    {
        public static Uri ApiBaseUrl => new Uri("https://api-eval.signnow.com/");
        public static Uri ApplicationBaseUrl => new Uri("https://app-eval.signnow.com/");

        private static readonly string BaseTestDataPath = "../../../TestData/"
            .Replace('/', Path.DirectorySeparatorChar);

#pragma warning disable CA1051 // Do not declare visible instance fields [SignNow.Net.Test]csharp(CA1051)
        protected static readonly string PdfFileName = "DocumentUpload.pdf";
        protected static readonly string TxtFileName = "DocumentUpload.txt";
#pragma warning restore CA1051 // Do not declare visible instance fields

        protected static string PdfFilePath => Path.Combine($"{BaseTestDataPath}Documents", PdfFileName);
        protected static string TxtFilePath => Path.Combine($"{BaseTestDataPath}Documents", TxtFileName);
    }
}
