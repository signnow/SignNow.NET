using SignNow.Net.Interfaces;
using SignNow.Net.Internal.Requests;

namespace SignNow.Net.Model
{
    public class FreeFormInvite : ISignInvite
    {
        /// <inheritdoc />
        public string Sender { get; set; }

        /// <inheritdoc />
        public string Recipient { get; set; }

        public IContent InviteContent()
        {
            return new JsonHttpContent(
                new
                {
                    from = Sender,
                    to = Recipient
                }
                );
        }
    }
}
