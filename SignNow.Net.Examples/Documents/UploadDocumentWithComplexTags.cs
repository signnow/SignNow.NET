using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SignNow.Net.Model;
using SignNow.Net.Model.ComplexTags;

namespace SignNow.Net.Examples
{
    public partial class DocumentExamples
    {
        [TestMethod]
        public async Task UploadDocumentWithComplexTagsAsync()
        {
            // Create complex tags collection based on tag name in {{ }} inside the document example
            var complexTags = new ComplexTextTags();
            complexTags.Properties.Add(new AttachmentTag
            {
                TagName = "AttachmentTagExample",
                Label = "Attach your files here",
                Role = "Signer 1",
                Required = true,
                Width = 200,
                Height = 20
            });
            complexTags.Properties.Add(new CheckBoxTag
            {
                TagName = "CheckboxTagExample",
                Role = "Signer 1",
                Required = true,
                Width = 12,
                Height = 12
            });
            complexTags.Properties.Add(new DateValidatorTag
            {
                TagName = "DateValidatorTagExample",
                Label = "Date of sign",
                Role = "Signer 1",
                Required = false,
                Width = 200,
                Height = 20,
                LockSigningDate = true,
                Validator = DataValidator.DateUS
            });
            complexTags.Properties.Add(new DropdownTag
            {
                TagName = "DropdownTagExample",
                Label = "Select item",
                Role = "Signer 1",
                Required = false,
                Width = 200,
                Height = 20,
                EnumerationOptions = new List<string>{"All", "None", "Custom"}
            });
            complexTags.Properties.Add(new HyperlinkTag
            {
                TagName = "HyperlinkTagExample",
                PageNumber = 0,
                Label = "signNow API documentation",
                Hint = "Click to view",
                Link = new Uri("https://docs.signnnow.com"),
                Role = "Signer 1",
                Required = false,
                Width = 200,
                Height = 20
            });

            var radioButton = new RadioButtonTag("my_options")
            {
                TagName = "RadioButtonTagExample",
                PageNumber = 0,
                Label = "Radio button label",
                Role = "Signer 1",
                Required = false,
                X = 72,
                Y = 658,
                Width = 200,
                Height = 20
            };
            radioButton.AddOption("value-1", 0, 0);
            radioButton.AddOption("value-2", 0, 14, 1);
            radioButton.AddOption("value-3", 0, 28);
            complexTags.Properties.Add(radioButton);

            complexTags.Properties.Add(new InitialsTag
            {
                TagName = "InitialsTagExample",
                Role = "Signer 1",
                Required = true,
                Width = 100,
                Height = 20
            });
            complexTags.Properties.Add(new SignatureTag
            {
                TagName = "SignatureTagExample",
                Role = "Signer 1",
                Required = true,
                Width = 200,
                Height = 20
            });

            // Upload test document
            await using var fileStream = File.OpenRead(PdfWithComplexTags);
            var testDocument = await testContext.Documents
                .UploadDocumentWithFieldExtractAsync(fileStream, "ComplexTextTags.pdf", complexTags)
                .ConfigureAwait(false);

            // Get document to check if all fields are present
            var actualDoc = await testContext.Documents.GetDocumentAsync(testDocument.Id).ConfigureAwait(false);

            // Dropdown tag may not be present as field if it's not allowed on Organization level settings
            Assert.IsTrue(actualDoc.Fields.Count >= complexTags.Properties.Count - 1);

            var documentFields = (List<Field>)actualDoc.Fields;

            // Check all fields are present in the document
            Assert.IsTrue(
                documentFields.Find(t => t.Type == FieldType.Attachment)?.Type == FieldType.Attachment,
                "Attachment field is not present in the document");
            Assert.IsTrue(
                documentFields.Find(t => t.Type == FieldType.Signature)?.Type == FieldType.Signature,
                "Signature field is not present in the document");
            Assert.IsTrue(
                documentFields.Find(t => t.Type == FieldType.Initials)?.Type == FieldType.Initials,
                "Initials field is not present in the document");
            Assert.IsTrue(
                documentFields.Find(t => t.Type == FieldType.Checkbox)?.Type == FieldType.Checkbox,
                "Checkbox field is not present in the document");
            Assert.IsTrue(
                documentFields.Find(t => t.Type == FieldType.Enumeration)?.Type == FieldType.Enumeration,
                "Dropdown field is not present in the document");
            Assert.IsTrue(
                documentFields.Find(t => t.Type == FieldType.RadioButton)?.Type == FieldType.RadioButton,
                "Radiobutton field is not present in the document");
            Assert.IsTrue(
                documentFields.Find(t => t.Type == FieldType.Text)?.Type == FieldType.Text,
                "Text field is not present in the document");

            // clean up
            DeleteTestDocument(actualDoc.Id);
        }
    }
}
