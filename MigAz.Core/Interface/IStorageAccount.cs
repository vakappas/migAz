namespace MigAz.Core.Interface
{
    public interface IStorageAccount
    {
        string Id { get; }
        string Name { get; }
        string BlobStorageNamespace { get; }

        string AccountType { get; }
    }
}
