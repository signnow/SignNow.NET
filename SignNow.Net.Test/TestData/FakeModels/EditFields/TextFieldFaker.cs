using Bogus;
using SignNow.Net.Model.EditFields;

namespace SignNow.Net.Test.FakeModels.EditFields
{
    /// <summary>
    /// Faker <see cref="TextField"/>
    /// </summary>
    internal class TextFieldFaker : Faker<TextField>
    {
        /// <summary>
        /// Creates new instance of <see cref="TextField"/> fake object.
        /// </summary>
        /// <example>
        /// This example shows Json representation.
        /// <code>
        /// {
        ///   "page_number": 1,
        ///   "type": "text",
        ///   "name": "Text_1",
        ///   "role": "Signer 1",
        ///   "required": false,
        ///   "x": 992,
        ///   "y": 674,
        ///   "width": 463,
        ///   "height": 578,
        ///   "prefilled_text": "labore",
        ///   "label": "Text_1_Label",
        /// }
        /// </code>
        /// </example>
        public TextFieldFaker()
        {
            Rules((f, o) =>
            {
                o.PageNumber    = f.IndexFaker + 1;
                o.Name          = $"Text_{f.IndexFaker + 1}";
                o.Role          = $"Signer {f.IndexFaker + 1}";
                o.Required      = f.Random.Bool();
                o.X             = f.Random.Int(0, 1024);
                o.Y             = f.Random.Int(0, 1024);
                o.Width         = f.Random.Int(0, 1024);
                o.Height        = f.Random.Int(0, 1024);
                o.PrefilledText = f.Lorem.Word();
                o.Label         = $"{o.Name}_Label";
            });
        }
    }
}
