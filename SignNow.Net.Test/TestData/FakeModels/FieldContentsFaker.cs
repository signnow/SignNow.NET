using System;
using System.Text;
using Bogus;
using SignNow.Net.Model.FieldContents;

namespace SignNow.Net.Test.FakeModels
{
    /// <summary>
    /// Faker for <see cref="TextContent"/>
    /// </summary>
    internal class TextContentFaker : Faker<TextContent>
    {
        /// <summary>
        /// Creates new instance of <see cref="TextContent"/> fake object.
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
        public TextContentFaker()
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
    /// Faker for <see cref="HyperlinkContent"/>
    /// </summary>
    internal class HyperlinkContentFaker : Faker<HyperlinkContent>
    {
        /// <summary>
        /// Creates new instance of <see cref="HyperlinkContent"/> fake object.
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
        public HyperlinkContentFaker()
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
    /// Faker for <see cref="CheckboxContent"/>
    /// </summary>
    internal class CheckboxContentFaker : Faker<CheckboxContent>
    {
        /// <summary>
        /// Creates new instance of <see cref="CheckboxContent"/> fake object.
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
        public CheckboxContentFaker()
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
    /// Faker for <see cref="AttachmentContent"/>
    /// </summary>
    internal class AttachmentContentFaker : Faker<AttachmentContent>
    {
        /// <summary>
        /// Creates new instance of <see cref="AttachmentContent"/> fake object.
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
        public AttachmentContentFaker()
        {
            Rules((f, o) =>
            {
                o.Id            = f.Random.Hash(40);
                o.UserId        = f.Random.Hash(40);
                o.PageNumber    = f.Random.Int(0, 50);
                o.OriginalName  = f.System.FileName();
                o.FileName      = $"{f.Random.Hash(40)}.{f.System.FileExt()}";
                o.FileSize      = f.Random.ULong();
            });
        }
    }

    /// <summary>
    /// Faker for <see cref="EnumerationContent"/>
    /// </summary>
    internal class EnumerationContentFaker : Faker<EnumerationContent>
    {
        /// <summary>
        /// Creates new instance of <see cref="EnumerationContent"/> fake object.
        /// </summary>
        /// <example>
        /// This example shows Json representation.
        /// <code>
        /// {
        ///   "id": "0b66c18d352d680d9e718e9c1d95a2b6b8bcf97b",
        ///   "enumeration_id": "078f803b534ed8a1295f82319c1ed3e06b016931",
        ///   "data": "12345",
        ///   "created": "1582138620",
        ///   "updated": "1582208088"
        /// }
        /// </code>
        /// </example>
        public EnumerationContentFaker()
        {
            Rules((f, o) =>
            {
                o.Id            = f.Random.Hash(40);
                o.EnumerationId = f.Random.Hash(40);
                o.Data          = f.Lorem.Word();
                o.Created       = f.Date.Recent().ToUniversalTime();
                o.Updated       = f.Date.Recent().ToUniversalTime();
            });
        }
    }

    /// <summary>
    /// Faker for <see cref="RadiobuttonContent"/>
    /// </summary>
    internal class RadiobuttonContentFaker : Faker<RadiobuttonContent>
    {
        /// <summary>
        /// Creates new instance of <see cref="RadiobuttonContent"/> fake object.
        /// </summary>
        /// <example>
        /// This example shows Json representation.
        /// <code>
        /// {
        ///   "id": "0b66c18d352d680d9e718e9c1d95a2b6b8bcf97b",
        ///   "user_id": "a954c1e8a45ff6bb0d233d8ac160bd4e3271ca94",
        ///   "name": "RadiobuttonName_ipsum",
        ///   "created": "1582138620",
        ///   "x": "234",
        ///   "y": "15",
        ///   "page_number": "3",
        ///   "radio": [
        ///      {
        ///         "radio_id": "0b66c18d352d680d9e718e9c1d95a2b6b8bcf97b",
        ///         "created": "1582138620",
        ///         "page_number": "3",
        ///         "x": "551",
        ///         "y": "203",
        ///         "width": "30.00",
        ///         "height": "30.00",
        ///         "checked": "1",
        ///         "value": "RadioButtonValue_lorem"
        ///      }
        ///      ...
        ///    ]
        /// }
        /// </code>
        /// </example>
        public RadiobuttonContentFaker()
        {
            Rules((f, o) =>
            {
                o.Id            = f.Random.Hash(40);
                o.UserId        = f.Random.Hash(40);
                o.Name          = $"RadiobuttonName_{f.Lorem.Word()}";
                o.Created       = f.Date.Recent().ToUniversalTime();
                o.X             = f.Random.Int(0, 1024);
                o.Y             = f.Random.Int(0, 1024);
                o.Radio         = new RadioContentFaker()
                    .Rules((fkr, itm) => itm.PageNumber = o.PageNumber)
                    .Generate(f.Random.Int(2, 10));
            })
            .FinishWith((f, obj) => obj.Radio.FindLast(itm => !itm.Checked).Checked = true);
        }
    }

    /// <summary>
    /// Faker for <see cref="RadioContent"/>
    /// </summary>
    internal class RadioContentFaker : Faker<RadioContent>
    {
        /// <summary>
        /// Creates new instance of <see cref="RadioContent"/> fake object.
        /// </summary>
        /// <example>
        /// This example shows Json representation.
        /// <code>
        /// {
        ///   "radio_id": "0b66c18d352d680d9e718e9c1d95a2b6b8bcf97b",
        ///   "created": "1582138620",
        ///   "page_number": "2",
        ///   "x": "551",
        ///   "y": "203",
        ///   "width": "30.00",
        ///   "height": "30.00",
        ///   "checked": "1",
        ///   "value": "RadioButtonValue_lorem"
        /// }
        /// </code>
        /// </example>
        public RadioContentFaker()
        {
            Rules((f, o) =>
            {
                o.Id            = f.Random.Hash(40);
                o.Created       = f.Date.Recent().ToUniversalTime();
                o.PageNumber    = f.Random.Int(0, 50);
                o.X             = f.Random.Int(0, 1024);
                o.Y             = f.Random.Int(0, 1024);
                o.Width         = Math.Round(f.Random.Decimal(0, 100), 2);
                o.Height        = Math.Round(f.Random.Decimal(0, 100), 2);
                o.Checked       = false;
                o.Data          = $"RadiobuttonValue_{f.Lorem.Word()}";
            });
        }
    }

    /// <summary>
    /// Faker for <see cref="SignatureContent"/>
    /// </summary>
    internal class SignatureContentFaker : Faker<SignatureContent>
    {
        /// <summary>
        /// Creates new instance of <see cref="SignatureContent"/> fake object.
        /// </summary>
        /// <example>
        /// This example shows Json representation.
        /// <code>
        /// {
        ///   "id": "49d2b8a40be75e853f1352afe33dfbb015829039",
        ///   "user_id": "625f0b4b6f345dc3280385667bdb62f100a1344c",
        ///   "signature_request_id": "350c40cf474bdb496456608774fb4789214ec810",
        ///   "page_number": "0",
        ///   "email": "Alek.Keebler19@yahoo.com",
        ///   "created": "1581765801",
        ///   "data": "dGVzdDEuLi4="
        /// }
        /// </code>
        /// </example>
        public SignatureContentFaker()
        {
            Rules((f, o) =>
            {
                o.Id                 = f.Random.Hash(40);
                o.UserId             = f.Random.Hash(40);
                o.PageNumber         = f.Random.Int(0, 50);
                o.SignatureRequestId = f.Random.Hash(40);
                o.Email              = f.Internet.Email();
                o.Created            = f.Date.Recent().ToUniversalTime();
                o.Data               = Encoding.UTF8.GetBytes(f.Lorem.Paragraph());
            });
        }
    }
}
