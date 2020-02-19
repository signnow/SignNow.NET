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
        ///  "page_number": "1",
        ///  "data": "test text value"
        /// }
        /// </code>
        /// </example>
        public TextFieldFaker()
        {
            Rules((f, o) =>
            {
                o.Id            = f.Random.Hash(40);
                o.UserId        = f.Random.Hash(40);
                o.Email         = f.Internet.Email();
                o.PageNumber    = f.IndexFaker + 1;
                o.Data          = f.Lorem.Word();
            });
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
        ///   "page_number": "2",
        ///   "label": "Signnow",
        ///   "data": "http:\/\/signnow.com"
        /// }
        /// </code>
        /// </example>
        public HyperlinkFieldFaker()
        {
            Rules((f, o) =>
            {
                o.Id            = f.Random.Hash(40);
                o.UserId        = f.Random.Hash(40);
                o.Email         = f.Internet.Email();
                o.PageNumber    = f.IndexFaker + 1;
                o.Label         = f.Internet.DomainWord();
                o.Data          = new Uri(f.Internet.Url());
            });
        }
    }

    /// <summary>
    /// Faker for <see cref="CheckboxField"/>
    /// </summary>
    internal class CheckboxFieldFaker : Faker<CheckboxField>
    {
        /// <summary>
        /// Creates new instance of <see cref="CheckboxField"/> fake object.
        /// </summary>
        /// <example>
        /// This example shows Json representation.
        /// <code>
        /// {
        ///   "id": "2ad6dd38b401bb4fad31858174e75642679a5bbd",
        ///   "user_id": "9640e7a2c6d2fca9282a6e24e3b5319a07718b34",
        ///   "email": "Bernadine46@yahoo.com",
        ///   "page_number": "3"
        /// }
        /// </code>
        /// </example>
        public CheckboxFieldFaker()
        {
            Rules((f, o) =>
            {
                o.Id            = f.Random.Hash(40);
                o.UserId        = f.Random.Hash(40);
                o.Email         = f.Internet.Email();
                o.PageNumber    = f.IndexFaker + 1;
            });
        }
    }

    /// <summary>
    /// Faker for <see cref="AttachmentField"/>
    /// </summary>
    internal class AttachmentFieldFaker : Faker<AttachmentField>
    {
        /// <summary>
        /// Creates new instance of <see cref="AttachmentField"/> fake object.
        /// </summary>
        /// <example>
        /// This example shows Json representation.
        /// <code>
        /// {
        ///   "id": "2ad6dd38b401bb4fad31858174e75642679a5bbd",
        ///   "user_id": "9640e7a2c6d2fca9282a6e24e3b5319a07718b34",
        ///   "page_number": "3",
        ///   "original_attachment_name": "repository-open-graph-template.png",
        ///   "filename": "40a594f2c3e4dd09735ed2f46fdbe7f6d2c39eaf.png",
        ///   "file_size": "51470"
        /// }
        /// </code>
        /// </example>
        public AttachmentFieldFaker()
        {
            Rules((f, o) =>
            {
                o.Id            = f.Random.Hash(40);
                o.UserId        = f.Random.Hash(40);
                o.PageNumber    = f.Random.Int(0, 50);
                o.OriginalName  = f.System.FileName();
                o.Filename      = $"{f.Random.Hash(40)}.{f.System.FileExt()}";
                o.FileSize      = f.Random.ULong();
            });
        }
    }
}
