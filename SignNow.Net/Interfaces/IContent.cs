using System.Net.Http;

namespace SignNow.Net.Interfaces
{
    /// <summary>
    /// Interface that represent <see cref="HttpContent"/> for <see cref="SignNow.Net.Model.RequestOptions"/>
    /// </summary>
    public interface IContent
    {
        HttpContent Content { get; }
    }
}
