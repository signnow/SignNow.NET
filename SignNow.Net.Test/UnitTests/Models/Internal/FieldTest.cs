using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using SignNow.Net.Internal.Model;
using SignNow.Net.Model;
using SignNow.Net.Test.FakeModels;

namespace UnitTests
{
    [TestClass]
    public class FieldTest
    {
        [TestMethod]
        public void ShouldDeserializeFromJson()
        {
            var fieldFake = new FieldFaker().Generate();
            var fieldFakerJson =  JsonConvert.SerializeObject(fieldFake, Formatting.Indented);

            var fieldActual = JsonConvert.DeserializeObject<Field>(fieldFakerJson);
            var fieldActualJson = JsonConvert.SerializeObject(fieldActual, Formatting.Indented);

            Assert.AreEqual(fieldFakerJson, fieldActualJson);
        }
    }
}
