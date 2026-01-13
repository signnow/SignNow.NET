using Newtonsoft.Json;

namespace SignNow.Net.Model
{
    /// <summary>
    /// Base class for CC step information
    /// </summary>
    public abstract class CcStepBase
    {
        /// <summary>
        /// Email of cc step
        /// </summary>
        [JsonProperty("email")]
        public string Email { get; set; }

        /// <summary>
        /// Step number
        /// </summary>
        [JsonProperty("step")]
        public int Step { get; set; }

        /// <summary>
        /// Name of cc step
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
