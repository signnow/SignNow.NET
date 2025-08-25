using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using SignNow.Net.Model.Responses;
using SignNow.Net.Test.FakeModels;

namespace UnitTests.Models
{
    [TestClass]
    public class DocumentFieldsResponseTest
    {
        [TestMethod]
        public void ShouldDeserializeFromJson()
        {
            var responseFake = new DocumentFieldsResponseFaker().Generate();
            var expected = JsonConvert.SerializeObject(responseFake, Formatting.Indented);
            var responseActual = JsonConvert.DeserializeObject<DocumentFieldsResponse>(expected);

            Assert.That.JsonEqual(expected, responseActual);
        }

        [TestMethod]
        public void ShouldDeserializeEmptyDataFromJson()
        {
            var responseFake = new DocumentFieldsResponseFaker()
                .RuleFor(o => o.Data, new DocumentFieldDataFaker().Generate(0))
                .Generate();
            
            var expected = JsonConvert.SerializeObject(responseFake, Formatting.Indented);
            var responseActual = JsonConvert.DeserializeObject<DocumentFieldsResponse>(expected);

            Assert.That.JsonEqual(expected, responseActual);
            Assert.AreEqual(0, responseActual.Data.Count);
        }

        [TestMethod]
        public void ShouldDeserializeNullValuesFromJson()
        {
            var responseFake = new DocumentFieldsResponseFaker()
                .RuleFor(o => o.Data, new DocumentFieldDataFaker()
                    .RuleFor(d => d.Value, (string)null)
                    .Generate(2))
                .Generate();
            
            var expected = JsonConvert.SerializeObject(responseFake, Formatting.Indented);
            var responseActual = JsonConvert.DeserializeObject<DocumentFieldsResponse>(expected);

            Assert.That.JsonEqual(expected, responseActual);
            Assert.IsTrue(responseActual.Data.All(d => d.Value == null));
        }
    }
}
