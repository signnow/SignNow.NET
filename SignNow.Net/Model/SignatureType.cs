using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace SignNow.Net.Model
{
    /// <summary>
    /// Type of QES signature requested from signers.
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum SignatureType
    {
        /// <summary>
        /// EID Easy signature type
        /// </summary>
        [EnumMember(Value = "eideasy")]
        Eideasy,

        /// <summary>
        /// EID Easy PDF signature type
        /// </summary>
        [EnumMember(Value = "eideasy-pdf")]
        EideasyPdf,

        /// <summary>
        /// NOM 151 signature type
        /// </summary>
        [EnumMember(Value = "nom151")]
        Nom151
    }
}
