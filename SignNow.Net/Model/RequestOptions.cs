namespace SignNow.Net.Model
{
    public class RequestOptions
    {
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
    }
}
