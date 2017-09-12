using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingLibrary
{
    public class Account
    {
        // Fields
        double Balance = 0;
        string Name;
        int Number;

        // Constructors
        public Account()
        {
            // Initialize stuff here
            Balance = 0;
            Name = "Checking";
            Number = 0;     // INVALID ACCOUNT NUMBER
        }
        
        // Creates an account with the specified account number
        public Account(int number)
        {
            Number = number;
        }

        // Methods



        // Puts money into the account
        public void Deposit(double amount)
        {
            Balance += amount;
        }

        // Removes money from the account
        public void Withdraw(double amount)
        {
            Balance -= amount;
        }

        // Changes the account name
        public void SetName(string name)
        {
            Name = name;
        }

        // Returns the current balance
        public double CheckBalance()
        {
            return Balance;
        }

        // Returns the account number
        public int GetNumber()
        {
            return Number;
        }

        // Returns a string of all account information
        public virtual string ToPrint()
        {
            return $"{Number}-\"{Name}\":${Balance}";
        }
    }
}
