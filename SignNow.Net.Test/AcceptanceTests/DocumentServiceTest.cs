using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignNow.Net.Interfaces;
using SignNow.Net.Service;
using SignNow.Net.Test;
using System.IO;

namespace AcceptanceTests
{
    [TestClass]
    public partial class DocumentServiceTest : AuthorizedApiTestBase
    {
        readonly IDocumentService docService;

        /// <summary>
        /// Platfom specific Directory Separator Char
        /// </summary>
        readonly static char DS = Path.DirectorySeparatorChar;

        readonly string testDocumentsPath = $"..{DS}..{DS}..{DS}TestData{DS}Documents";
        readonly string pdfFileName = "DocumentUpload.pdf";
        readonly string txtFileName = "DocumentUpload.txt";

        private string PdfFilePath => Path.Combine(testDocumentsPath, pdfFileName);
        private string TxtFilePath => Path.Combine(testDocumentsPath, txtFileName);

        public DocumentServiceTest()
        {
            docService = new DocumentService(ApiBaseUrl, Token);
        }
    }
}
