using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SignNow.Net.Internal.Helpers.Converters;
using SignNow.Net.Model;

namespace UnitTests.Helpers.Converters
{
    [TestClass]
    public class PageLinksOrEmptyArrayConverterTest
    {
        private PageLinksOrEmptyArrayConverter _converter;

        [TestInitialize]
        public void Setup()
        {
            _converter = new PageLinksOrEmptyArrayConverter();
        }

        #region CanConvert Tests

        [TestMethod]
        public void CanConvert_ShouldReturnTrue_ForPageLinksType()
        {
            // Act & Assert
            Assert.IsTrue(_converter.CanConvert(typeof(PageLinks)));
        }

        [TestMethod]
        public void CanConvert_ShouldReturnFalse_ForOtherTypes()
        {
            // Act & Assert
            Assert.IsFalse(_converter.CanConvert(typeof(string)));
            Assert.IsFalse(_converter.CanConvert(typeof(int)));
            Assert.IsFalse(_converter.CanConvert(typeof(Uri)));
            Assert.IsFalse(_converter.CanConvert(typeof(object)));
        }

        #endregion

        #region ReadJson Tests - Object Scenarios

        [TestMethod]
        public void ReadJson_ShouldDeserializeObject_WhenTokenIsStartObject()
        {
            // Arrange
            var json = @"{
                ""previous"": ""https://api.signnow.com/api/v2/events?page=1"",
                ""next"": ""https://api.signnow.com/api/v2/events?page=3""
            }";
            var reader = new JsonTextReader(new System.IO.StringReader(json));
            reader.Read(); // Move to StartObject

            // Act
            var result = _converter.ReadJson(reader, typeof(PageLinks), null, JsonSerializer.CreateDefault());

            // Assert
            Assert.IsInstanceOfType(result, typeof(PageLinks));
            var pageLinks = (PageLinks)result;
            Assert.AreEqual("https://api.signnow.com/api/v2/events?page=1", pageLinks.Previous?.OriginalString);
            Assert.AreEqual("https://api.signnow.com/api/v2/events?page=3", pageLinks.Next?.OriginalString);
        }

        [TestMethod]
        public void ReadJson_ShouldDeserializeObjectWithOnlyPrevious_WhenTokenIsStartObject()
        {
            // Arrange
            var json = @"{
                ""previous"": ""https://api.signnow.com/api/v2/events?page=1""
            }";
            var reader = new JsonTextReader(new System.IO.StringReader(json));
            reader.Read(); // Move to StartObject

            // Act
            var result = _converter.ReadJson(reader, typeof(PageLinks), null, JsonSerializer.CreateDefault());

            // Assert
            Assert.IsInstanceOfType(result, typeof(PageLinks));
            var pageLinks = (PageLinks)result;
            Assert.AreEqual("https://api.signnow.com/api/v2/events?page=1", pageLinks.Previous?.OriginalString);
            Assert.IsNull(pageLinks.Next);
        }

        [TestMethod]
        public void ReadJson_ShouldDeserializeObjectWithOnlyNext_WhenTokenIsStartObject()
        {
            // Arrange
            var json = @"{
                ""next"": ""https://api.signnow.com/api/v2/events?page=3""
            }";
            var reader = new JsonTextReader(new System.IO.StringReader(json));
            reader.Read(); // Move to StartObject

            // Act
            var result = _converter.ReadJson(reader, typeof(PageLinks), null, JsonSerializer.CreateDefault());

            // Assert
            Assert.IsInstanceOfType(result, typeof(PageLinks));
            var pageLinks = (PageLinks)result;
            Assert.IsNull(pageLinks.Previous);
            Assert.AreEqual("https://api.signnow.com/api/v2/events?page=3", pageLinks.Next?.OriginalString);
        }

        [TestMethod]
        public void ReadJson_ShouldDeserializeEmptyObject_WhenTokenIsStartObject()
        {
            // Arrange
            var json = @"{}";
            var reader = new JsonTextReader(new System.IO.StringReader(json));
            reader.Read(); // Move to StartObject

            // Act
            var result = _converter.ReadJson(reader, typeof(PageLinks), null, JsonSerializer.CreateDefault());

            // Assert
            Assert.IsInstanceOfType(result, typeof(PageLinks));
            var pageLinks = (PageLinks)result;
            Assert.IsNull(pageLinks.Previous);
            Assert.IsNull(pageLinks.Next);
        }

        #endregion

        #region ReadJson Tests - Array Scenarios

        [TestMethod]
        public void ReadJson_ShouldReturnEmptyPageLinks_WhenTokenIsEmptyArray()
        {
            // Arrange
            var json = "[]";
            var reader = new JsonTextReader(new System.IO.StringReader(json));
            reader.Read(); // Move to StartArray

            // Act
            var result = _converter.ReadJson(reader, typeof(PageLinks), null, JsonSerializer.CreateDefault());

            // Assert
            Assert.IsInstanceOfType(result, typeof(PageLinks));
            var pageLinks = (PageLinks)result;
            Assert.IsNull(pageLinks.Previous);
            Assert.IsNull(pageLinks.Next);
        }

        [TestMethod]
        public void ReadJson_ShouldThrowException_WhenTokenIsNonEmptyArray()
        {
            // Arrange
            var json = @"[""some_value""]";
            var reader = new JsonTextReader(new System.IO.StringReader(json));
            reader.Read(); // Move to StartArray

            // Act & Assert
            var exception = Assert.ThrowsException<JsonSerializationException>(
                () => _converter.ReadJson(reader, typeof(PageLinks), null, JsonSerializer.CreateDefault()));

            // After JArray.Load(reader), the reader is at EndArray token
            Assert.AreEqual("Unexpected token type: EndArray", exception.Message);
        }

        #endregion

        #region ReadJson Tests - Invalid Token Scenarios

        [TestMethod]
        public void ReadJson_ShouldThrowException_WhenTokenIsString()
        {
            // Arrange
            var json = @"""invalid""";
            var reader = new JsonTextReader(new System.IO.StringReader(json));
            reader.Read(); // Move to String

            // Act & Assert
            var exception = Assert.ThrowsException<JsonSerializationException>(
                () => _converter.ReadJson(reader, typeof(PageLinks), null, JsonSerializer.CreateDefault()));

            Assert.AreEqual("Unexpected token type: String", exception.Message);
        }

        [TestMethod]
        public void ReadJson_ShouldThrowException_WhenTokenIsNumber()
        {
            // Arrange
            var json = "42";
            var reader = new JsonTextReader(new System.IO.StringReader(json));
            reader.Read(); // Move to Integer

            // Act & Assert
            var exception = Assert.ThrowsException<JsonSerializationException>(
                () => _converter.ReadJson(reader, typeof(PageLinks), null, JsonSerializer.CreateDefault()));

            Assert.AreEqual("Unexpected token type: Integer", exception.Message);
        }

        [TestMethod]
        public void ReadJson_ShouldThrowException_WhenTokenIsBoolean()
        {
            // Arrange
            var json = "true";
            var reader = new JsonTextReader(new System.IO.StringReader(json));
            reader.Read(); // Move to Boolean

            // Act & Assert
            var exception = Assert.ThrowsException<JsonSerializationException>(
                () => _converter.ReadJson(reader, typeof(PageLinks), null, JsonSerializer.CreateDefault()));

            Assert.AreEqual("Unexpected token type: Boolean", exception.Message);
        }

        [TestMethod]
        public void ReadJson_ShouldThrowException_WhenTokenIsNull()
        {
            // Arrange
            var json = "null";
            var reader = new JsonTextReader(new System.IO.StringReader(json));
            reader.Read(); // Move to Null

            // Act & Assert
            var exception = Assert.ThrowsException<JsonSerializationException>(
                () => _converter.ReadJson(reader, typeof(PageLinks), null, JsonSerializer.CreateDefault()));

            Assert.AreEqual("Unexpected token type: Null", exception.Message);
        }

        #endregion

        #region WriteJson Tests

        [TestMethod]
        public void WriteJson_ShouldWriteEmptyArray_WhenBothLinksAreNull()
        {
            // Arrange
            var pageLinks = new PageLinks
            {
                Previous = null,
                Next = null
            };
            var stringWriter = new System.IO.StringWriter();
            var jsonWriter = new JsonTextWriter(stringWriter);

            // Act
            _converter.WriteJson(jsonWriter, pageLinks, JsonSerializer.CreateDefault());

            // Assert
            Assert.AreEqual("[]", stringWriter.ToString());
        }

        [TestMethod]
        public void WriteJson_ShouldSerializeNormally_WhenPreviousIsNotNull()
        {
            // Arrange
            var pageLinks = new PageLinks
            {
                Previous = new Uri("https://api.signnow.com/api/v2/events?page=1"),
                Next = null
            };
            var stringWriter = new System.IO.StringWriter();
            var jsonWriter = new JsonTextWriter(stringWriter);

            // Act
            _converter.WriteJson(jsonWriter, pageLinks, JsonSerializer.CreateDefault());

            // Assert
            var result = stringWriter.ToString();
            StringAssert.Contains(result, "previous");
            StringAssert.Contains(result, "https://api.signnow.com/api/v2/events?page=1");
        }

        [TestMethod]
        public void WriteJson_ShouldSerializeNormally_WhenNextIsNotNull()
        {
            // Arrange
            var pageLinks = new PageLinks
            {
                Previous = null,
                Next = new Uri("https://api.signnow.com/api/v2/events?page=3")
            };
            var stringWriter = new System.IO.StringWriter();
            var jsonWriter = new JsonTextWriter(stringWriter);

            // Act
            _converter.WriteJson(jsonWriter, pageLinks, JsonSerializer.CreateDefault());

            // Assert
            var result = stringWriter.ToString();
            StringAssert.Contains(result, "next");
            StringAssert.Contains(result, "https://api.signnow.com/api/v2/events?page=3");
        }

        [TestMethod]
        public void WriteJson_ShouldSerializeNormally_WhenBothLinksAreNotNull()
        {
            // Arrange
            var pageLinks = new PageLinks
            {
                Previous = new Uri("https://api.signnow.com/api/v2/events?page=1"),
                Next = new Uri("https://api.signnow.com/api/v2/events?page=3")
            };
            var stringWriter = new System.IO.StringWriter();
            var jsonWriter = new JsonTextWriter(stringWriter);

            // Act
            _converter.WriteJson(jsonWriter, pageLinks, JsonSerializer.CreateDefault());

            // Assert
            var result = stringWriter.ToString();
            StringAssert.Contains(result, "previous");
            StringAssert.Contains(result, "next");
            StringAssert.Contains(result, "https://api.signnow.com/api/v2/events?page=1");
            StringAssert.Contains(result, "https://api.signnow.com/api/v2/events?page=3");
        }

        [TestMethod]
        public void WriteJson_ShouldSerializeNormally_WhenValueIsNotPageLinks()
        {
            // Arrange
            var stringWriter = new System.IO.StringWriter();
            var jsonWriter = new JsonTextWriter(stringWriter);
            var nonPageLinksObject = new { someProperty = "value" };

            // Act
            _converter.WriteJson(jsonWriter, nonPageLinksObject, JsonSerializer.CreateDefault());

            // Assert
            var result = stringWriter.ToString();
            StringAssert.Contains(result, "someProperty");
            StringAssert.Contains(result, "value");
        }

        #endregion

        #region Real SignNow API Behavior Tests

        [TestMethod]
        public void ReadJson_ShouldHandleRealSignNowPaginationResponse_WithLinks()
        {
            // Arrange - Real signNow API response pattern
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
            var pagination = JsonConvert.DeserializeObject<Pagination>(realApiResponse);

            // Assert
            Assert.IsNotNull(pagination.Links);
            Assert.AreEqual("https://api.signnow.com/api/v2/events?page=1", pagination.Links.Previous?.OriginalString);
            Assert.AreEqual("https://api.signnow.com/api/v2/events?page=3", pagination.Links.Next?.OriginalString);
        }

        [TestMethod]
        public void ReadJson_ShouldHandleRealSignNowPaginationResponse_WithEmptyArray()
        {
            // Arrange - Real signNow API response pattern when no pagination links
            var realApiResponse = @"{
                ""total"": 5,
                ""count"": 5,
                ""per_page"": 20,
                ""current_page"": 1,
                ""total_pages"": 1,
                ""links"": []
            }";

            // Act
            var pagination = JsonConvert.DeserializeObject<Pagination>(realApiResponse);

            // Assert
            Assert.IsNotNull(pagination.Links);
            Assert.IsNull(pagination.Links.Previous);
            Assert.IsNull(pagination.Links.Next);
        }

        [TestMethod]
        public void ReadJson_ShouldHandleRealSignNowPaginationResponse_WithOnlyPrevious()
        {
            // Arrange - Real signNow API response pattern for last page
            var realApiResponse = @"{
                ""total"": 100,
                ""count"": 20,
                ""per_page"": 20,
                ""current_page"": 5,
                ""total_pages"": 5,
                ""links"": {
                    ""previous"": ""https://api.signnow.com/api/v2/events?page=4""
                }
            }";

            // Act
            var pagination = JsonConvert.DeserializeObject<Pagination>(realApiResponse);

            // Assert
            Assert.IsNotNull(pagination.Links);
            Assert.AreEqual("https://api.signnow.com/api/v2/events?page=4", pagination.Links.Previous?.OriginalString);
            Assert.IsNull(pagination.Links.Next);
        }

        [TestMethod]
        public void ReadJson_ShouldHandleRealSignNowPaginationResponse_WithOnlyNext()
        {
            // Arrange - Real signNow API response pattern for first page
            var realApiResponse = @"{
                ""total"": 100,
                ""count"": 20,
                ""per_page"": 20,
                ""current_page"": 1,
                ""total_pages"": 5,
                ""links"": {
                    ""next"": ""https://api.signnow.com/api/v2/events?page=2""
                }
            }";

            // Act
            var pagination = JsonConvert.DeserializeObject<Pagination>(realApiResponse);

            // Assert
            Assert.IsNotNull(pagination.Links);
            Assert.IsNull(pagination.Links.Previous);
            Assert.AreEqual("https://api.signnow.com/api/v2/events?page=2", pagination.Links.Next?.OriginalString);
        }

        [TestMethod]
        public void WriteJson_ShouldProduceCorrectJson_ForEmptyPageLinks()
        {
            // Arrange
            var pagination = new Pagination
            {
                Total = 5,
                Count = 5,
                PerPage = 20,
                CurrentPage = 1,
                TotalPages = 1,
                Links = new PageLinks() // Both Previous and Next are null
            };

            // Act
            var json = JsonConvert.SerializeObject(pagination);

            // Assert
            StringAssert.Contains(json, "\"links\":[]");
        }

        [TestMethod]
        public void WriteJson_ShouldProduceCorrectJson_ForPageLinksWithValues()
        {
            // Arrange
            var pagination = new Pagination
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

            // Act
            var json = JsonConvert.SerializeObject(pagination);

            // Assert
            StringAssert.Contains(json, "\"previous\":\"https://api.signnow.com/api/v2/events?page=1\"");
            StringAssert.Contains(json, "\"next\":\"https://api.signnow.com/api/v2/events?page=3\"");
        }

        #endregion

        #region Edge Cases and Error Scenarios

        [TestMethod]
        public void ReadJson_ShouldHandleMalformedJson_WithInvalidLinksStructure()
        {
            // Arrange - This should not happen in real API, but tests robustness
            var malformedJson = @"{
                ""total"": 100,
                ""count"": 20,
                ""per_page"": 20,
                ""current_page"": 2,
                ""total_pages"": 5,
                ""links"": ""invalid_string""
            }";

            // Act & Assert
            var exception = Assert.ThrowsException<JsonSerializationException>(
                () => JsonConvert.DeserializeObject<Pagination>(malformedJson));

            Assert.AreEqual("Unexpected token type: String", exception.Message);
        }

        [TestMethod]
        public void ReadJson_ShouldHandleNullLinks_WhenLinksPropertyIsNull()
        {
            // Arrange - API might return null for links
            var jsonWithNullLinks = @"{
                ""total"": 100,
                ""count"": 20,
                ""per_page"": 20,
                ""current_page"": 2,
                ""total_pages"": 5,
                ""links"": null
            }";

            // Act - When links is null, the converter is not called due to NullValueHandling.Ignore
            var pagination = JsonConvert.DeserializeObject<Pagination>(jsonWithNullLinks);

            // Assert - Links should be null (not set)
            Assert.IsNull(pagination.Links);
        }

        #endregion
    }
}
