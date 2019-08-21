using System;
using System.Collections.Generic;
using Method = System.Net.Http.HttpMethod;

namespace SignNow.Net.Model
{
    public abstract class RequestOptions
    {
        public object Content { get; set; }

        public Uri RequestUrl { get; set; }

        public abstract Method HttpMethod { get; }

        public Token Token { get; set; }
       
       /*
        Previous:
        public string URL { get; set; }
        public string AuthorizationCode { get; set; }
        public string Accept { get; set; } = "application/json";
        public string ContentType { get; set; } = "application/x-www-form-urlencoded";
        public string Login { get; set; }
        public string Password { get; set; }
        public string GrantType { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public Scope Scope { get; set; } = Scope.All;
        */
    }

    public class GetHttpRequestOptions : RequestOptions
    {
        public override Method HttpMethod => Method.Get;
    }

    public class PostHttpRequesOptions : RequestOptions
    {
        public override Method HttpMethod => Method.Post;

        public PostHttpRequesOptions(object content = null)
        {
            Content = content;
        }
    }

    public class PutHttpRequesOptions : RequestOptions
    {
        public override Method HttpMethod => Method.Put;

        public PutHttpRequesOptions(object content = null)
        {
            Content = content;
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