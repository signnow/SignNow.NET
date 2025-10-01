using Bogus;
using SignNow.Net.Model;

namespace SignNow.Net.Test.FakeModels
{
    public sealed class FolderFaker : Faker<Folder>
    {
        /// <summary>
        /// Faker <see cref="Folder"/>
        /// </summary>
        /// <example>
        /// This example shows Json representation.
        /// <code>
        /// {
        ///    "id": "e1d8d63ba51c4009ab8241f279c908a0fd5a5e48",
        ///    "parent_id": "a7138ccc971e98080bfa999cc32d4bef4cca51a9",
        ///    "user_id": "8f882dc816e2ab59288a9744dd84dee3ba67c0b3",
        ///    "name": "Documents",
        ///    "created": "1566560035",
        ///    "shared": false,
        ///    "document_count": "150",
        ///    "template_count": "0",
        ///    "folder_count": "0"
        /// }
        /// </code>
        /// </example>
        public FolderFaker()
        {
            Rules((f, o) =>
            {
                o.Id = f.Random.Hash(40);
                o.ParentId = f.Random.Hash(40);
                o.UserId = f.Random.Hash(40);
                o.Name = f.System.FileName();
                o.Created = f.Date.Recent().ToUniversalTime();
                o.Shared = f.Random.Bool();
                o.TotalDocuments = f.Random.Int(0, 100);
                o.TotalTemplates = f.Random.Int(0, 100);
                o.TotalFolders = f.Random.Int(0, 100);
            });
        }
    }

    public sealed class SignNowFoldersFaker : Faker<SignNowFolders>
    {
        /// <summary>
        /// Faker <see cref="SignNowFolders"/>
        /// </summary>
        /// <example>
        /// This example shows Json representation.
        /// <code>
        /// {
        ///     "id": "6bda660e1f5b4396ac5e0d4828abc1834f031473",
        ///     "created": "1623708677",
        ///     "name": "virgin_islands__u.s..p8",
        ///     "user_id": "f5d5ffec3b8648eb51f61642646427f44732c5d2",
        ///     "parent_id": "a7138ccc971e98080bfa999cc32d4bef4cca51a9",
        ///     "system_folder": false,
        ///     "shared": false,
        ///     "total_documents": 7,
        ///     "folders": [],
        ///     "documents": []
        /// }
        /// </code>
        /// </example>
        public SignNowFoldersFaker()
        {
            Rules((f, o) =>
            {
                o.Id = f.Random.Hash(40);
                o.Created = f.Date.Recent().ToUniversalTime();
                o.Name = f.System.FileName();
                o.UserId = f.Random.Hash(40);
                o.ParentId = f.Random.Hash(40);
                o.SystemFolder = f.Random.Bool();
                o.Shared = f.Random.Bool();
                o.TotalDocuments = f.Random.Int(0, 10);
                o.Folders = new FolderFaker().Generate(1);
                o.Documents = new SignNowDocumentFaker().Generate(o.TotalDocuments);
            });
        }
    }
}
