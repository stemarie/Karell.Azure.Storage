using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using Microsoft.WindowsAzure.StorageClient;

namespace Karell.Azure.Storage
{
    public class BlobStorage : BlobStorageBase
    {
        public BlobStorage(string connectionString, string containerName)
            : base(connectionString, containerName)
        { }

        public BlobStorage(Account account, string containerName)
            : base(account, containerName)
        { }

        public BlobStorage(Account account, CloudBlobClient client, string containerName)
            : base(account, client, containerName)
        { }

        public BlobStorage(Account account, CloudBlobClient client, CloudBlobContainer container)
            : base(account, client, container)
        { }

        public bool Delete(string fileName)
        {
            try
            {
                var blob = Container.GetBlobReference(fileName);
                blob.Delete();
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

        public bool Upload(string fileName, string dataContent, string contentType)
        {
            try
            {
                CloudBlob blob = Container.GetBlobReference(fileName);
                blob.UploadText(dataContent);
                blob.Properties.ContentType = contentType;
                blob.SetProperties();
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

        public string Download(string fileName)
        {
            try
            {
                CloudBlob blob = Container.GetBlobReference(fileName);
                string content = blob.DownloadText();
                return content;
            }
            catch (StorageClientException ex)
            {
                if ((int)ex.StatusCode == 404)
                {
                    return string.Empty;
                }

                throw;
            }
        }

        public bool UploadXml(string fileName, XDocument xml)
        {
            return Upload(fileName, xml.ToString(), "text/xml");
        }

        public XDocument DownloadXml(string fileName)
        {
            return XDocument.Parse(Download(fileName));
        }

        public bool UploadText(string fileName, string textContent)
        {
            return Upload(fileName, textContent, "text/text");
        }

        public bool UploadBinary(string fileName, byte[] dataContent, string contentType)
        {
            try
            {
                CloudBlob blob = Container.GetBlobReference(fileName);
                blob.UploadByteArray(dataContent);
                blob.Properties.ContentType = contentType;
                blob.SetProperties();
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

        public byte[] DownloadBinary(string fileName)
        {
            try
            {
                CloudBlob blob = Container.GetBlobReference(fileName);
                byte[] content = blob.DownloadByteArray();
                return content;
            }
            catch (StorageClientException ex)
            {
                if ((int)ex.StatusCode == 404)
                {
                    return null;
                }

                throw;
            }
        }
    }
}
