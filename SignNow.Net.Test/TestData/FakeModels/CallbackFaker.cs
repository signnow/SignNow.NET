using System;
using Bogus;
using SignNow.Net.Model;

namespace SignNow.Net.Test.FakeModels
{
    /// <summary>
    /// Faker for generating <see cref="Callback{T}"/> test data with <see cref="CallbackContentAllFields"/> content.
    /// </summary>
    public class CallbackFaker : Faker<Callback<CallbackContentAllFields>>
    {
        /// <summary>
        /// Faker for <see cref="Callback{CallbackContentAllFields}"/>
        /// </summary>
        /// <example>
        /// This example shows Json representation.
        /// <code>
        /// {
        ///   "id": "callback_123456789",
        ///   "application_name": "MyApp",
        ///   "entity_id": "doc_987654321",
        ///   "event_subscription_id": "sub_123456",
        ///   "event_subscription_active": true,
        ///   "entity_type": "document",
        ///   "event_name": "document.complete",
        ///   "callback_url": "https://example.com/webhook",
        ///   "request_method": "POST",
        ///   "duration": 1.5,
        ///   "request_start_time": 1609459200,
        ///   "request_end_time": 1609459205,
        ///   "request_headers": {
        ///     "string_head": "header_value",
        ///     "int_head": 42,
        ///     "bool_head": true,
        ///     "float_head": 3.14
        ///   },
        ///   "response_content": "OK",
        ///   "response_status_code": 200,
        ///   "event_subscription_owner_email": "owner@example.com",
        ///   "request_content": {
        ///     "meta": {
        ///       "timestamp": 1609459200,
        ///       "event": "document.complete",
        ///       "environment": "https://api.signnow.com",
        ///       "initiator_id": "user_456",
        ///       "callback_url": "https://example.com/webhook",
        ///       "access_token": "masked_token"
        ///     },
        ///     "content": {
        ///       "document_id": "doc_123",
        ///       "document_name": "Test Document.pdf",
        ///       "user_id": "user_456"
        ///     }
        ///   }
        /// }
        /// </code>
        /// </example>
        public CallbackFaker()
        {
            StrictMode(true);
            
            // Base callback properties
            RuleFor(o => o.Id, f => $"callback_{f.Random.Hash(10)}");
            RuleFor(o => o.ApplicationName, f => f.Company.CompanyName());
            RuleFor(o => o.EntityId, f => $"doc_{f.Random.Hash(10)}");
            RuleFor(o => o.EventSubscriptionId, f => $"sub_{f.Random.Hash(8)}");
            RuleFor(o => o.EventSubscriptionActive, f => f.Random.Bool(0.9f));
            RuleFor(o => o.EntityType, f => f.PickRandom<EventSubscriptionEntityType>());
            RuleFor(o => o.EventName, f => f.PickRandom<EventType>());
            RuleFor(o => o.CallbackUrl, f => new Uri(f.Internet.Url()));
            RuleFor(o => o.RequestMethod, f => f.PickRandom("POST", "PUT", "PATCH"));
            RuleFor(o => o.Duration, f => new TimeSpan((long) (f.Random.Double(0.1, 30.0) * TimeSpan.TicksPerSecond)));
            
            // Timestamps
            var baseTime = DateTimeOffset.UtcNow.AddDays(-7).DateTime;
            RuleFor(o => o.RequestStartTime, f => baseTime.AddSeconds(f.Random.Long(0, 604800))); // Within last 7 days
            RuleFor(o => o.RequestEndTime, (f, o) => o.RequestStartTime.AddSeconds(f.Random.Long(1, 300)) ); // 1-300 seconds after start
            
            // Headers
            RuleFor(o => o.RequestHeaders, f => f.Random.Bool(0.8f) ? new EventAttributeHeaders
            {
                StringHead = f.Lorem.Word(),
                IntHead = f.Random.Int(1, 1000),
                BoolHead = f.Random.Bool(),
                FloatHead = f.Random.Float(0, 100)
            } : null);
            
            // Response properties
            RuleFor(o => o.ResponseContent, f => f.Random.Bool(0.7f) ? f.PickRandom("OK", "Success", "Processed", "") : null);
            RuleFor(o => o.ResponseStatusCode, f => f.PickRandom(200, 201, 204, 400, 401, 403, 404, 500, 502, 503));
            RuleFor(o => o.EventSubscriptionOwnerEmail, f => f.Internet.Email());
            
            // Request content
            RuleFor(o => o.RequestContent, (f, o) => new CallbackRequestContent<CallbackContentAllFields>
            {
                Meta = new CallbackRequestMeta
                {
                    Timestamp = o.RequestStartTime,
                    Event = o.EventName,
                    Environment = new Uri(f.PickRandom("https://api.signnow.com", "https://api-eval.signnow.com")),
                    InitiatorId = $"user_{f.Random.Hash(8)}",
                    CallbackUrl = o.CallbackUrl,
                    AccessToken = $"***masked_token_{f.Random.Hash(6)}***"
                },
                Content = new CallbackContentAllFields
                {
                    DocumentId = o.EntityId,
                    DocumentName = f.System.FileName("pdf"),
                    UserId = $"user_{f.Random.Hash(8)}",
                    TemplateId = f.Random.Bool(0.3f) ? $"template_{f.Random.Hash(8)}" : null,
                    InviteId = f.Random.Bool(0.4f) ? $"invite_{f.Random.Hash(8)}" : null,
                    Signer = f.Random.Bool(0.4f) ? f.Internet.Email() : null,
                    Status = f.Random.Bool(0.5f) ? f.PickRandom("pending", "completed", "declined", "cancelled") : null,
                    OldInviteUniqueId = f.Random.Bool(0.2f) ? $"old_invite_{f.Random.Hash(8)}" : null,
                    GroupId = f.Random.Bool(0.2f) ? $"group_{f.Random.Hash(8)}" : null,
                    GroupName = f.Random.Bool(0.2f) ? string.Join("_", f.Lorem.Words(2)) : null,
                    GroupInvite = f.Random.Bool(0.2f) ? f.Random.Bool().ToString().ToLower() : null,
                    GroupInviteId = f.Random.Bool(0.2f) ? $"group_invite_{f.Random.Hash(8)}" : null,
                    InitiatorId = f.Random.Bool(0.3f) ? $"initiator_{f.Random.Hash(8)}" : null,
                    InitiatorEmail = f.Random.Bool(0.3f) ? f.Internet.Email() : null,
                    ViewerUserUniqueId = f.Random.Bool(0.2f) ? $"viewer_{f.Random.Hash(8)}" : null
                }
            });
        }

        /// <summary>
        /// Creates a successful callback (HTTP 2xx status codes).
        /// </summary>
        /// <returns>Faker configured for successful callbacks</returns>
        public CallbackFaker Successful()
        {
            var faker = new CallbackFaker();
            faker.RuleFor(o => o.ResponseStatusCode, f => f.PickRandom(200, 201, 204));
            faker.RuleFor(o => o.ResponseContent, f => f.PickRandom("OK", "Success", "Processed"));
            return faker;
        }

        /// <summary>
        /// Creates a failed callback (HTTP 4xx or 5xx status codes).
        /// </summary>
        /// <returns>Faker configured for failed callbacks</returns>
        public CallbackFaker Failed()
        {
            var faker = new CallbackFaker();
            faker.RuleFor(o => o.ResponseStatusCode, f => f.PickRandom(400, 401, 403, 404, 500, 502, 503));
            faker.RuleFor(o => o.ResponseContent, f => f.PickRandom("Bad Request", "Unauthorized", "Internal Server Error", ""));
            return faker;
        }

        /// <summary>
        /// Creates a callback for document events.
        /// </summary>
        /// <returns>Faker configured for document events</returns>
        public CallbackFaker DocumentEvent()
        {
            var faker = new CallbackFaker();
            faker.RuleFor(o => o.EntityType, f => EventSubscriptionEntityType.Document);
            faker.RuleFor(o => o.EventName, f => f.PickRandom(
                EventType.DocumentComplete, EventType.DocumentUpdate, EventType.DocumentOpen, 
                EventType.DocumentDelete, EventType.DocumentFieldInviteCreate, EventType.DocumentFieldInviteSigned));
            faker.RuleFor(o => o.EntityId, f => $"doc_{f.Random.Hash(10)}");
            return faker;
        }

        /// <summary>
        /// Creates a callback for user events.
        /// </summary>
        /// <returns>Faker configured for user events</returns>
        public CallbackFaker UserEvent()
        {
            var faker = new CallbackFaker();
            faker.RuleFor(o => o.EntityType, f => EventSubscriptionEntityType.User);
            faker.RuleFor(o => o.EventName, f => f.PickRandom(
                EventType.UserDocumentCreate, EventType.UserDocumentUpdate, EventType.UserDocumentComplete, 
                EventType.UserDocumentDelete, EventType.UserDocumentOpen));
            faker.RuleFor(o => o.EntityId, f => $"user_{f.Random.Hash(10)}");
            return faker;
        }

        /// <summary>
        /// Creates a callback for document group events.
        /// </summary>
        /// <returns>Faker configured for document group events</returns>
        public CallbackFaker DocumentGroupEvent()
        {
            var faker = new CallbackFaker();
            faker.RuleFor(o => o.EntityType, f => EventSubscriptionEntityType.DocumentGroup);
            faker.RuleFor(o => o.EventName, f => f.PickRandom(
                EventType.DocumentGroupComplete, EventType.DocumentGroupDelete, EventType.DocumentGroupUpdate,
                EventType.UserDocumentGroupCreate, EventType.UserDocumentGroupUpdate, EventType.UserDocumentGroupComplete));
            faker.RuleFor(o => o.EntityId, f => $"group_{f.Random.Hash(10)}");
            return faker;
        }

        /// <summary>
        /// Creates a callback for template events.
        /// </summary>
        /// <returns>Faker configured for template events</returns>
        public CallbackFaker TemplateEvent()
        {
            var faker = new CallbackFaker();
            faker.RuleFor(o => o.EntityType, f => EventSubscriptionEntityType.Template);
            faker.RuleFor(o => o.EventName, f => f.PickRandom(EventType.TemplateCopy, EventType.UserTemplateCopy));
            faker.RuleFor(o => o.EntityId, f => $"template_{f.Random.Hash(10)}");
            return faker;
        }
    }
}
