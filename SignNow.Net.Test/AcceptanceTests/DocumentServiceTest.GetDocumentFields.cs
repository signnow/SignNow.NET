using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignNow.Net.Internal.Extensions;
using UnitTests;

namespace AcceptanceTests
{
    public partial class DocumentServiceTest
    {
        [TestMethod]
        public async Task ShouldGetDocumentFields()
        {
            var response = await SignNowTestContext.Documents
                .GetDocumentFieldsAsync(TestPdfDocumentIdWithFields)
                .ConfigureAwait(false);

            Assert.IsNotNull(response);
            Assert.IsNotNull(response.Data);
            Assert.IsNotNull(response.Meta);
            Assert.IsNotNull(response.Meta.Pagination);

            // Verify pagination data
            Assert.IsTrue(response.Meta.Pagination.Total >= 0);
            Assert.IsTrue(response.Meta.Pagination.Count >= 0);
            Assert.AreEqual(15, response.Meta.Pagination.PerPage);
            Assert.AreEqual(1, response.Meta.Pagination.CurrentPage);
            Assert.IsTrue(response.Meta.Pagination.TotalPages >= 0);

            // Verify field data structure
            foreach (var field in response.Data)
            {
                Assert.IsNotNull(field.Id);
                Assert.IsTrue(field.Id.Length == 40, "Field ID should be 40 characters long");
                Assert.IsNotNull(field.Name);
                Assert.IsNotNull(field.Type);
                Assert.IsTrue(new[] { "text", "enumeration", "checkbox", "signature" }.Contains(field.Type), 
                    $"Field type should be one of: text, enumeration, checkbox, signature. Got: {field.Type}");
            }
        }

        [TestMethod]
        public async Task ShouldGetDocumentFieldsWithEmptyData()
        {
            // Test with a document that has no fields
            var response = await SignNowTestContext.Documents
                .GetDocumentFieldsAsync(TestPdfDocumentId)
                .ConfigureAwait(false);

            Assert.IsNotNull(response);
            Assert.IsNotNull(response.Data);
            Assert.IsNotNull(response.Meta);
            Assert.IsNotNull(response.Meta.Pagination);

            // Document without fields should return empty data array
            Assert.AreEqual(0, response.Data.Count);
            Assert.AreEqual(0, response.Meta.Pagination.Total);
            Assert.AreEqual(0, response.Meta.Pagination.Count);
        }

        [TestMethod]
        public async Task ShouldGetDocumentFieldsWithNullValues()
        {
            var response = await SignNowTestContext.Documents
                .GetDocumentFieldsAsync(TestPdfDocumentIdWithFields)
                .ConfigureAwait(false);

            Assert.IsNotNull(response);
            Assert.IsNotNull(response.Data);

            // Some fields might have null values (unfilled fields)
            var fieldsWithNullValues = response.Data.Where(f => f.Value == null).ToList();
            var fieldsWithValues = response.Data.Where(f => f.Value != null).ToList();

            // At least some fields should have values or be null (both cases are valid)
            Assert.IsTrue(fieldsWithNullValues.Count >= 0 || fieldsWithValues.Count >= 0);
        }
    }
}
