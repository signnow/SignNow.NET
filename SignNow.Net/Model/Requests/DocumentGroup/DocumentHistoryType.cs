using System.Runtime.Serialization;

namespace SignNow.Net.Model.Requests.DocumentGroup
{
    /// <summary>
    /// Whether or not to attach document history to the download.
    /// </summary>
    public enum DocumentHistoryType
    {
        /// <summary>
        /// Do not attach document history to the download.
        /// </summary>
        [EnumMember(Value = "no")]
        NoHistory,

        /// <summary>
        /// Attach document history to the download after each document.
        /// </summary>
        [EnumMember(Value = "after_each_document")]
        AfterEachDocument,

        /// <summary>
        /// Attach document history to the download after the merged PDF.
        /// </summary>
        [EnumMember(Value = "after_merged_pdf")]
        AfterMergedPdf
    }
}
