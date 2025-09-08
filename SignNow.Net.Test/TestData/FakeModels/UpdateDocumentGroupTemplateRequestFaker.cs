using System.Collections.Generic;
using Bogus;
using SignNow.Net.Model.Requests.DocumentGroup;

namespace SignNow.Net.Test.FakeModels
{
    /// <summary>
    /// Faker <see cref="UpdateDocumentGroupTemplateRequest"/>
    /// </summary>
    public class UpdateDocumentGroupTemplateRequestFaker : Faker<UpdateDocumentGroupTemplateRequest>
    {
        /// <summary>
        /// Creates new instance of <see cref="UpdateDocumentGroupTemplateRequest"/> fake object.
        /// </summary>
        /// <example>
        /// This example shows Json representation.
        /// <code>
        /// {
        ///   "template_ids_to_add": ["ddc7ce43dfc5ad3b2f0fdb1db36889ce53f00789"],
        ///   "template_ids_to_remove": ["ddc7ce43dfc5ad3b2f0fdb1db36889ce53f00790"],
        ///   "routing_details": "{\"invite_steps\":[{\"order\":1,\"invite_emails\":[{\"email\":\"max@max.com\",\"subject\":\"Document Needs Your Signature\",\"message\":\"Please sign this document\",\"expiration_days\":30,\"reminder\":0,\"hasSignActions\":true,\"allow_reassign\":\"0\"}]}]}",
        ///   "template_group_name": "Updated Template Group"
        /// }
        /// </code>
        /// </example>
        public UpdateDocumentGroupTemplateRequestFaker()
        {
            Rules((f, o) =>
            {
                o.TemplateIdsToAdd = f.Make(f.Random.Int(0, 3), () => f.Random.Hash(40));
                o.TemplateIdsToRemove = f.Make(f.Random.Int(0, 2), () => f.Random.Hash(40));
                o.RoutingDetails = GenerateRoutingDetailsJson(f);
                o.TemplateGroupName = f.Commerce.ProductName() + " Template Group";
            });
        }

        private static string GenerateRoutingDetailsJson(Faker f)
        {
            var routingDetails = new
            {
                invite_steps = new[]
                {
                    new
                    {
                        order = 1,
                        invite_emails = new[]
                        {
                            new
                            {
                                email = f.Internet.Email(),
                                subject = f.Lorem.Sentence(5),
                                message = f.Lorem.Sentence(10),
                                expiration_days = f.Random.Int(7, 30),
                                reminder = f.Random.Int(0, 3),
                                hasSignActions = true,
                                allow_reassign = "0"
                            }
                        }
                    }
                },
                include_email_attachments = 0
            };

            return Newtonsoft.Json.JsonConvert.SerializeObject(routingDetails);
        }
    }
}
