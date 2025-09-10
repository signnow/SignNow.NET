using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using SignNow.Net.Internal.Helpers.Converters;
using SignNow.Net.Model;
using UnitTests;

namespace UnitTests.Helpers.Converters
{
    [TestClass]
    public class PageLinksOrEmptyArrayConverterTest
    {
        #region Real SignNow API Response Tests

        [TestMethod]
        public void ReadJson_ShouldHandleRealSignNowPaginationResponse_WithLinks()
        {
            // Arrange - Real signNow API response with both previous and next links
            var realApiResponse = @"{
                ""total"": 100,
                ""count"": 20,
                ""per_page"": 20,
                ""current_page"": 2,
                ""total_pages"": 5,
                ""links"": {
                    ""previous"": ""https://api.signnow.com/api/v2/events?page=1"",
                    ""next"": ""https://api.signnow.com/api/v2/events?page=3""
                }
            }";

            // Act
            var pagination = TestUtils.DeserializeFromJson<Pagination>(realApiResponse);

            // Assert
            Assert.IsNotNull(pagination.Links);
            Assert.AreEqual("https://api.signnow.com/api/v2/events?page=1", pagination.Links.Previous?.OriginalString);
            Assert.AreEqual("https://api.signnow.com/api/v2/events?page=3", pagination.Links.Next?.OriginalString);
        }

        [TestMethod]
        public void ReadJson_ShouldHandleRealSignNowPaginationResponse_WithEmptyArray()
        {
            // Arrange - Real signNow API response when no pagination links
            var realApiResponse = @"{
                ""total"": 5,
                ""count"": 5,
                ""per_page"": 20,
                ""current_page"": 1,
                ""total_pages"": 1,
                ""links"": []
            }";

            // Act
            var pagination = TestUtils.DeserializeFromJson<Pagination>(realApiResponse);

            // Assert
            Assert.IsNotNull(pagination.Links);
            Assert.IsNull(pagination.Links.Previous);
            Assert.IsNull(pagination.Links.Next);
        }

        [TestMethod]
        public void ReadJson_ShouldThrowException_WhenLinksIsInvalidFormat()
        {
            // Arrange - Invalid links format (should not happen in real API)
            var invalidJson = @"{
                ""total"": 100,
                ""count"": 20,
                ""per_page"": 20,
                ""current_page"": 2,
                ""total_pages"": 5,
                ""links"": ""invalid_string""
            }";

            // Act & Assert
            var exception = Assert.ThrowsException<JsonSerializationException>(
                () => TestUtils.DeserializeFromJson<Pagination>(invalidJson));

            Assert.AreEqual("Unexpected token type: String", exception.Message);
        }

        #endregion

        #region Serialize/Deserialize Roundtrip Test

        [TestMethod]
        public void SerializeDeserialize_ShouldProduceSameResult_ForPageLinksWithValues()
        {
            // Arrange - Create a fake pagination object
            var originalPagination = new Pagination
            {
                Total = 100,
                Count = 20,
                PerPage = 20,
                CurrentPage = 2,
                TotalPages = 5,
                Links = new PageLinks
                {
                    Previous = new Uri("https://api.signnow.com/api/v2/events?page=1"),
                    Next = new Uri("https://api.signnow.com/api/v2/events?page=3")
                }
            };

            // Act - Serialize to JSON and deserialize back
            var json = JsonConvert.SerializeObject(originalPagination);
            var deserializedPagination = TestUtils.DeserializeFromJson<Pagination>(json);

            // Assert - The result should be the same as the original
            Assert.AreEqual(originalPagination.Total, deserializedPagination.Total);
            Assert.AreEqual(originalPagination.Count, deserializedPagination.Count);
            Assert.AreEqual(originalPagination.PerPage, deserializedPagination.PerPage);
            Assert.AreEqual(originalPagination.CurrentPage, deserializedPagination.CurrentPage);
            Assert.AreEqual(originalPagination.TotalPages, deserializedPagination.TotalPages);
            Assert.AreEqual(originalPagination.Links.Previous?.OriginalString, deserializedPagination.Links.Previous?.OriginalString);
            Assert.AreEqual(originalPagination.Links.Next?.OriginalString, deserializedPagination.Links.Next?.OriginalString);
        }

        [TestMethod]
        public void SerializeDeserialize_ShouldProduceSameResult_ForEmptyPageLinks()
        {
            // Arrange - Create a fake pagination object with empty links
            var originalPagination = new Pagination
            {
                Total = 5,
                Count = 5,
                PerPage = 20,
                CurrentPage = 1,
                TotalPages = 1,
                Links = new PageLinks() // Both Previous and Next are null
            };

            // Act - Serialize to JSON and deserialize back
            var json = JsonConvert.SerializeObject(originalPagination);
            var deserializedPagination = TestUtils.DeserializeFromJson<Pagination>(json);

            // Assert - The result should be the same as the original
            Assert.AreEqual(originalPagination.Total, deserializedPagination.Total);
            Assert.AreEqual(originalPagination.Count, deserializedPagination.Count);
            Assert.AreEqual(originalPagination.PerPage, deserializedPagination.PerPage);
            Assert.AreEqual(originalPagination.CurrentPage, deserializedPagination.CurrentPage);
            Assert.AreEqual(originalPagination.TotalPages, deserializedPagination.TotalPages);
            Assert.IsNull(deserializedPagination.Links.Previous);
            Assert.IsNull(deserializedPagination.Links.Next);
        }

        #endregion
    }
}
