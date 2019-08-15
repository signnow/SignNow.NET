using System;
using System.Collections.Generic;
using System.Text;

namespace SignNow.Net.Test
{
    public class ApiTestBase
    {
        public virtual Uri ApiBaseUrl => new Uri("https://api-eval.signnow.com/");
        public virtual Uri ApplicationBaseUrl => new Uri("https://app-eval.signnow.com/");
    }
}
