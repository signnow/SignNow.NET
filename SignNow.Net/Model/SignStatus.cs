namespace SignNow.Net.Model
{
    /// <summary>
    /// Represents statuses of document signing state.
    /// </summary>
    public enum SignStatus
    {
        /// <summary>
        /// Status for case when the document haven't any sign request.
        /// </summary>
        None,

        /// <summary>
        /// Status for case when the document have one or many sign requests which are not signed yet.
        /// </summary>
        Pending,

        /// <summary>
        /// Status for case when the document have one or many sign requests which are all signed.
        /// </summary>
        Completed,

        /// <summary>
        /// Status for case when signer was declined invite to sign the document.
        /// </summary>
        Declined,

        /// <summary>
        /// Status for case when the fill form in the document was filled by signer.
        /// </summary>
        Fulfilled,

        /// <summary>
        /// Status for document group when invite or action has been created but is not waiting to be signed.
        /// </summary>
        Created,

        /// <summary>
        /// Status for document role when invitee can skip sign order.
        /// </summary>
        Skipped
    }
}
