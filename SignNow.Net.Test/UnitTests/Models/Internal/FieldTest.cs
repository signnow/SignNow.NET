using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
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

            Assert.That.JsonEqual(fieldFakerJson, fieldActual);
        }
    }
}
