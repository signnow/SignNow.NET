using System;
using SignNow.Net.Model;

namespace SignNow.Net.Interfaces
{
    public interface ISignNowInviteStatus
    {
        /// <summary>
        /// Unique identifier of invite.
        /// </summary>
        string Id { get; }

        /// <summary>
        /// Signer email.
        /// </summary>
        string SignerEmail { get; }

        /// <summary>
        /// Status of the invite sign request.
        /// </summary>
        SignStatus Status { get; }

        /// <summary>
        /// Timestamp invite was created.
        /// </summary>
        DateTime Created { get; }
    }
}
