using SignNow.Net.Model;
using SignNow.Net.Model.FieldContents;

namespace SignNow.Net.Interfaces
{
    /// <summary>
    /// Interface for all SignNow Fields.
    /// </summary>
    public interface ISignNowField
    {
        /// <summary>
        /// Get SignNow field type.
        /// </summary>
        FieldType Type { get; }

        /// <summary>
        /// Returns identity for element with content.
        /// </summary>
        string ElementId { get; }

        /// <summary>
        /// Field attributes: name, label, x/y coordinates, width, height...
        /// </summary>
        FieldJsonAttributes JsonAttributes { get; }
    }
}
