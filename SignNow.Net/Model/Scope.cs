using System.ComponentModel;

namespace SignNow.Net.Model
{
    public enum Scope
    {
        [Description("*")]
        All,
        [Description("user")]
        User
    }
}
