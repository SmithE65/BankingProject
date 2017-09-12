using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingLibrary
{
    public class Savings : Account
    {
        double InterestRate = 0.05;

        public Savings() : base()
        {
            
        }

        public Savings(int number) : base(number)
        {
            
        }

        public void CompoundInterest()
        {
            Deposit(CheckBalance() * InterestRate);
        }

        public override string ToPrint()
        {
            return base.ToPrint() + $"::{InterestRate}%";
        }
    }
}
