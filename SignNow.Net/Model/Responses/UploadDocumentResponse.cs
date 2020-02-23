namespace SignNow.Net.Model
{
    /// <summary>
    /// Represents response from SignNow API for upload document request.
    /// </summary>
    public class UploadDocumentResponse
    {
        /// <summary>
        /// Document ID of then newly created file.
        /// </summary>
        public string Id { get; set; }
    }
}
