using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sparky
{
    public class BankAccount
    {
        public int Balance { get; set; }
        private readonly ILogBook _logBook;
        public BankAccount(ILogBook logBook)
        {
            Balance = 0;
            _logBook = logBook;
        }

        public bool Deposit(int amount)
        {
            _logBook.Message("Deposit invoked");
            _logBook.Message("Temp");
            _logBook.LogSeverity = 101;
            Balance += amount;
            return true;
        }
        public bool Withdraw(int amount)
        {
            if (amount <= Balance)
            {
                _logBook.LogToDb("Withdrawal Amount:" + amount.ToString());
                Balance -= amount;
                return _logBook.LogBalanceAfterWithdrawal(Balance);
            }
            return _logBook.LogBalanceAfterWithdrawal(Balance-amount);
        }

        public int GetBalance() { return Balance; }


    }
}
