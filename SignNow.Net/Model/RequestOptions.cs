using System;
using System.Collections.Generic;

namespace SignNow.Net.Model
{
    public abstract class RequestOptions
    {
        public object Content { get; set; }

        public Uri RequestUrl { get; set; }

        public abstract string HttpMethod { get; }

        public Token Token { get; set; }
    }

    public class GetHttpRequestOptions : RequestOptions
    {
        public override string HttpMethod => System.Net.Http.HttpMethod.Get.ToString();
    }

    public class PostHttpRequesOptions : RequestOptions
    {
        public override string HttpMethod => System.Net.Http.HttpMethod.Post.ToString();

        public PostHttpRequesOptions(object content)
        {
            Content = content;
        }
    }

    public class PutHttpRequesOptions : RequestOptions
    {
        public override string HttpMethod => System.Net.Http.HttpMethod.Put.ToString();

        public PutHttpRequesOptions(object content)
        {
            Content = content;
        }
    }

    public class DeleteHttpRequesOptions : RequestOptions
    {
        public override string HttpMethod => System.Net.Http.HttpMethod.Delete.ToString();
    }

    public class HeadHttpRequestOptions : RequestOptions
    {
        public override string HttpMethod => System.Net.Http.HttpMethod.Head.ToString();
    }

    public class OptionsHttpRequestOptions : RequestOptions
    {
        public override string HttpMethod => System.Net.Http.HttpMethod.Options.ToString();
    }
}