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

        /// <summary>
        /// Allows to get only callbacks of type Callback&lt;T&gt; where T class inherited from EventContentCallbackBase
        /// </summary>
        public IEnumerable<Callback<T>> GetCallbacksWith<T>() where T : EventContentCallbackBase
        {
            var eventTypes = typeof(T).Name switch
            {
                nameof(DocumentDeleteEventContent) => new[] {
                    EventType.DocumentDelete, EventType.UserDocumentDelete
                },
                nameof(DocumentUpdateEventContent) => new[] {
                    EventType.DocumentUpdate, EventType.UserDocumentUpdate, EventType.UserDocumentCreate, EventType.UserDocumentComplete, EventType.DocumentComplete
                },
                nameof(DocumentOpenEventContent) => new[] {
                    EventType.DocumentOpen, EventType.UserDocumentOpen
                },
                nameof(TemplateCopyEventContent) => new[] {
                    EventType.TemplateCopy, EventType.UserTemplateCopy
                },
                nameof(DocumentInviteEventContent) => new[] {
                    EventType.UserDocumentFieldInviteCreate, EventType.UserDocumentFieldInviteDecline, EventType.UserDocumentFieldInviteDelete,
                    EventType.UserDocumentFieldInviteSigned, EventType.UserDocumentFieldInviteSent, EventType.UserDocumentFreeformCreate,
                    EventType.UserDocumentFreeformSigned, EventType.DocumentFieldInviteCreate, EventType.DocumentFieldInviteDecline,
                    EventType.DocumentFieldInviteDelete, EventType.DocumentFieldInviteSigned, EventType.DocumentFieldInviteSent,
                    EventType.DocumentFreeformCreate, EventType.DocumentFreeformSigned
                },
                nameof(DocumentInviteReassignEventContent) => new[] {
                    EventType.UserDocumentFieldInviteReassign, EventType.DocumentFieldInviteReassign
                },
                nameof(DocumentInviteReplaceEventContent) => new[] {
                    EventType.UserDocumentFieldInviteReplace, EventType.DocumentFieldInviteReplace
                },
                nameof(DocumentGroupEventContent) => new[] {
                    EventType.UserDocumentGroupCreate, EventType.UserDocumentGroupUpdate, EventType.UserDocumentGroupComplete,
                    EventType.DocumentGroupUpdate, EventType.DocumentGroupComplete
                },
                nameof(DocumentGroupDeleteEventContent) => new[] {
                    EventType.DocumentGroupDelete, EventType.UserDocumentGroupDelete
                },
                nameof(DocumentGroupInviteEventContent) => new[] {
                    EventType.UserDocumentGroupInviteCreate, EventType.UserDocumentGroupInviteResend, EventType.UserDocumentGroupInviteUpdate,
                    EventType.UserDocumentGroupInviteCancel, EventType.DocumentGroupInviteCreate, EventType.DocumentGroupInviteResend,
                    EventType.DocumentGroupInviteUpdate, EventType.DocumentGroupInviteCancel
                },
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
