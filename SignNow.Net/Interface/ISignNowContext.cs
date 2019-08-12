namespace SignNow.Net.Interface
{
    public interface ISignNowContext
    {
        IDocumentService Documents { get; }
        IUserService Users { get; }
    }
}
