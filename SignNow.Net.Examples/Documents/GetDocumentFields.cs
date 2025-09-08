using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignNow.Net.Interfaces;
using SignNow.Net.Model.EditFields;

namespace SignNow.Net.Examples
{
    public partial class DocumentExamples
    {
        [TestMethod]
        public async Task GetDocumentFieldsAsync()
        {
            // Upload a test document
            await using var fileStream = File.OpenRead(PdfWithSignatureField);
            var testDocument = await testContext.Documents
                .UploadDocumentWithFieldExtractAsync(fileStream, "GetDocumentFieldsExample.pdf")
                .ConfigureAwait(false);

            // Add some fields to the document to demonstrate field retrieval
            var editFields = new List<IFieldEditable>
            {
                new TextField
                {
                    PageNumber = 0,
                    Name = "CustomerName",
                    Height = 40,
                    Width = 200,
                    X = 10,
                    Y = 40,
                    Role = "Signer 1"
                },
                new TextField
                {
                    PageNumber = 0,
                    Name = "EmailAddress",
                    Height = 40,
                    Width = 200,
                    X = 10,
                    Y = 90,
                    Role = "Signer 1"
                },
                new SignatureField
                {
                    PageNumber = 0,
                    Name = "CustomerSignature",
                    Height = 50,
                    Width = 200,
                    X = 10,
                    Y = 140,
                    Role = "Signer 1"
                }
            };

            var editedDocument = await testContext.Documents
                .EditDocumentAsync(testDocument.Id, editFields)
                .ConfigureAwait(false);

            Console.WriteLine("Retrieving field data from document...");

            var response = await testContext.Documents
                .GetDocumentFieldsAsync(editedDocument.Id)
                .ConfigureAwait(false);

            Console.WriteLine($"Successfully retrieved field data for document {editedDocument.Id}");
            
            // Verify the response structure first
            Assert.IsNotNull(response);
            Assert.IsNotNull(response.Data);
            Assert.IsNotNull(response.Meta);
            Assert.IsNotNull(response.Meta.Pagination);
            Assert.IsTrue(response.Data.Count > 0, "Document should have fields");

            Console.WriteLine($"Total fields: {response.Meta.Pagination.Total}");
            Console.WriteLine($"Fields in current page: {response.Data.Count}");

            // Display field information
            foreach (var field in response.Data)
            {
                Console.WriteLine($"Field: {field.Name} (Type: {field.Type})");
                Console.WriteLine($"  ID: {field.Id}");
                Console.WriteLine($"  Value: {(field.Value ?? "Not filled")}");
                Console.WriteLine();
            }

            // Display pagination information
            Console.WriteLine("Pagination Information:");
            Console.WriteLine($"  Current Page: {response.Meta.Pagination.CurrentPage}");
            Console.WriteLine($"  Total Pages: {response.Meta.Pagination.TotalPages}");
            Console.WriteLine($"  Per Page: {response.Meta.Pagination.PerPage}");

            // Clean up test documents
            DeleteTestDocument(testDocument.Id);
            DeleteTestDocument(editedDocument.Id);
        }
    }
}
