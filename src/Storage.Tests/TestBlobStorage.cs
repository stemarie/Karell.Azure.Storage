using System;
using System.Xml.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.WindowsAzure.StorageClient;

namespace Karell.Azure.Storage.Tests
{
    [TestClass]
    public class TestBlobStorage
    {
        private const string _containerName = "karellazurestorageunittests";
        Account account;

        [TestInitialize]
        public void Initialize()
        {
            account = new Account("UseDevelopmentStorage=true");

            // Let's nuke the container, to have clean tests
            try
            {
                var storage = new BlobStorage(account, _containerName);
                storage.Container.Delete();
            }
            catch (StorageClientException ex)
            {
                // we don't care about the exception here, because in most
                // cases, it will be because the previous test ran perfectly
                // and the container doesn't exist
            }
        }

        [TestCleanup]
        public void TearDown()
        { }

        [TestMethod]
        public void Test_CanCreateAndDeleteContainer()
        {
            var storage = new BlobStorage(account, _containerName);
            Assert.IsTrue(storage.Create());
            Assert.IsTrue(storage.Delete());
        }

        [TestMethod]
        public void Test_CanUploadRetrieveDeleteTextBlob()
        {
            var storage = new BlobStorage(account, _containerName);
            Assert.IsTrue(storage.Create());
            const string dataContent = "data content";
            Assert.IsTrue(storage.UploadText("test.txt", dataContent));
            string content = storage.Download("test.txt");
            Assert.AreEqual(dataContent, content);
            Assert.IsTrue(storage.Delete("test.txt"));
            Assert.IsTrue(storage.Delete());
        }

        [TestMethod]
        public void Test_CanUploadRetrieveDeleteXMLBlob()
        {
            var storage = new BlobStorage(account, _containerName);
            Assert.IsTrue(storage.Create());
            XDocument xml = new XDocument(
                new XComment("This is a comment"),
                new XElement("Root",
                             new XElement("Child1", "data1"),
                             new XElement("Child2", "data2"),
                             new XElement("Child3", "data3"),
                             new XElement("Child2", "data4"),
                             new XElement("Info5", "info5"),
                             new XElement("Info6", "info6"),
                             new XElement("Info7", "info7"),
                             new XElement("Info8", "info8")
                    )
                );

            Assert.IsTrue(storage.UploadXml("test.xml", xml));
            XDocument content = storage.DownloadXml("test.xml");
            Assert.AreEqual(xml.ToString(), content.ToString());
            Assert.IsTrue(storage.Delete("test.xml"));
            Assert.IsTrue(storage.Delete());
        }

        [TestMethod]
        public void Test_CanUploadRetrieveDeleteBinaryBlob()
        {
            var storage = new BlobStorage(account, _containerName);
            Assert.IsTrue(storage.Create());
            byte[] dataContent = new byte[] { 1, 2, 3 };
            const string fileName = "test.bin";
            Assert.IsTrue(storage.UploadBinary(fileName, dataContent, "binary/binary"));
            byte[] content = storage.DownloadBinary(fileName);
            for (int i = 0; i < 3; i++)
            {
                Assert.AreEqual(dataContent[i], content[i]);
            }
            Assert.IsTrue(storage.Delete(fileName));
            Assert.IsTrue(storage.Delete());
        }
    }
}