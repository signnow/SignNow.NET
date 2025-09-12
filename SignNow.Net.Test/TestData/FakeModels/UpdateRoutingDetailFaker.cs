using System.Collections.Generic;
using System.Linq;
using Bogus;
using SignNow.Net.Model.Requests;
using SignNow.Net.Model.Responses;

namespace SignNow.Net.Test.FakeModels
{
    /// <summary>
    /// Faker <see cref="UpdateRoutingDetailRequest"/>
    /// </summary>
    public class UpdateRoutingDetailRequestFaker : Faker<UpdateRoutingDetailRequest>
    {
        /// <summary>
        /// Creates new instance of <see cref="UpdateRoutingDetailRequest"/> fake object.
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
        public UpdateRoutingDetailRequestFaker()
        {
            Rules((f, o) =>
            {
                o.Id = f.Random.Hash(40); // 40-character ID
                o.DocumentId = f.Random.Hash(40); // 40-character ID
                o.Data = new RoutingDetailDataFaker().Generate(f.Random.Int(1, 3));
                o.Cc = f.Make(f.Random.Int(0, 3), () => f.Internet.Email()).ToList();
                o.CcStep = new UpdateRoutingDetailCcStepFaker().Generate(f.Random.Int(0, 2));
                o.InviteLinkInstructions = f.Lorem.Sentence();
                o.Viewers = new UpdateRoutingDetailViewerFaker().Generate(f.Random.Int(0, 2));
                o.Approvers = new UpdateRoutingDetailApproverFaker().Generate(f.Random.Int(0, 2));
            });
        }
    }

    /// <summary>
    /// Faker <see cref="RoutingDetailData"/>
    /// </summary>
    public class RoutingDetailDataFaker : Faker<RoutingDetailData>
    {
        /// <summary>
        /// Creates new instance of <see cref="RoutingDetailData"/> fake object.
        /// </summary>
        public RoutingDetailDataFaker()
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
    /// Faker <see cref="UpdateRoutingDetailCcStep"/>
    /// </summary>
    public class UpdateRoutingDetailCcStepFaker : Faker<SignNow.Net.Model.Requests.UpdateRoutingDetailCcStep>
    {
        /// <summary>
        /// Creates new instance of <see cref="UpdateRoutingDetailCcStep"/> fake object.
        /// </summary>
        public UpdateRoutingDetailCcStepFaker()
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
    /// Faker <see cref="UpdateRoutingDetailViewer"/>
    /// </summary>
    public class UpdateRoutingDetailViewerFaker : Faker<SignNow.Net.Model.Requests.UpdateRoutingDetailViewer>
    {
        /// <summary>
        /// Creates new instance of <see cref="UpdateRoutingDetailViewer"/> fake object.
        /// </summary>
        public UpdateRoutingDetailViewerFaker()
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
    /// Faker <see cref="UpdateRoutingDetailApprover"/>
    /// </summary>
    public class UpdateRoutingDetailApproverFaker : Faker<SignNow.Net.Model.Requests.UpdateRoutingDetailApprover>
    {
        /// <summary>
        /// Creates new instance of <see cref="UpdateRoutingDetailApprover"/> fake object.
        /// </summary>
        public UpdateRoutingDetailApproverFaker()
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
    /// Faker <see cref="UpdateRoutingDetailResponse"/>
    /// </summary>
    public class UpdateRoutingDetailResponseFaker : Faker<UpdateRoutingDetailResponse>
    {
        /// <summary>
        /// Creates new instance of <see cref="UpdateRoutingDetailResponse"/> fake object.
        /// </summary>
        public UpdateRoutingDetailResponseFaker()
        {
            Rules((f, o) =>
            {
                o.TemplateData = new UpdateRoutingDetailResponseTemplateDataFaker().Generate(f.Random.Int(1, 3));
                o.Cc = f.Make(f.Random.Int(0, 3), () => f.Internet.Email()).ToList();
                o.CcStep = new UpdateRoutingDetailResponseCcStepFaker().Generate(f.Random.Int(0, 2));
                o.InviteLinkInstructions = f.Lorem.Sentence();
                o.Viewers = new UpdateRoutingDetailResponseViewerFaker().Generate(f.Random.Int(0, 2));
                o.Approvers = new UpdateRoutingDetailResponseApproverFaker().Generate(f.Random.Int(0, 2));
                o.Attributes = new UpdateRoutingDetailResponseAttributesFaker().Generate();
            });
        }
    }

    /// <summary>
    /// Faker <see cref="SignNow.Net.Model.Responses.UpdateRoutingDetailTemplateData"/>
    /// </summary>
    public class UpdateRoutingDetailResponseTemplateDataFaker : Faker<SignNow.Net.Model.Responses.UpdateRoutingDetailTemplateData>
    {
        /// <summary>
        /// Creates new instance of <see cref="UpdateRoutingDetailTemplateData"/> fake object.
        /// </summary>
        public UpdateRoutingDetailResponseTemplateDataFaker()
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
    /// Faker <see cref="SignNow.Net.Model.Responses.UpdateRoutingDetailCcStep"/>
    /// </summary>
    public class UpdateRoutingDetailResponseCcStepFaker : Faker<SignNow.Net.Model.Responses.UpdateRoutingDetailCcStep>
    {
        /// <summary>
        /// Creates new instance of <see cref="SignNow.Net.Model.Responses.UpdateRoutingDetailCcStep"/> fake object.
        /// </summary>
        public UpdateRoutingDetailResponseCcStepFaker()
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
    /// Faker <see cref="SignNow.Net.Model.Responses.UpdateRoutingDetailViewer"/>
    /// </summary>
    public class UpdateRoutingDetailResponseViewerFaker : Faker<SignNow.Net.Model.Responses.UpdateRoutingDetailViewer>
    {
        /// <summary>
        /// Creates new instance of <see cref="SignNow.Net.Model.Responses.UpdateRoutingDetailViewer"/> fake object.
        /// </summary>
        public UpdateRoutingDetailResponseViewerFaker()
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
    /// Faker <see cref="SignNow.Net.Model.Responses.UpdateRoutingDetailApprover"/>
    /// </summary>
    public class UpdateRoutingDetailResponseApproverFaker : Faker<SignNow.Net.Model.Responses.UpdateRoutingDetailApprover>
    {
        /// <summary>
        /// Creates new instance of <see cref="SignNow.Net.Model.Responses.UpdateRoutingDetailApprover"/> fake object.
        /// </summary>
        public UpdateRoutingDetailResponseApproverFaker()
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
    /// Faker <see cref="SignNow.Net.Model.Responses.UpdateRoutingDetailAttributes"/>
    /// </summary>
    public class UpdateRoutingDetailResponseAttributesFaker : Faker<SignNow.Net.Model.Responses.UpdateRoutingDetailAttributes>
    {
        /// <summary>
        /// Creates new instance of <see cref="SignNow.Net.Model.Responses.UpdateRoutingDetailAttributes"/> fake object.
        /// </summary>
        public UpdateRoutingDetailResponseAttributesFaker()
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
