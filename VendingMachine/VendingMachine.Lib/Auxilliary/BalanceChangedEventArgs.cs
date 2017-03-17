using System;

namespace VendingMachine.Auxilliary
{
    public class BalanceChangedEventArgs:EventArgs
    {
        public BalanceChangedEventArgs (decimal newBalance)
        {
            Balance = newBalance;
        }

        public decimal Balance { get; private set; }
    }
}
