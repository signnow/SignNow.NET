using System;
using System.IO;

namespace SignNow.Net.Test
{
    public abstract class SignNowTestBase
    {
        public virtual Uri ApiBaseUrl => new Uri("https://api-eval.signnow.com/");
        public virtual Uri ApplicationBaseUrl => new Uri("https://app-eval.signnow.com/");

        private readonly string baseTestDataPath = "../../../TestData/"
            .Replace('/', Path.DirectorySeparatorChar);

#pragma warning disable CA1051 // Do not declare visible instance fields [SignNow.Net.Test]csharp(CA1051)
        protected readonly string pdfFileName = "DocumentUpload.pdf";
        protected readonly string txtFileName = "DocumentUpload.txt";
#pragma warning restore CA1051 // Do not declare visible instance fields

        protected string PdfFilePath => Path.Combine($"{baseTestDataPath}Documents", pdfFileName);
        protected string TxtFilePath => Path.Combine($"{baseTestDataPath}Documents", txtFileName);
    }
}
