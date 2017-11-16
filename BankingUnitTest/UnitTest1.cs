using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using BankingLibrary;

namespace BankingUnitTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            Account account = new Account(4378932);
            account.Withdraw(100);
            Assert.AreEqual(0, account.CheckBalance());
        }

        [TestMethod]
        public void TestMethod2()
        {
            Account account = new Account(4378932);
            account.Deposit(100);
            Assert.AreEqual(100.0, account.CheckBalance());
        }

        [TestMethod]
        public void TestMethod3()
        {
            Account account = new Account(4378932);
            account.Deposit(100);
            account.Withdraw(100);
            Assert.AreEqual(0, account.CheckBalance());
        }
    }
}
