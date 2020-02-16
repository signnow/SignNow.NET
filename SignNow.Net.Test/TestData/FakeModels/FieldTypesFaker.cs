using System;
using Bogus;
using SignNow.Net.Internal.Model.FieldTypes;

namespace SignNow.Net.Test.FakeModels
{
    /// <summary>
    /// Faker for <see cref="TextField"/>
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
        ///  "id": "a77b550226c6f41ab9677f0d7b24ac70aea9c47a",
        ///  "user_id": "068d235caa6a4a3c75fcb0a7d71c78c13abbbd93"
        ///  "email": "Judson63@yahoo.com",
        ///  "data": "test text value"
        /// }
        /// </code>
        /// </example>
        public TextFieldFaker()
        {
            RuleFor(obj => obj.Id, f => f.Random.Hash(40));
            RuleFor(obj => obj.UserId, f => f.Random.Hash(40));
            RuleFor(obj => obj.Email, f => f.Internet.Email());
            RuleFor(obj => obj.Data, f => f.Lorem.Word());
        }
    }

    /// <summary>
    /// Faker for <see cref="HyperlinkField"/>
    /// </summary>
    internal class HyperlinkFieldFaker : Faker<HyperlinkField>
    {
        /// <summary>
        /// Creates new instance of <see cref="HyperlinkField"/> fake object.
        /// </summary>
        /// <example>
        /// This example shows Json representation.
        /// <code>
        /// {
        ///   "id": "2ad6dd38b401bb4fad31858174e75642679a5bbd",
        ///   "user_id": "9640e7a2c6d2fca9282a6e24e3b5319a07718b34",
        ///   "email": "Bernadine46@yahoo.com",
        ///   "data": "http:\/\/signnow.com",
        ///   "label": "Signnow",
        ///   "created": "1581630901"
        /// }
        /// </code>
        /// </example>
        public HyperlinkFieldFaker()
        {
            RuleFor(obj => obj.Id, f => f.Random.Hash(40));
            RuleFor(obj => obj.UserId, f => f.Random.Hash(40));
            RuleFor(obj => obj.Email, f => f.Internet.Email());
            RuleFor(obj => obj.Data, f => new Uri(f.Internet.Url()));
            RuleFor(obj => obj.Label, f => f.Internet.DomainWord());
        }
    }
}
