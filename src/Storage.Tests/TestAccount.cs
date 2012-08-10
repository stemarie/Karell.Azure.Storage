using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Karell.Azure.Storage.Tests
{
    [TestClass]
    public class TestAccount
    {
        [TestMethod]
        public void Test_CanInstantiateFromConnectionString()
        {
            var storage = new BlobStorage("UseDevelopmentStorage=true", "test");
            Assert.IsNotNull(storage.Account.Instance);
        }

        [TestMethod]
        public void Test_CanInstantiateFromAccount()
        {
            var account = new Account("UseDevelopmentStorage=true");
            var storage = new BlobStorage(account, "test");
            Assert.IsNotNull(storage.Account.Instance);
        }
    }
}
