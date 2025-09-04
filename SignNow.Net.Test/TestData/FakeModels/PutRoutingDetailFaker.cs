using System.Collections.Generic;
using System.Linq;
using Bogus;
using SignNow.Net.Model.Requests;
using SignNow.Net.Model.Responses;

namespace SignNow.Net.Test.FakeModels
{
    /// <summary>
    /// Faker <see cref="PutRoutingDetailRequest"/>
    /// </summary>
    public class PutRoutingDetailRequestFaker : Faker<PutRoutingDetailRequest>
    {
        /// <summary>
        /// Creates new instance of <see cref="PutRoutingDetailRequest"/> fake object.
        /// </summary>
        /// <example>
        /// This example shows Json representation.
        /// <code>
        /// {
        ///   "id": "e849617a2f26af2eb3d52e1251031050d933d6a6",
        ///   "document_id": "e996459c6b8cead31b8ec252898f91731cf3acd8",
        ///   "data": [
        ///     {
        ///       "default_email": "signer1@example.com",
        ///       "inviter_role": false,
        ///       "name": "Signer 1",
        ///       "role_id": "abc123def456ghi789jkl012mno345pqr678stu901vwx234yz",
        ///       "signer_order": 1,
        ///       "decline_by_signature": false
        ///     }
        ///   ],
        ///   "cc": ["cc1@example.com", "cc2@example.com"],
        ///   "cc_step": [
        ///     {
        ///       "email": "cc1@example.com",
        ///       "step": 1,
        ///       "name": "CC Recipient 1"
        ///     }
        ///   ],
        ///   "invite_link_instructions": "Please review and sign this document",
        ///   "viewers": [
        ///     {
        ///       "default_email": "viewer1@example.com",
        ///       "name": "Viewer 1",
        ///       "signing_order": 1,
        ///       "inviter_role": false,
        ///       "contact_id": "abc123def456ghi789jkl012mno345pqr678stu901vwx234yz"
        ///     }
        ///   ],
        ///   "approvers": [
        ///     {
        ///       "default_email": "approver1@example.com",
        ///       "name": "Approver 1",
        ///       "signing_order": 1,
        ///       "inviter_role": false,
        ///       "expiration_days": 15,
        ///       "contact_id": "abc123def456ghi789jkl012mno345pqr678stu901vwx234yz"
        ///     }
        ///   ]
        /// }
        /// </code>
        /// </example>
        public PutRoutingDetailRequestFaker()
        {
            Rules((f, o) =>
            {
                o.Id = f.Random.Hash(40); // 40-character ID
                o.DocumentId = f.Random.Hash(40); // 40-character ID
                o.Data = new PutRoutingDetailDataFaker().Generate(f.Random.Int(1, 3));
                o.Cc = f.Make(f.Random.Int(0, 3), () => f.Internet.Email()).ToList();
                o.CcStep = new PutCcStepFaker().Generate(f.Random.Int(0, 2));
                o.InviteLinkInstructions = f.Lorem.Sentence();
                o.Viewers = new PutViewerFaker().Generate(f.Random.Int(0, 2));
                o.Approvers = new PutApproverFaker().Generate(f.Random.Int(0, 2));
            });
        }
    }

    /// <summary>
    /// Faker <see cref="PutRoutingDetailData"/>
    /// </summary>
    public class PutRoutingDetailDataFaker : Faker<PutRoutingDetailData>
    {
        /// <summary>
        /// Creates new instance of <see cref="PutRoutingDetailData"/> fake object.
        /// </summary>
        public PutRoutingDetailDataFaker()
        {
            Rules((f, o) =>
            {
                o.DefaultEmail = f.Internet.Email();
                o.InviterRole = false; // Always false according to API spec
                o.Name = f.Name.FullName();
                o.RoleId = f.Random.Hash(40); // 40-character ID
                o.SignerOrder = f.Random.Int(1, 10);
                o.DeclineBySignature = f.Random.Bool();
            });
        }
    }

    /// <summary>
    /// Faker <see cref="PutCcStep"/>
    /// </summary>
    public class PutCcStepFaker : Faker<PutCcStep>
    {
        /// <summary>
        /// Creates new instance of <see cref="PutCcStep"/> fake object.
        /// </summary>
        public PutCcStepFaker()
        {
            Rules((f, o) =>
            {
                o.Email = f.Internet.Email();
                o.Step = f.Random.Int(1, 5);
                o.Name = f.Name.FullName();
            });
        }
    }

    /// <summary>
    /// Faker <see cref="PutViewer"/>
    /// </summary>
    public class PutViewerFaker : Faker<PutViewer>
    {
        /// <summary>
        /// Creates new instance of <see cref="PutViewer"/> fake object.
        /// </summary>
        public PutViewerFaker()
        {
            Rules((f, o) =>
            {
                o.DefaultEmail = f.Internet.Email();
                o.Name = f.Name.FullName();
                o.SigningOrder = f.Random.Int(1, 10);
                o.InviterRole = false; // Always false according to API spec
                o.ContactId = f.Random.Hash(40); // 40-character ID
            });
        }
    }

    /// <summary>
    /// Faker <see cref="PutApprover"/>
    /// </summary>
    public class PutApproverFaker : Faker<PutApprover>
    {
        /// <summary>
        /// Creates new instance of <see cref="PutApprover"/> fake object.
        /// </summary>
        public PutApproverFaker()
        {
            Rules((f, o) =>
            {
                o.DefaultEmail = f.Internet.Email();
                o.Name = f.Name.FullName();
                o.SigningOrder = f.Random.Int(1, 10);
                o.InviterRole = false; // Always false according to API spec
                o.ExpirationDays = f.Random.Bool() ? f.Random.Int(1, 30) : (int?)null;
                o.ContactId = f.Random.Hash(40); // 40-character ID
            });
        }
    }

    /// <summary>
    /// Faker <see cref="PutRoutingDetailResponse"/>
    /// </summary>
    public class PutRoutingDetailResponseFaker : Faker<PutRoutingDetailResponse>
    {
        /// <summary>
        /// Creates new instance of <see cref="PutRoutingDetailResponse"/> fake object.
        /// </summary>
        public PutRoutingDetailResponseFaker()
        {
            Rules((f, o) =>
            {
                o.TemplateData = new PutRoutingDetailTemplateDataFaker().Generate(f.Random.Int(1, 3));
                o.Cc = f.Make(f.Random.Int(0, 3), () => f.Internet.Email()).ToList();
                o.CcStep = new PutRoutingDetailCcStepFaker().Generate(f.Random.Int(0, 2));
                o.InviteLinkInstructions = f.Lorem.Sentence();
                o.Viewers = new PutRoutingDetailViewerFaker().Generate(f.Random.Int(0, 2));
                o.Approvers = new PutRoutingDetailApproverFaker().Generate(f.Random.Int(0, 2));
                o.Attributes = new PutRoutingDetailAttributesFaker().Generate();
            });
        }
    }

    /// <summary>
    /// Faker <see cref="PutRoutingDetailTemplateData"/>
    /// </summary>
    public class PutRoutingDetailTemplateDataFaker : Faker<PutRoutingDetailTemplateData>
    {
        /// <summary>
        /// Creates new instance of <see cref="PutRoutingDetailTemplateData"/> fake object.
        /// </summary>
        public PutRoutingDetailTemplateDataFaker()
        {
            Rules((f, o) =>
            {
                o.DefaultEmail = f.Internet.Email();
                o.InviterRole = false; // Always false according to API spec
                o.Name = f.Name.FullName();
                o.RoleId = f.Random.Hash(40); // 40-character ID
                o.SignerOrder = f.Random.Int(1, 10);
                o.DeclineBySignature = f.Random.Bool();
            });
        }
    }

    /// <summary>
    /// Faker <see cref="PutRoutingDetailCcStep"/>
    /// </summary>
    public class PutRoutingDetailCcStepFaker : Faker<PutRoutingDetailCcStep>
    {
        /// <summary>
        /// Creates new instance of <see cref="PutRoutingDetailCcStep"/> fake object.
        /// </summary>
        public PutRoutingDetailCcStepFaker()
        {
            Rules((f, o) =>
            {
                o.Email = f.Internet.Email();
                o.Step = f.Random.Int(1, 5);
                o.Name = f.Name.FullName();
            });
        }
    }

    /// <summary>
    /// Faker <see cref="PutRoutingDetailViewer"/>
    /// </summary>
    public class PutRoutingDetailViewerFaker : Faker<PutRoutingDetailViewer>
    {
        /// <summary>
        /// Creates new instance of <see cref="PutRoutingDetailViewer"/> fake object.
        /// </summary>
        public PutRoutingDetailViewerFaker()
        {
            Rules((f, o) =>
            {
                o.DefaultEmail = f.Internet.Email();
                o.Name = f.Name.FullName();
                o.SigningOrder = f.Random.Int(1, 10);
                o.InviterRole = false; // Always false according to API spec
                o.ContactId = f.Random.Hash(40); // 40-character ID
            });
        }
    }

    /// <summary>
    /// Faker <see cref="PutRoutingDetailApprover"/>
    /// </summary>
    public class PutRoutingDetailApproverFaker : Faker<PutRoutingDetailApprover>
    {
        /// <summary>
        /// Creates new instance of <see cref="PutRoutingDetailApprover"/> fake object.
        /// </summary>
        public PutRoutingDetailApproverFaker()
        {
            Rules((f, o) =>
            {
                o.DefaultEmail = f.Internet.Email();
                o.Name = f.Name.FullName();
                o.SigningOrder = f.Random.Int(1, 10);
                o.InviterRole = false; // Always false according to API spec
                o.ContactId = f.Random.Hash(40); // 40-character ID
            });
        }
    }

    /// <summary>
    /// Faker <see cref="PutRoutingDetailAttributes"/>
    /// </summary>
    public class PutRoutingDetailAttributesFaker : Faker<PutRoutingDetailAttributes>
    {
        /// <summary>
        /// Creates new instance of <see cref="PutRoutingDetailAttributes"/> fake object.
        /// </summary>
        public PutRoutingDetailAttributesFaker()
        {
            Rules((f, o) =>
            {
                o.BrandId = f.Random.Hash(40); // 40-character ID
                o.RedirectUri = f.Internet.Url();
                o.CloseRedirectUri = f.Internet.Url();
            });
        }
    }
}
