using System.Collections.Generic;
using System.Linq;
using Bogus;
using SignNow.Net.Model.Responses;

namespace SignNow.Net.Test.FakeModels
{
    /// <summary>
    /// Faker <see cref="PostRoutingDetailResponse"/>
    /// </summary>
    public class PostRoutingDetailResponseFaker : Faker<PostRoutingDetailResponse>
    {
        /// <summary>
        /// Creates new instance of <see cref="PostRoutingDetailResponse"/> fake object.
        /// </summary>
        /// <example>
        /// This example shows Json representation.
        /// <code>
        /// {
        ///   "routing_details": [
        ///     {
        ///       "default_email": "signer1@example.com",
        ///       "inviter_role": false,
        ///       "name": "Signer 1",
        ///       "role_id": "abc123def456ghi789jkl012mno345pqr678stu901vwx234yz",
        ///       "signer_order": 1
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
        ///   "invite_link_instructions": "Please review and sign this document"
        /// }
        /// </code>
        /// </example>
        public PostRoutingDetailResponseFaker()
        {
            Rules((f, o) =>
            {
                o.RoutingDetails = new PostRoutingDetailFaker().Generate(f.Random.Int(1, 3));
                o.Cc = f.Make(f.Random.Int(0, 3), () => f.Internet.Email()).ToList();
                o.CcStep = new PostCcStepFaker().Generate(f.Random.Int(0, 2));
                o.InviteLinkInstructions = f.Lorem.Sentence();
            });
        }
    }

    /// <summary>
    /// Faker <see cref="PostRoutingDetail"/>
    /// </summary>
    public class PostRoutingDetailFaker : Faker<PostRoutingDetail>
    {
        /// <summary>
        /// Creates new instance of <see cref="PostRoutingDetail"/> fake object.
        /// </summary>
        public PostRoutingDetailFaker()
        {
            Rules((f, o) =>
            {
                o.DefaultEmail = f.Internet.Email();
                o.InviterRole = false; // Always false according to API spec
                o.Name = f.Name.FullName();
                o.RoleId = f.Random.Hash(40); // 40-character ID
                o.SignerOrder = f.Random.Int(1, 10);
            });
        }
    }

    /// <summary>
    /// Faker <see cref="PostCcStep"/>
    /// </summary>
    public class PostCcStepFaker : Faker<PostCcStep>
    {
        /// <summary>
        /// Creates new instance of <see cref="PostCcStep"/> fake object.
        /// </summary>
        public PostCcStepFaker()
        {
            Rules((f, o) =>
            {
                o.Email = f.Internet.Email();
                o.Step = f.Random.Int(1, 5);
                o.Name = f.Name.FullName();
            });
        }
    }
}