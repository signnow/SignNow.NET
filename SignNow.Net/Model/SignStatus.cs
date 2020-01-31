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
        Completed
    }
}
