using Bogus;
using Bogus.Extensions;
using SignNow.Net.Internal.Model;
using SignNow.Net.Model;

namespace SignNow.Net.Test.FakeModels
{
    /// <summary>
    /// Faker <see cref="Field"/>
    /// </summary>
    internal class FieldFaker : Faker<Field>
    {
        /// <summary>
        /// Creates new instance of <see cref="Field"/> fake object.
        /// </summary>
        /// <example>
        /// This example shows Json representation.
        /// <code>
        /// {
        ///   "id": "c376990ca7e1e84ea7f6e252144e435f314bb63b",
        ///   "type": "Attachment",
        ///   "role_id": "fedaea9d9bf46196f048ff646bdde8d172c9fb12",
        ///   "role": "Signer 1",
        ///   "json_attributes": {
        ///      "page_number": 1,
        ///      "x": 992,
        ///      "y": 674,
        ///      "width": 463,
        ///      "height": 578,
        ///      "required": false,
        ///      "prefilled_text": "labore",
        ///      "label": "AttachmentLabelName",
        ///      "name": "AttachmentName"
        ///   },
        ///   "originator": "owner@gmail.com",
        ///   "fulfiller": "signer1@gmail.com",
        ///   "element_id": null
        /// }
        /// </code>
        /// </example>
        public FieldFaker()
        {
            // For Checkbox PrefilledText value can be only one of: "1" or ""
            var checkboxAllowedValues = new[] { "1", "" };

            Rules((f, o) =>
            {
                o.Id = f.Random.Hash(40);
                o.Type = f.PickRandom<FieldType>();
                o.RoleId = f.Random.Hash(40);
                o.RoleName = $"Signer {f.IndexFaker + 1}";
                o.JsonAttributes = new FieldJsonAttributesFaker()
                    .Rules((f1, obj) =>
                    {
                        obj.Name = $"{o.Type.ToString()}Name";
                        obj.PrefilledText = o.Type == FieldType.Checkbox ? f1.PickRandom(checkboxAllowedValues) : f1.Lorem.Word();
                    });
                o.Owner = f.Internet.Email();
                o.Signer = f.Internet.Email();
                // A nullable Id? with 80% probability of being null.
                o.ElementId = (string)f.Random.Hash(40).OrNull(f, .8f);
            })
                .FinishWith((f, o) =>
                o.RadioGroup = o.Type == FieldType.RadioButton ? new RadioContentFaker().Generate(2) : null);
        }
    }

    /// <summary>
    /// Faker <see cref="FieldJsonAttributes"/>
    /// </summary>
    internal class FieldJsonAttributesFaker : Faker<FieldJsonAttributes>
    {
        /// <summary>
        /// Creates new instance of <see cref="Field"/> fake object.
        /// </summary>
        /// <example>
        /// This example shows Json representation.
        /// <code>
        /// {
        ///   "page_number": 1,
        ///   "x": 992,
        ///   "y": 674,
        ///   "width": 463,
        ///   "height": 578,
        ///   "required": false,
        ///   "prefilled_text": "labore",
        ///   "label": "HyperlinkLabelName",
        ///   "name": "HyperlinkName"
        /// }
        /// </code>
        /// </example>
        public FieldJsonAttributesFaker()
        {
            Rules((f, o) =>
            {
                o.PageNumber    = f.IndexFaker + 1;
                o.X             = f.Random.Int(0, 1024);
                o.Y             = f.Random.Int(0, 1024);
                o.Width         = f.Random.Int(0, 1024);
                o.Height        = f.Random.Int(0, 1024);
                o.Required      = f.Random.Bool();
                o.PrefilledText = f.Lorem.Word();
                o.Name          = $"{f.PickRandom<FieldType>().ToString()}Name";
                o.Label         = $"{o.Name}Label";
            });
        }
    }
}
