using System;
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
        FieldType FieldType();

        /// <summary>
        /// Returns FieldContent ID.
        /// </summary>
        string GetFieldContentId();
    }
}
