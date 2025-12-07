using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace SignNow.Net.Model.Responses
{
    /// <summary>
    /// Response containing a list of callback events with metadata.
    /// This follows the v2 API response structure with data and meta fields.
    /// </summary>
    public class CallbacksResponse
    {
        private readonly static EventType[] documentDeleteTypes = new[] {
            EventType.DocumentDelete, EventType.UserDocumentDelete
        };
        private readonly static EventType[] documentUpdateTypes = new[] {
            EventType.DocumentUpdate, EventType.UserDocumentUpdate,
            EventType.UserDocumentCreate, EventType.UserDocumentComplete, EventType.DocumentComplete
        };
        private readonly static EventType[] documentOpenTypes = new[] {
            EventType.DocumentOpen, EventType.UserDocumentOpen
        };
        private readonly static EventType[] templateCopyTypes = new[] {
            EventType.TemplateCopy, EventType.UserTemplateCopy
        };
        private readonly static EventType[] documentInviteTypes = new[] {
            EventType.UserDocumentFieldInviteCreate,
            EventType.UserDocumentFieldInviteDecline, EventType.UserDocumentFieldInviteDelete, EventType.UserDocumentFieldInviteSigned,
            EventType.UserDocumentFieldInviteSent, EventType.UserDocumentFreeformCreate, EventType.UserDocumentFreeformSigned,
            EventType.DocumentFieldInviteCreate, EventType.DocumentFieldInviteDecline, EventType.DocumentFieldInviteDelete,
            EventType.DocumentFieldInviteSigned, EventType.DocumentFieldInviteSent, EventType.DocumentFreeformCreate, EventType.DocumentFreeformSigned
        };
        private readonly static EventType[] documentInviteReassignTypes = new[] {
            EventType.UserDocumentFieldInviteReassign, EventType.DocumentFieldInviteReassign
        };
        private readonly static EventType[] documentInviteReplaceTypes = new[] {
            EventType.UserDocumentFieldInviteReplace, EventType.DocumentFieldInviteReplace
        };
        private readonly static EventType[] documentGroupTypes = new[] { 
            EventType.UserDocumentGroupCreate, EventType.UserDocumentGroupUpdate, EventType.UserDocumentGroupComplete, 
            EventType.DocumentGroupUpdate, EventType.DocumentGroupComplete
        };
        private readonly static EventType[] documentGroupDeleteTypes = new[] {
            EventType.DocumentGroupDelete, EventType.UserDocumentGroupDelete
        };
        private readonly static EventType[] documentGroupInviteTypes = new[] { 
            EventType.UserDocumentGroupInviteCreate, EventType.UserDocumentGroupInviteResend, EventType.UserDocumentGroupInviteUpdate, 
            EventType.UserDocumentGroupInviteCancel, EventType.DocumentGroupInviteCreate, EventType.DocumentGroupInviteResend, 
            EventType.DocumentGroupInviteUpdate, EventType.DocumentGroupInviteCancel
        };

        /// <summary>
        /// The list of callback events.
        /// </summary>
        [JsonProperty("data")]
        public IReadOnlyList<Callback<CallbackContentAllFields>> Data { get; set; }

        /// <summary>
        /// Metadata information including pagination details.
        /// </summary>
        [JsonProperty("meta")]
        public MetaInfo Meta { get; set; }

        // Allow to filter & get only Callback<ConcreetModel> 
        public IEnumerable<Callback<T>> Get<T>()
        {
            var eventTypes = typeof(T).Name switch
            {
                nameof(DocumentDeleteEventContent) => documentDeleteTypes,
                nameof(DocumentUpdateEventContent) => documentUpdateTypes,
                nameof(DocumentOpenEventContent) => documentOpenTypes,
                nameof(TemplateCopyEventContent) => templateCopyTypes,
                nameof(DocumentInviteEventContent) => documentInviteTypes,
                nameof(DocumentInviteReassignEventContent) => documentInviteReassignTypes,
                nameof(DocumentInviteReplaceEventContent) => documentInviteReplaceTypes,
                nameof(DocumentGroupEventContent) => documentGroupTypes,
                nameof(DocumentGroupDeleteEventContent) => documentGroupDeleteTypes,
                nameof(DocumentGroupInviteEventContent) => documentGroupInviteTypes,
                _ => null
            };
            return Data
                .Where(d => eventTypes.Contains(d.EventName))
                .Select(d =>
                {
                    var json = JsonConvert.SerializeObject(d);
                    return JsonConvert.DeserializeObject<Callback<T>>(json);
                });
        }
    }
}
