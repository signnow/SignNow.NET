using System;
using SignNow.Net.Model.FieldContents;

namespace SignNow.Net.Interfaces
{
    /// <summary>
    /// Interface for all SignNow field types content retrieval.
    /// Resources that implement this interface can be used as SignNow FieldContent.
    /// <para>Possible concrete classes:</para>
    /// <list type="bullet">
    /// <item><description><see cref="AttachmentContent" /></description></item>
    /// <item><description><see cref="CheckboxContent" /></description></item>
    /// <item><description><see cref="HyperlinkContent" /></description></item>
    /// <item><description><see cref="RadiobuttonContent" /></description></item>
    /// <item><description><see cref="SignatureContent" /></description></item>
    /// <item><description><see cref="TextContent" /></description></item>
    /// </list>
    /// </summary>
    public interface ISignNowFieldContent
    {
        /// <summary>
        /// Returns the field value for any of SignNow FieldContent object.
        /// </summary>
        object GetFieldValue();
    }
}
