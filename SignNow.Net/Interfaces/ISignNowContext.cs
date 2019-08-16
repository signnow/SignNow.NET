namespace SignNow.Net.Interfaces
{
    public interface ISignNowContext
    {
        IDocumentService Documents { get; }
        IUserService Users { get; }
    }
}
