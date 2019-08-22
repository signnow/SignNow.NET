using SignNow.Net.Service;
using System;
using System.Net.Http;
using SignNow.Net.Interfaces;
using Method = System.Net.Http.HttpMethod;

namespace SignNow.Net.Model
{
    public abstract class RequestOptions
    {
        public IContent Content { get; set; }

        public Uri RequestUrl { get; set; }

        public abstract Method HttpMethod { get; }

        public Token Token { get; set; }

    }

    public class GetHttpRequestOptions : RequestOptions
    {
        public override Method HttpMethod => Method.Get;
    }

    public class PostHttpRequesOptions : RequestOptions
    {
        public override Method HttpMethod => Method.Post;

        public PostHttpRequesOptions(IContent ContentObj = null)
        {
            Content = ContentObj;
        }
    }

    public class PutHttpRequesOptions : RequestOptions
    {
        public override Method HttpMethod => Method.Put;

        public PutHttpRequesOptions(IContent ContentObj = null)
        {
            Content = ContentObj;
        }
    }

    public class DeleteHttpRequesOptions : RequestOptions
    {
        public override Method HttpMethod => Method.Delete;
    }

    public class HeadHttpRequestOptions : RequestOptions
    {
        public override Method HttpMethod => Method.Head;
    }

    public class OptionsHttpRequestOptions : RequestOptions
    {
        public override Method HttpMethod => Method.Options;
    }
}