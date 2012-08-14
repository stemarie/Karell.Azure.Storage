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
            Container.GetBlobReference(Filename(i.Filename)).UploadByteArray(Serialize(i));
        }

        public T Load(IFilename item)
        {
            return Load(Filename(item.Filename));
        }

        public bool Exists(IFilename item)
        {
            return Container.GetBlobReference(Filename(item.Filename)).Exists();
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
            Container.GetBlobReference(Filename(item.Filename)).DeleteIfExists();
        }

        public T Load(string uri)
        {
            return Deserialize(Container.GetBlobReference(uri).DownloadByteArray());
        }

        public IList<T> List()
        {
            return Container
                .GetDirectoryReference(GetDirectory())
                .ListBlobs()
                .Select(x => x.Uri.ToString())
                .ToList()
                .Select(Load)
                .ToList();
        }
    }
}
