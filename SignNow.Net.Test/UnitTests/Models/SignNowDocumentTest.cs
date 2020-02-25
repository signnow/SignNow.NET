using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using SignNow.Net.Model;
using SignNow.Net.Model.FieldContents;
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
            var qty = 3;

            var fakeDocument = new SignNowDocumentFaker()
                .RuleFor(obj => obj.Roles, new RoleFaker().Generate(qty))
                .RuleFor(obj => obj.Signatures, new SignatureContentFaker().Generate(qty))
                .RuleFor(obj => obj.InviteRequests, new FreeformInviteFaker().Generate(qty))
                .RuleFor(obj => obj.Attachments, new AttachmentContentFaker().Generate(qty))
                .RuleFor(obj => obj.Checkboxes, new CheckboxContentFaker().Generate(qty))
                .RuleFor(obj => obj.Enumerations, new EnumerationContentFaker().Generate(qty))
                .RuleFor(obj => obj.FieldInvites, new FieldInviteFaker().Generate(qty))
                .RuleFor(obj => obj.Fields, new FieldFaker().Generate(qty))
                .RuleFor(obj => obj.Hyperlinks, new HyperlinkContentFaker().Generate(qty))
                .RuleFor(obj => obj.Radiobuttons, new RadiobuttonContentFaker().Generate(qty))
                .RuleFor(obj => obj.Texts, new TextContentFaker().Generate(qty));

            var fakeDocumentJson = JsonConvert.SerializeObject(fakeDocument.Generate(), Formatting.Indented);

            var testDocument = JsonConvert.DeserializeObject<SignNowDocument>(fakeDocumentJson);
            var testDocumentJson = JsonConvert.SerializeObject(testDocument, Formatting.Indented);

            Assert.AreEqual(fakeDocumentJson, testDocumentJson);
        }

        [DataTestMethod]
        [DataRow(FieldType.Text,        DisplayName = "Get values for Text Fields")]
        [DataRow(FieldType.Signature,   DisplayName = "Get values for SignatureContent Fields")]
        [DataRow(FieldType.Initial,     DisplayName = "Get values for Initial Fields")]
        [DataRow(FieldType.Hyperlink,   DisplayName = "Get values for Hyperlink Fields")]
        [DataRow(FieldType.Checkbox,    DisplayName = "Get values for Checkbox Fields")]
        [DataRow(FieldType.Attachment,  DisplayName = "Get values for Attachment Fields")]
        [DataRow(FieldType.Dropdown,    DisplayName = "Get values for Dropdown Fields")]
        [DataRow(FieldType.RadioButton, DisplayName = "Get values for Radiobutton Fields")]
        public void ShouldGetFieldValuesForDocument(object testType)
        {
            var testObjQty = 3;

            var docWithFields = new SignNowDocumentFaker()
                .RuleFor(obj => obj.Roles, new RoleFaker().Generate(testObjQty))
                .RuleFor(obj => obj.Fields, new FieldFaker().Rules((f1, obj1) =>
                        {
                            obj1.Type = (FieldType)testType;
                            obj1.ElementId = f1.Random.Hash(40);
                            if ((FieldType)testType == FieldType.RadioButton)
                            {
                                obj1.RadioGroup = new RadioContentFaker()
                                    .Rules((f2, o2) => o2.PageNumber = obj1.JsonAttributes.PageNumber)
                                    .Generate(testObjQty);
                            }
                        })
                        .Generate(testObjQty))
                .RuleFor(obj => obj.Texts, new TextContentFaker().Generate(testObjQty))
                .RuleFor(obj => obj.Hyperlinks, new HyperlinkContentFaker().Generate(testObjQty))
                .RuleFor(obj => obj.Signatures, new SignatureContentFaker().Generate(testObjQty))
                .RuleFor(obj => obj.Checkboxes, new CheckboxContentFaker().Generate(testObjQty))
                .RuleFor(obj => obj.Attachments, new AttachmentContentFaker().Generate(testObjQty))
                .RuleFor(obj => obj.Enumerations, new EnumerationContentFaker().Generate(testObjQty))
                .RuleFor(obj => obj.Radiobuttons, new RadiobuttonContentFaker().Rules((f3, obj3) =>
                        {
                            obj3.Radio = new RadioContentFaker().Generate(testObjQty);
                        })
                        .Generate(testObjQty))
                .FinishWith((f, obj) => {
                    var role    = obj.Roles.GetEnumerator();
                    var field   = obj.Fields.GetEnumerator();
                    var text    = obj.Texts.GetEnumerator();
                    var link    = obj.Hyperlinks.GetEnumerator();
                    var sign    = obj.Signatures.GetEnumerator();
                    var attach  = obj.Attachments.GetEnumerator();
                    var checkbox = obj.Checkboxes.GetEnumerator();
                    var radio   = obj.Radiobuttons.GetEnumerator();

                    while (role.MoveNext() && field.MoveNext() && text.MoveNext()
                        && sign.MoveNext() && link.MoveNext() && checkbox.MoveNext()
                        && attach.MoveNext() && radio.MoveNext())
                    {
                        field.Current.RoleId = role.Current.Id;
                        field.Current.Owner = obj.Owner;
                        field.Current.JsonAttributes.Name = ((FieldType)testType).ToString() + "Name";
                        field.Current.JsonAttributes.Label = ((FieldType)testType).ToString() + "LabelName";

                        text.Current.Id = field.Current.ElementId;
                        text.Current.Email = field.Current.Signer;
                        text.Current.Data = (FieldType)testType == FieldType.Dropdown
                            ? "this is test dropdown element value"
                            : "this is test text field value";

                        link.Current.Id = field.Current.ElementId;
                        link.Current.Email = field.Current.Signer;
                        link.Current.Data = new System.Uri($"https://signnow.com/{link.Current.UserId}");
                        link.Current.Label = $"label for {link.Current.UserId}";

                        sign.Current.Id = field.Current.ElementId;
                        sign.Current.Email = field.Current.Signer;
                        sign.Current.UserId = text.Current.UserId;
                        sign.Current.Data = Encoding.UTF8.GetBytes("this is test signature field value");

                        checkbox.Current.Id = field.Current.ElementId;
                        if (field.Current.Type == FieldType.Checkbox)
                        {
                            field.Current.JsonAttributes.PrefilledText = f.PickRandom(new[] { "1", "" });
                        }

                        attach.Current.Id = field.Current.ElementId;
                        attach.Current.OriginalName = "TestFileName.pdf";

                        radio.Current.Id = field.Current.ElementId;
                        radio.Current.Radio.First(itm => itm.Checked == true).Data = "this is test radiobutton field value";
                    }
                })
                .Generate();

            foreach (var field in docWithFields.Fields)
            {
                var fieldValue = docWithFields.GetFieldContent(field);

                switch (field.Type)
                {
                    case FieldType.Text:
                        var textValue = fieldValue as TextContent;
                        Assert.AreEqual(docWithFields.Owner, field.Owner, "Wrong field Owner");
                        Assert.AreEqual("this is test text field value", textValue?.Data);
                        Assert.AreEqual("this is test text field value", textValue?.ToString());
                        break;

                    case FieldType.Signature:
                    case FieldType.Initial:
                        var signValue = fieldValue as SignatureContent;
                        Assert.AreEqual(field.ElementId, signValue.Id, "Wrong signature ID");
                        Assert.AreEqual("this is test signature field value", Encoding.UTF8.GetString(signValue.Data));
                        Assert.AreEqual("dGhpcyBpcyB0ZXN0IHNpZ25hdHVyZSBmaWVsZCB2YWx1ZQ==", signValue?.ToString());
                        break;

                    case FieldType.Hyperlink:
                        var linkValue = fieldValue as HyperlinkContent;
                        Assert.AreEqual(field.ElementId, linkValue.Id, "Wrong hyperlink ID");
                        Assert.AreEqual($"label for {linkValue.UserId}", linkValue.Label);
                        Assert.AreEqual($"https://signnow.com/{linkValue.UserId}", linkValue?.Data.OriginalString);
                        Assert.AreEqual($"https://signnow.com/{linkValue.UserId}", linkValue?.ToString());
                        break;

                    case FieldType.Checkbox:
                        var checkboxValue = fieldValue as CheckboxContent;
                        Assert.AreEqual(field.ElementId, checkboxValue.Id, "Wrong checkbox ID");
                        Assert.AreEqual(field.JsonAttributes.PrefilledText == "1", checkboxValue.Data);
                        Assert.AreEqual(field.JsonAttributes.PrefilledText, checkboxValue?.ToString());
                        break;

                    case FieldType.Attachment:
                        var attachmentValue = fieldValue as AttachmentContent;
                        Assert.AreEqual(field.ElementId, attachmentValue?.Id, "Wrong attachment ID");
                        Assert.AreEqual("TestFileName.pdf", attachmentValue.OriginalName);
                        break;

                    case FieldType.Dropdown:
                        var dropdownValue = fieldValue as TextContent;
                        Assert.AreEqual(field.ElementId, dropdownValue?.Id, "Wrong dropdown ID");
                        Assert.AreEqual("this is test dropdown element value", dropdownValue?.Data);
                        Assert.AreEqual("this is test dropdown element value", dropdownValue?.ToString());
                        break;

                    case FieldType.RadioButton:
                        var radioValue = fieldValue as RadiobuttonContent;
                        Assert.AreEqual(field.ElementId, radioValue?.Id, "Wrong dropdown ID");
                        Assert.AreEqual("this is test radiobutton field value", radioValue?.Data);
                        Assert.AreEqual("this is test radiobutton field value", radioValue?.ToString());
                        break;

                    default:
                        break;
                }
            }
        }
    }
}
