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
}
