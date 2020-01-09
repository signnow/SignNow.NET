using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SignNow.Net.Test
{
    public abstract class ApiTestBase
    {
        public virtual Uri ApiBaseUrl => new Uri("https://api-eval.signnow.com/");
        public virtual Uri ApplicationBaseUrl => new Uri("https://app-eval.signnow.com/");

        /// <summary>
        /// Platform specific Directory Separator Char
        /// </summary>
        static readonly char DS = Path.DirectorySeparatorChar;

        private readonly string testDocumentsPath = $"..{DS}..{DS}..{DS}TestData{DS}Documents";
        protected readonly string pdfFileName = "DocumentUpload.pdf";
        protected readonly string txtFileName = "DocumentUpload.txt";

        protected string PdfFilePath => Path.Combine(testDocumentsPath, pdfFileName);
        protected string TxtFilePath => Path.Combine(testDocumentsPath, txtFileName);
    }
}
