using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SignNow.Net.Examples
{
    public partial class DocumentExamples
    {
        [TestMethod]
        public async Task GetDocumentFieldsAsync()
        {
            // Use a document ID that has fields for this example
            // You can replace this with an actual document ID from your account
            var documentId = "your_document_id_with_fields";

            try
            {
                Console.WriteLine("Retrieving field data from document...");

                var response = await testContext.Documents
                    .GetDocumentFieldsAsync(documentId)
                    .ConfigureAwait(false);

                Console.WriteLine($"Successfully retrieved field data for document {documentId}");
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

                // Verify the response structure
                Assert.IsNotNull(response);
                Assert.IsNotNull(response.Data);
                Assert.IsNotNull(response.Meta);
                Assert.IsNotNull(response.Meta.Pagination);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving document fields: {ex.Message}");
                throw;
            }
        }
    }
}
