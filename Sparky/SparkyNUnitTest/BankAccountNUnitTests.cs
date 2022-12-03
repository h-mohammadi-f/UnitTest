using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sparky
{
    [TestFixture]
    public class BankAccountNUnitTests
    {
        //It is not a unit test. BEcause it will test the bank account with log book. 
        //we should just test one functionality related to bank account and not log book.

        private BankAccount bankAccount;
        [SetUp]
        public void Setup()
        {
            //a solution is to use log fakker:
            //bankAccount = new(new LogFakker());
            //bankAccount = new(new LogBook());


        }
        [Test]
        public void BankDeposit_Add100_ReturnTrue()
        {
            //implementing MOQ
            var logMock = new Mock<ILogBook>();
            bankAccount = new BankAccount(logMock.Object);

            var result = bankAccount.Deposit(100);
            Assert.IsTrue(result);
            Assert.That(bankAccount.GetBalance, Is.EqualTo(100));
        }

        [Test]
        [TestCase(200, 100)]
        [TestCase(200, 150)]
        public void BankWithdraw_WithdrawaWithEnoughBalance_ReturnTrue(int balance, int withdraw)
        {
            //we are testing with moq depending on other objects
            var logMock = new Mock<ILogBook>();
            logMock.Setup(u => u.LogToDb(It.IsAny<string>())).Returns(true);
            logMock.Setup(u => u.LogBalanceAfterWithdrawal(It.Is<int>(x => x > 0))).Returns(true);

            bankAccount = new BankAccount(logMock.Object);
            bankAccount.Deposit(balance);
            var result = bankAccount.Withdraw(withdraw);

            Assert.IsTrue(result);

        }

        [Test]
        [TestCase(200, 300)]
        [TestCase(200, 250)]
        public void BankWithdraw_WithdrawaWithNotEnoughBalance_ReturnFalse(int balance, int withdraw)
        {
            //we are testing with moq depending on other objects
            var logMock = new Mock<ILogBook>();
            logMock.Setup(u => u.LogBalanceAfterWithdrawal(It.Is<int>(x => x > 0))).Returns(true);
            //logMock.Setup(u => u.LogBalanceAfterWithdrawal(It.Is<int>(x => x < 0))).Returns(false);
            logMock.Setup(u => u.LogBalanceAfterWithdrawal(It.IsInRange<int>(int.MinValue, -1, Moq.Range.Inclusive))).Returns(false);

            bankAccount = new BankAccount(logMock.Object);
            bankAccount.Deposit(balance);
            var result = bankAccount.Withdraw(withdraw);

            Assert.IsFalse(result);
        }


        [Test]
        public void BankLogDummy_LogMockString_ReturnMessage()
        {
            //we are testing with moq depending on other objects
            var logMock = new Mock<ILogBook>();
            string desiredOutput = "hello";

            logMock.Setup(u => u.MessageWithReturnStr(It.IsAny<string>())).Returns((string str) => str.ToLower());

            Assert.That(logMock.Object.MessageWithReturnStr("HELLO"), Is.EqualTo(desiredOutput));

        }

        [Test]
        public void BankLogDummy_LogMockStringOutputSTR_ReturnTrue()
        {
            //we are testing with moq depending on other objects
            var logMock = new Mock<ILogBook>();
            string desiredOutput = "hello";

            logMock.Setup(u => u.LogWithOutputResult(It.IsAny<string>(), out desiredOutput)).Returns(true);
            string result = "";
            Assert.IsTrue(logMock.Object.LogWithOutputResult("Hooman", out result));
            Assert.That(result, Is.EqualTo(desiredOutput));
        }

        [Test]
        public void BankLogDummy_LogRefChecker_ReturnTrue()
        {
            //we are testing with moq depending on other objects
            var logMock = new Mock<ILogBook>();
            Customer customer = new();
            Customer customerNotUsed = new();

            logMock.Setup(u => u.LogWithRefObj(ref customer)).Returns(true);

            Assert.IsTrue(logMock.Object.LogWithRefObj(ref customer));
            Assert.IsFalse(logMock.Object.LogWithRefObj(ref customerNotUsed));
        }

        [Test]
        public void BankLogDummy_SetAndGetProperties_MockTest()
        {
            var logMock = new Mock<ILogBook>();
            logMock.Setup(u => u.LogSeverity).Returns(10);
            logMock.Setup(u => u.LogType).Returns("Warning");
            //logMock.Object.LogSeverity = 100; it will not work

            //we should use like this if we want it works: 
            logMock.SetupAllProperties(); // after this we should set all properties
            logMock.Object.LogSeverity = 10;
            //if we remove this line the test will fails
            logMock.Setup(u => u.LogType).Returns("Warning");

            Assert.That(logMock.Object.LogSeverity, Is.EqualTo(10));
            Assert.That(logMock.Object.LogType, Is.EqualTo("Warning"));

            //callbacks

            string logTemp = "Hello, ";
            logMock.Setup(u => u.LogToDb(It.IsAny<string>()))
                .Returns(true).Callback((string str) => logTemp += str);

            logMock.Object.LogToDb("Hooman");

            Assert.That(logTemp, Is.EqualTo("Hello, Hooman"));
        }

        [Test]
        public void BankLogDummy_VerifyExample()
        {
            var logMock = new Mock<ILogBook>();
            BankAccount bankAccount = new BankAccount(logMock.Object);

            bankAccount.Deposit(100);
            Assert.That(bankAccount.GetBalance, Is.EqualTo(100));

            //verification to see how many time a method or property is accessed
            logMock.Verify(u => u.Message(It.IsAny<string>()), Times.Exactly(2));
            logMock.Verify(u => u.Message("Temp"), Times.AtLeastOnce);
            logMock.VerifySet(u => u.LogSeverity = 101, Times.Once);

        }

    }
}
