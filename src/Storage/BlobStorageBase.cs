using Microsoft.WindowsAzure.StorageClient;

namespace Karell.Azure.Storage
{
    public class BlobStorageBase
    {
        public BlobStorageBase(string connectionString, string containerName)
            : this(new Account(connectionString), containerName)
        { }

        public BlobStorageBase(Account account, string containerName)
            : this(account, account.Instance.CreateCloudBlobClient(), containerName)
        { }

        public BlobStorageBase(Account account, CloudBlobClient client, string containerName)
            : this(account, client, client.GetContainerReference(containerName))
        { }

        public BlobStorageBase(Account account, CloudBlobClient client, CloudBlobContainer container)
        {
            Account = account;
            Client = client;
            Container = container;
        }

        public CloudBlobClient Client { get; private set; }
        public Account Account { get; private set; }
        public CloudBlobContainer Container { get; private set; }

        public bool Create()
        {
            try
            {
                Container.Create();
                return true;
            }
            catch (StorageClientException ex)
            {
                if ((int)ex.StatusCode == 409)
                {
                    return false;
                }

                throw;
            }
        }

        public bool Delete()
        {
            try
            {
                Container.Delete();
                return true;
            }
            catch (StorageClientException ex)
            {
                if ((int)ex.StatusCode == 404)
                {
                    return false;
                }

                throw;
            }
        }
    }
}