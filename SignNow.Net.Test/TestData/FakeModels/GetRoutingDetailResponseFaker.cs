using System.Collections.Generic;
using System.Linq;
using Bogus;
using SignNow.Net.Model;
using SignNow.Net.Model.Responses;

namespace SignNow.Net.Test.FakeModels
{
    /// <summary>
    /// Faker <see cref="GetRoutingDetailResponse"/>
    /// </summary>
    public class GetRoutingDetailResponseFaker : Faker<GetRoutingDetailResponse>
    {
        /// <summary>
        /// Creates new instance of <see cref="GetRoutingDetailResponse"/> fake object.
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
        ///       "signing_order": 1
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
        ///       "authentication": {
        ///         "type": "password"
        ///       }
        ///     }
        ///   ],
        ///   "attributes": {
        ///     "brand_id": "abc123def456ghi789jkl012mno345pqr678stu901vwx234yz",
        ///     "redirect_uri": "https://signnow.com",
        ///     "on_complete": "none"
        ///   }
        /// }
        /// </code>
        /// </example>
        public GetRoutingDetailResponseFaker()
        {
            Rules((f, o) =>
            {
                o.RoutingDetails = new RoutingDetailFaker().Generate(f.Random.Int(1, 3));
                o.Cc = f.Make(f.Random.Int(0, 3), () => f.Internet.Email()).ToList();
                o.CcStep = new CcStepFaker().Generate(f.Random.Int(0, 2));
                o.InviteLinkInstructions = f.Lorem.Sentence();
                o.Viewers = new ViewerFaker().Generate(f.Random.Int(0, 2));
                o.Approvers = new ApproverFaker().Generate(f.Random.Int(0, 2));
                o.Attributes = new RoutingAttributesFaker().Generate();
            });
        }
    }

    /// <summary>
    /// Faker <see cref="RoutingDetail"/>
    /// </summary>
    public class RoutingDetailFaker : Faker<RoutingDetail>
    {
        /// <summary>
        /// Creates new instance of <see cref="RoutingDetail"/> fake object.
        /// </summary>
        public RoutingDetailFaker()
        {
            Rules((f, o) =>
            {
                o.DefaultEmail = f.Internet.Email();
                o.InviterRole = false; // Always false according to API spec
                o.Name = f.Name.FullName();
                o.RoleId = f.Random.Hash(40); // 40-character ID
                o.SigningOrder = f.Random.Int(1, 10);
            });
        }
    }

    /// <summary>
    /// Faker <see cref="CcStep"/>
    /// </summary>
    public class CcStepFaker : Faker<CcStep>
    {
        /// <summary>
        /// Creates new instance of <see cref="CcStep"/> fake object.
        /// </summary>
        public CcStepFaker()
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
    /// Faker <see cref="Viewer"/>
    /// </summary>
    public class ViewerFaker : Faker<Viewer>
    {
        /// <summary>
        /// Creates new instance of <see cref="Viewer"/> fake object.
        /// </summary>
        public ViewerFaker()
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
    /// Faker <see cref="Approver"/>
    /// </summary>
    public class ApproverFaker : Faker<Approver>
    {
        /// <summary>
        /// Creates new instance of <see cref="Approver"/> fake object.
        /// </summary>
        public ApproverFaker()
        {
            Rules((f, o) =>
            {
                o.DefaultEmail = f.Internet.Email();
                o.Name = f.Name.FullName();
                o.SigningOrder = f.Random.Int(1, 10);
                o.InviterRole = false; // Always false according to API spec
                o.ExpirationDays = f.Random.Bool() ? f.Random.Int(1, 30) : (int?)null;
                o.Authentication = f.Random.Bool() ? new AuthenticationInfoFaker().Generate() : null;
            });
        }
    }

    /// <summary>
    /// Faker <see cref="AuthenticationInfo"/>
    /// </summary>
    public class AuthenticationInfoFaker : Faker<AuthenticationInfo>
    {
        /// <summary>
        /// Creates new instance of <see cref="AuthenticationInfo"/> fake object.
        /// </summary>
        public AuthenticationInfoFaker()
        {
            Rules((f, o) =>
            {
                o.Type = f.PickRandom(AuthenticationInfoType.Password, AuthenticationInfoType.Phone);
                
                // If type is Phone, set method and phone
                if (o.Type == AuthenticationInfoType.Phone)
                {
                    o.Method = f.PickRandom(PhoneAuthenticationMethod.PhoneCall, PhoneAuthenticationMethod.Sms);
                    o.Phone = f.Phone.PhoneNumber();
                }
            });
        }
    }

    /// <summary>
    /// Faker <see cref="RoutingAttributes"/>
    /// </summary>
    public class RoutingAttributesFaker : Faker<RoutingAttributes>
    {
        /// <summary>
        /// Creates new instance of <see cref="RoutingAttributes"/> fake object.
        /// </summary>
        public RoutingAttributesFaker()
        {
            Rules((f, o) =>
            {
                o.BrandId = f.Random.Hash(40); // 40-character ID
                o.RedirectUri = f.Internet.Url();
                o.OnComplete = f.PickRandom("none", "redirect", "close");
            });
        }
    }
}