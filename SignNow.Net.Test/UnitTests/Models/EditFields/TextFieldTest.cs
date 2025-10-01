using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignNow.Net.Model.EditFields;
using SignNow.Net.Test.FakeModels.EditFields;

namespace UnitTests.Models
{
    [TestClass]
    public class TextFieldTest
    {
        [TestMethod]
        public void ShouldDeserializeFromJson()
        {
            var fakeModel = new TextFieldFaker().Generate();
            var jsonFake = TestUtils.SerializeToJsonFormatted(fakeModel);

            var field = TestUtils.DeserializeFromJson<TextField>(jsonFake);

            Assert.AreEqual(fakeModel.PageNumber, field.PageNumber);
            Assert.AreEqual(fakeModel.Label, field.Label);
            Assert.AreEqual(fakeModel.Type, field.Type);
            Assert.AreEqual(fakeModel.Name, field.Name);
            Assert.AreEqual(fakeModel.Role, field.Role);
            Assert.AreEqual(fakeModel.Required, field.Required);
            Assert.AreEqual(fakeModel.X, field.X);
            Assert.AreEqual(fakeModel.Y, field.Y);
            Assert.AreEqual(fakeModel.Width, field.Width);
            Assert.AreEqual(fakeModel.Height, field.Height);
            Assert.AreEqual(fakeModel.PrefilledText, field.PrefilledText);
        }

        [TestMethod]
        public void ThrowsExceptionForWrongPageNumber()
        {
            var exception = Assert.ThrowsException<ArgumentException>(
                () => new TextField() { PageNumber = -1 });

            StringAssert.Contains(exception.Message, "Value cannot be less than 0");
            Assert.AreEqual(exception.ParamName, "PageNumber");
        }
    }
}
