using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.WindowsAzure.StorageClient;
using ProtoBuf;

namespace Karell.Azure.Storage.Tests
{
    [TestClass]
    public class TestGenericBlobStorage
    {
        private const string _containerName = "karellazurestoragegenericunittests";
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
        public void Test_GetPathName()
        {
            var x = new GenericBlobStorage<TestClass>(account, _containerName);
            Assert.AreEqual(@"Karell.Azure.Storage.Tests.TestGenericBlobStorage+TestClass/test", x.Filename("test"));
        }

        [TestMethod]
        public void Test_SaveItem()
        {
            var x = new GenericBlobStorage<TestClass>(account, _containerName);
            x.Create();
            TestClass expected = new TestClass { Filename = 5.ToString(), Value = 5 };
            x.Save(expected);
            TestClass value = x.Load(expected);
            Assert.AreEqual(expected.Value, value.Value);
            x.Delete(expected);
            x.Delete();
        }

        [ProtoContract]
        public class TestClass : IFilename
        {
            [ProtoMember(1)]
            public string Filename { get; set; }
            [ProtoMember(2)]
            public int Value { get; set; }
        }
    }
}
