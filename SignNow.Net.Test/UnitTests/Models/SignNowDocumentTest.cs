using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using SignNow.Net.Internal.Model;
using SignNow.Net.Internal.Model.FieldTypes;
using SignNow.Net.Model;
using SignNow.Net.Test;
using SignNow.Net.Test.FakeModels;

namespace UnitTests
{
    [TestClass]
    public class SignNowDocumentTest : SignNowTestBase
    {
        [TestMethod]
        public void ShouldDeserializeFromJson()
        {
            var fakeDocument = new SignNowDocumentFaker()
                .RuleFor(obj => obj.Roles, new RoleFaker().Generate(3))
                .RuleFor(obj => obj.Signatures, new SignatureFaker().Generate(3))
                .RuleFor(obj => obj.InviteRequests, new FreeformInviteFaker().Generate(2));

            var fakeDocumentJson = JsonConvert.SerializeObject(fakeDocument.Generate(), Formatting.Indented);

            var testDocument = JsonConvert.DeserializeObject<SignNowDocument>(fakeDocumentJson);
            var testDocumentJson = JsonConvert.SerializeObject(testDocument, Formatting.Indented);

            Assert.AreEqual(fakeDocumentJson, testDocumentJson);
        }

        [DataTestMethod]
        [DataRow(FieldType.Text, DisplayName = "Get values for TextFields")]
        [DataRow(FieldType.Signature, DisplayName = "Get values for Signature Fields")]
        public void ShouldGetFieldValuesForDocument(object testType)
        {
            var testObjQty = 3;

            var docWithFields = new SignNowDocumentFaker()
                .RuleFor(obj => obj.Roles, new RoleFaker().Generate(testObjQty))
                .RuleFor(
                    obj => obj.Fields,
                    new FieldFaker().RuleFor(obj => obj.ElementId, f => f.Random.Hash(40)).Generate(testObjQty))
                .RuleFor(obj => obj.Texts, new TextFieldFaker().Generate(testObjQty))
                .RuleFor(obj => obj.Signatures, new SignatureFaker().Generate(testObjQty))
                .FinishWith((f, obj) => {
                    var role = obj.Roles.GetEnumerator();
                    var field = obj.Fields.GetEnumerator();
                    var text = obj.Texts.GetEnumerator();
                    var sign = obj.Signatures.GetEnumerator();

                    while (role.MoveNext() && field.MoveNext() && text.MoveNext() && sign.MoveNext())
                    {
                        field.Current.RoleId = role.Current.Id;
                        field.Current.Owner = obj.Owner;
                        field.Current.Type = (FieldType)testType;

                        text.Current.Id = field.Current.ElementId;
                        text.Current.Email = field.Current.Signer;
                        text.Current.Data = "this is test text field value";

                        sign.Current.Id = field.Current.ElementId;
                        sign.Current.Email = field.Current.Signer;
                        sign.Current.UserId = text.Current.UserId;
                        sign.Current.Data = Encoding.UTF8.GetBytes("this is test signature field value");
                    }
                })
                .Generate();

            foreach (var field in docWithFields.Fields)
            {
                var fieldValue = docWithFields.GetFieldValue(field);

                switch (field.Type)
                {
                    case FieldType.Text:
                        var textValue = fieldValue as TextField;
                        Assert.AreEqual(docWithFields.Owner, field.Owner);
                        Assert.AreEqual("this is test text field value", textValue?.Data);
                        Assert.AreEqual("this is test text field value", textValue?.ToString());
                        break;

                    case FieldType.Signature:
                        var signValue = fieldValue as Signature;
                        Assert.AreEqual(field.ElementId, signValue.Id);
                        Assert.AreEqual("this is test signature field value", Encoding.UTF8.GetString(signValue.Data));
                        Assert.AreEqual("dGhpcyBpcyB0ZXN0IHNpZ25hdHVyZSBmaWVsZCB2YWx1ZQ==", signValue?.ToString());
                        break;

                    default:
                        break;
                }
            }
        }
    }
}
