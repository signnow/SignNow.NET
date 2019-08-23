using SignNow.Net.Interfaces;
using SignNow.Net.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SignNow.Net.Test.UnitTests.Fakes
{
    //class FakeSignNowClient : ISignNowClient
    //{
    //    public async Task<TResponse> RequestAsync<TResponse>(RequestOptions requestOptions, CancellationToken cancellationToken = default) where TResponse : class
    //    {
    //        return
    //    }

    //    private TResponse CreateResponse<TResponse>(RequestOptions requestOptions)
    //    {
    //        switch (typeof(TResponse))
    //        {
    //            case UploadDocumentResponse: return CreateDocumentUploadResponse<TResponse>(); break;
    //            default: return default;
    //        }
    //    }
    //    private TResponse CreateDocumentUploadResponse<TResponse>()
    //    {
    //        return (TResponse)new UploadDocumentResponse { Id = "1234567890" };
    //    }
    //}
}
