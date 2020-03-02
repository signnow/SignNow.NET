using SignNow.Net.Model;

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
    }
}
