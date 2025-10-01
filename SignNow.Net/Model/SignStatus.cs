namespace SignNow.Net.Model
{
    /// <summary>
    /// Represents statuses of document signing state.
    /// </summary>
    public enum DocumentStatus
    {
        /// <summary>
        /// Status for case when the document haven't any sign request.
        /// </summary>
        NoInvite,

        /// <summary>
        /// Status for case when the document have one or many sign requests which are not signed yet.
        /// </summary>
        Pending,

        /// <summary>
        /// Status for case when the document have one or many sign requests which are all signed.
        /// </summary>
        Completed
    }

    /// <summary>
    /// Represents statuses for fields (field invite or role-based invite)
    /// </summary>
    public enum InviteStatus
    {
        /// <summary>
        /// Status of field for document group when invite or action has been created but is not waiting to be signed.
        /// </summary>
        Created,

        /// <summary>
        /// Status of field for case when the document have one or many sign requests which are not signed yet.
        /// </summary>
        Pending,

        /// <summary>
        /// Status of field for case when the fill form in the document was filled by signer.
        /// </summary>
        Fulfilled,

        /// <summary>
        /// Status of field for document with conditional fields which can be skipped
        /// by choosing one of condition.
        /// </summary>
        Skipped
    }
}
