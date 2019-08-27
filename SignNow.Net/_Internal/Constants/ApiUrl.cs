using System;

namespace SignNow.Net.Internal.Constants
{
    static class ApiUrl
    {
#if DEBUG
        public static Uri ApiBaseUrl = new Uri("https://api-eval.signnow.com");
#else
        public static Uri ApiBaseUrl = new Uri("https://api.signnow.com");	  
#endif
    }
}
