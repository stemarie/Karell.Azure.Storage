using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.WindowsAzure.StorageClient;

namespace Karell.Azure.Storage
{
    public interface IFilename
    {
        string Filename { get; }
    }

    public class GenericBlobStorage<T> : BlobStorageBase
        where T : IFilename
    {
        public GenericBlobStorage(string connectionString, string containerName)
            : base(connectionString, containerName)
        { }

        public GenericBlobStorage(Account account, string containerName)
            : base(account, containerName)
        { }

        public GenericBlobStorage(Account account, CloudBlobClient client, string containerName)
            : base(account, client, containerName)
        { }

        public GenericBlobStorage(Account account, CloudBlobClient client, CloudBlobContainer container)
            : base(account, client, container)
        { }

        public string Filename(string fileName)
        {
            return string.Format("{0}/{1}", GetTypename(), fileName);
        }

        private static string GetDirectory()
        {
            return GetTypename();
        }

        private static string GetTypename()
        {
            return typeof(T).ToString();
        }

        public void Save(T i)
        {
            var blob = Container.GetBlobReference(Filename(i.Filename));
            blob.UploadByteArray(Serialize(i));
        }

        public T Load(IFilename item)
        {
            return Load(Filename(item.Filename));
        }

        public bool Exists(IFilename item)
        {
            var blob = Container.GetBlobReference(Filename(item.Filename));
            return blob.Exists();
        }

        private static byte[] Serialize(T data)
        {
            using (var ms = new MemoryStream())
            {
                ProtoBuf.Serializer.Serialize(ms, data);
                return ms.ToArray();
            }
        }

        private static T Deserialize(byte[] data)
        {
            using (var ms = new MemoryStream(data))
            {
                return ProtoBuf.Serializer.Deserialize<T>(ms);
            }
        }

        public void Delete(T item)
        {
            var blob = Container.GetBlobReference(Filename(item.Filename));
            blob.DeleteIfExists();
        }

        public T Load(string uri)
        {
            var blob = Container.GetBlobReference(uri);
            return Deserialize(blob.DownloadByteArray());
        }

        public List<string> List()
        {
            return Container.GetDirectoryReference(GetDirectory()).ListBlobs().Select(x => x.Uri.ToString()).ToList();
        }
    }
}
