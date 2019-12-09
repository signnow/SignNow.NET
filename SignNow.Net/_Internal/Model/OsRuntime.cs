using System;

namespace SignNow.Net.Internal.Model
{
    /// <summary>
    /// Represents runtime information: OS name, architecture, platform
    /// </summary>
    public class OsRuntime
    {
        /// <summary>
        /// OS name (e.g.: Linux, macOs, Windows)
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// OS architecture
        /// </summary>
        public string Arch { get; set; }

        /// <summary>
        /// OS platform
        /// </summary>
        public string Platform { get; set; }
    }
}
