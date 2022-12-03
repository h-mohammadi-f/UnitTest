using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sparky
{
    [TestFixture]
    public class WrongBankAccountNUnitTests
    {
        //It is not a unit test. BEcause it will test the bank account with log book. 
        //we should just test one functionality related to bank account and not log book.

        private BankAccount bankAccount;
        [SetUp]
        public void Setup()
        {
            //a solution is to use log fakker:
            //it is not a good approach because it will end with a lot of unued code. 
            //We should use use a MOCK framework like MOQ. 
            //bankAccount = new(new LogFakker());
            bankAccount = new(new LogBook());
        }
        [Test]
        public void BankDeposit_Add100_ReturnTrue()
        {
            var result = bankAccount.Deposit(100);
            Assert.IsTrue(result);
            Assert.That(bankAccount.GetBalance,Is.EqualTo(100));
        }

    }
}
