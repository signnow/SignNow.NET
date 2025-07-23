using System.Runtime.Serialization;

namespace SignNow.Net.Model
{
    /// <summary>
    /// Specific types of event of user or document
    /// </summary>
    public enum EventType
    {
        // Document events

        /// <summary>
        /// All required fields in the document have been filled in
        /// </summary>
        [EnumMember(Value = "document.complete")]
        DocumentComplete,

        /// <summary>
        /// The document has been deleted
        /// </summary>
        [EnumMember(Value = "document.delete")]
        DocumentDelete,

        /// <summary>
        /// The document has been opened
        /// </summary>
        [EnumMember(Value = "document.open")]
        DocumentOpen,

        /// <summary>
        /// The document has been edited
        /// </summary>
        [EnumMember(Value = "document.update")]
        DocumentUpdate,


        // User's actions with documents

        /// <summary>
        /// In the specified user's account a document has been completed
        /// </summary>
        [EnumMember(Value = "user.document.complete")]
        UserDocumentComplete,

        /// <summary>
        /// In the specified user's account a document has been created
        /// </summary>
        [EnumMember(Value = "user.document.create")]
        UserDocumentCreate,

        /// <summary>
        /// In the specified user's account a document has been deleted
        /// </summary>
        [EnumMember(Value = "user.document.delete")]
        UserDocumentDelete,

        /// <summary>
        /// In the specified user's account a document has been opened
        /// </summary>
        [EnumMember(Value = "user.document.open")]
        UserDocumentOpen,

        /// <summary>
        /// In the specified user's account a document has been edited
        /// </summary>
        [EnumMember(Value = "user.document.update")]
        UserDocumentUpdate,


        // Template events

        /// <summary>
        /// A document has been generated from a template
        /// </summary>
        [EnumMember(Value = "template.copy")]
        TemplateCopy,

        /// <summary>
        /// In the specified user's account a document has been generated from a template
        /// </summary>
        [EnumMember(Value = "user.template.copy")]
        UserTemplateCopy,


        // Field invite events
        // Field invite is an invite to sign the document which contains at least one Signature field.

        /// <summary>
        /// The field invite has been created for the specific document
        /// </summary>
        [EnumMember(Value = "document.fieldinvite.create")]
        DocumentFieldInviteCreate,

        /// <summary>
        /// The field invite for the specific document has been declined
        /// </summary>
        [EnumMember(Value = "document.fieldinvite.decline")]
        DocumentFieldInviteDecline,

        /// <summary>
        /// The field invite for the specific document has been deleted
        /// </summary>
        [EnumMember(Value = "document.fieldinvite.delete")]
        DocumentFieldInviteDelete,

        /// <summary>
        /// The field invite for the specific document has been reassigned to a different address by the signer
        /// </summary>
        [EnumMember(Value = "document.fieldinvite.reassign")]
        DocumentFieldInviteReassign,

        /// <summary>
        /// The field invite for the specific document has been sent out to the signer(s)
        /// </summary>
        [EnumMember(Value = "document.fieldinvite.sent")]
        DocumentFieldInviteSent,

        /// <summary>
        /// All Signature fields in the specific document have been signed
        /// </summary>
        [EnumMember(Value = "document.fieldinvite.signed")]
        DocumentFieldInviteSigned,

        /// <summary>
        /// The field invite for the specific document has been reassigned to a different address by the document owner
        /// </summary>
        [EnumMember(Value = "document.fieldinvite.replace")]
        DocumentFieldInviteReplace,


        // Field invite events triggered by Document Owner

        /// <summary>
        /// In the specified user's account a field invite has been created
        /// </summary>
        [EnumMember(Value = "user.document.fieldinvite.create")]
        UserDocumentFieldInviteCreate,

        /// <summary>
        /// In the specified user's account a field invite has been declined
        /// </summary>
        [EnumMember(Value = "user.document.fieldinvite.decline")]
        UserDocumentFieldInviteDecline,

        /// <summary>
        /// In the specified user's account a field invite has been deleted
        /// </summary>
        [EnumMember(Value = "user.document.fieldinvite.delete")]
        UserDocumentFieldInviteDelete,

        /// <summary>
        /// In the specified user's account a field invite has been reassigned to a different email address by the signer
        /// </summary>
        [EnumMember(Value = "user.document.fieldinvite.reassign")]
        UserDocumentFieldInviteReassign,

        /// <summary>
        /// In the specified user's account a field invite has been sent
        /// </summary>
        [EnumMember(Value = "user.document.fieldinvite.sent")]
        UserDocumentFieldInviteSent,

        /// <summary>
        /// In the specified user's account a field invite has been signed
        /// </summary>
        [EnumMember(Value = "user.document.fieldinvite.signed")]
        UserDocumentFieldInviteSigned,

        /// <summary>
        /// In the specified user's account a field invite has been reassigned to a different address by the document owner
        /// </summary>
        [EnumMember(Value = "user.document.fieldinvite.replace")]
        UserDocumentFieldInviteReplace,


        // Freeform invite events
        // Freeform invite - the invite to sign the document which has no fillable fields.

        /// <summary>
        /// The freeform invite to sign the document was created
        /// </summary>
        [EnumMember(Value = "document.freeform.create")]
        DocumentFreeformCreate,

        /// <summary>
        /// The freeform invite has been signed
        /// </summary>
        [EnumMember(Value = "document.freeform.signed")]
        DocumentFreeformSigned,


        // Freeform invite events triggered by Document Owners.

        /// <summary>
        /// Tn the specified user's account a freeform invite has been created
        /// </summary>
        [EnumMember(Value = "user.document.freeform.create")]
        UserDocumentFreeformCreate,

        /// <summary>
        /// Tn the specified user's account a freeform invite has been signed
        /// </summary>
        [EnumMember(Value = "user.document.freeform.signed")]
        UserDocumentFreeformSigned,


        // Document Group events

        /// <summary>
        /// The document group has been created by a specific user
        /// </summary>
        [EnumMember(Value = "user.document_group.create")]
        UserDocumentGroupCreate,

        /// <summary>
        /// The document group has been deleted
        /// </summary>
        [EnumMember(Value = "document_group.delete")]
        DocumentGroupDelete,

        /// <summary>
        /// The document group has been completed
        /// </summary>
        [EnumMember(Value = "document_group.complete")]
        DocumentGroupComplete,

        /// <summary>
        /// The invite to sign the document group has been created
        /// </summary>
        [EnumMember(Value = "document_group.invite.create")]
        DocumentGroupInviteCreate,

        /// <summary>
        /// The invite to sign the document group has been resent
        /// </summary>
        [EnumMember(Value = "document_group.invite.resend")]
        DocumentGroupInviteResend,

        /// <summary>
        /// The invite to sign the document group has been updated
        /// </summary>
        [EnumMember(Value = "document_group.invite.update")]
        DocumentGroupInviteUpdate,

        /// <summary>
        /// The invite to sign the document group has been canceled
        /// </summary>
        [EnumMember(Value = "document_group.invite.cancel")]
        DocumentGroupInviteCancel
    }
}
