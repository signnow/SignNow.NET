using System;
using System.Collections.Generic;
using Bogus;
using Bogus.Extensions;
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

            base
                .RuleFor(r => r.Id, f => f.Random.Hash(40))
                .RuleFor(r => r.Type, f => f.PickRandom<FieldType>())
                .RuleFor(r => r.RoleId, f => f.Random.Hash(40))
                .RuleFor(r => r.RoleName, f => $"Signer {f.IndexFaker + 1}")
                .RuleFor(
                    r => r.JsonAttributes,
                    (f, r) => new FieldJsonAttributesFaker()
                        .RuleFor(fld => fld.Name, f1 => $"{r.Type.ToString()}Name")
                        .RuleFor(fld => fld.PrefilledText,
                            r.Type == FieldType.Checkbox ? f.PickRandom(checkboxAllowedValues) : f.Lorem.Word()))
                .RuleFor(r => r.Owner, f => f.Internet.Email())
                .RuleFor(r => r.Signer, f => f.Internet.Email())
                // A nullable Id? with 80% probability of being null.
                .RuleFor(r => r.ElementId, f => f.Random.Hash(40).OrNull(f, .8f));
        }
    }

    /// <summary>
    /// Faker <see cref="FieldJsonAttributes"/>
    /// </summary>
    public class FieldJsonAttributesFaker : Faker<FieldJsonAttributes>
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
            base
                .RuleFor(fld => fld.PageNumber, f => f.IndexFaker + 1)
                .RuleFor(fld => fld.X, f => f.Random.Int(0, 1024))
                .RuleFor(fld => fld.Y, f => f.Random.Int(0, 1024))
                .RuleFor(fld => fld.Width, f => f.Random.Int(0, 1024))
                .RuleFor(fld => fld.Height, f => f.Random.Int(0, 1024))
                .RuleFor(fld => fld.Required, f => f.Random.Bool())
                .RuleFor(fld => fld.PrefilledText, f => f.Lorem.Word())
                .RuleFor(fld => fld.Name, f => $"{f.PickRandom<FieldType>().ToString()}Name")
                .RuleFor(fld => fld.Label, (f, fld) => $"{fld.Name}Label");
        }
    }
}
