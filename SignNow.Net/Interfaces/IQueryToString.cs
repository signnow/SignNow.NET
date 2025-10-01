namespace SignNow.Net.Interfaces
{
    /// <summary>
    /// Interface that allow to build http query string from DTO class properties
    /// </summary>
    public interface IQueryToString
    {
        /// <summary>
        /// Build http query string from class properties
        /// </summary>
        /// <returns>query string representation</returns>
        public string ToQueryString();
    }
}
