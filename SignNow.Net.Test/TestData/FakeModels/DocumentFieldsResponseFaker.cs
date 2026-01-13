using Bogus;
using SignNow.Net.Model;
using SignNow.Net.Model.Responses;

namespace SignNow.Net.Test.FakeModels
{
    /// <summary>
    /// Faker <see cref="DocumentFieldsResponse"/>
    /// </summary>
    public class DocumentFieldsResponseFaker : Faker<DocumentFieldsResponse>
    {
        /// <summary>
        /// Creates new instance of <see cref="DocumentFieldsResponse"/> fake object.
        /// </summary>
        /// <example>
        /// This example shows Json representation.
        /// <code>
        /// {
        ///   "data": [
        ///     {
        ///       "id": "350b5dce53694e2eb17b432ad75cc1d288827831",
        ///       "name": "TextName",
        ///       "type": "text",
        ///       "value": "dev test search"
        ///     },
        ///     {
        ///       "id": "9320dbacf5cf4d208deabd62dd1a7b4f0b9eed68",
        ///       "name": "datetime",
        ///       "type": "text",
        ///       "value": "10/11/2021"
        ///     }
        ///   ],
        ///   "meta": {
        ///     "pagination": {
        ///       "total": 4,
        ///       "count": 4,
        ///       "per_page": 15,
        ///       "current_page": 1,
        ///       "total_pages": 1,
        ///       "links": []
        ///     }
        ///   }
        /// }
        /// </code>
        /// </example>
        public DocumentFieldsResponseFaker()
        {
            Rules((f, o) =>
            {
                o.Data = new DocumentFieldDataFaker().Generate(f.Random.Int(1, 5));
                o.Meta = new MetaInfoFaker().Generate();
            });
        }
    }

    /// <summary>
    /// Faker <see cref="DocumentFieldData"/>
    /// </summary>
    public class DocumentFieldDataFaker : Faker<DocumentFieldData>
    {
        /// <summary>
        /// Creates new instance of <see cref="DocumentFieldData"/> fake object.
        /// </summary>
        public DocumentFieldDataFaker()
        {
            var fieldTypes = new[] { "text", "enumeration", "checkbox", "signature" };
            
            Rules((f, o) =>
            {
                o.Id = f.Random.Hash(40);
                o.Name = f.Lorem.Word();
                o.Type = f.PickRandom(fieldTypes);
                o.Value = f.Random.Bool() ? f.Lorem.Sentence() : null;
            });
        }
    }

    /// <summary>
    /// Faker <see cref="MetaInfo"/>
    /// </summary>
    public class MetaInfoFaker : Faker<MetaInfo>
    {
        /// <summary>
        /// Creates new instance of <see cref="MetaInfo"/> fake object.
        /// </summary>
        public MetaInfoFaker()
        {
            Rules((f, o) =>
            {
                o.Pagination = new PaginationFaker().Generate();
            });
        }
    }

    /// <summary>
    /// Faker <see cref="Pagination"/>
    /// </summary>
    public class PaginationFaker : Faker<Pagination>
    {
        /// <summary>
        /// Creates new instance of <see cref="Pagination"/> fake object.
        /// </summary>
        public PaginationFaker()
        {
            Rules((f, o) =>
            {
                o.Total = f.Random.Int(1, 100);
                o.Count = f.Random.Int(1, 15);
                o.PerPage = 15;
                o.CurrentPage = 1;
                o.TotalPages = 1;
                o.Links = new PageLinksFaker().Generate();
            });
        }
    }

    /// <summary>
    /// Faker <see cref="PageLinks"/>
    /// </summary>
    public class PageLinksFaker : Faker<PageLinks>
    {
        /// <summary>
        /// Creates new instance of <see cref="PageLinks"/> fake object.
        /// </summary>
        public PageLinksFaker()
        {
            Rules((f, o) =>
            {
                o.Previous = null;
                o.Next = null;
            });
        }
    }
}
