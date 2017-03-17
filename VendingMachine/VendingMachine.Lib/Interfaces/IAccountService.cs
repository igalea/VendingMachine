using System;
using System.Security;
using System.Collections.Generic;
using VendingMachine.Auxilliary;

namespace VendingMachine.Interfaces
{
    public interface IAccountService
    {
        decimal Balance { get; }
        event EventHandler<BalanceChangedEventArgs> BalanceChanged;
        bool CanSpend(decimal amountToSpend);
        IReadOnlyCollection<string> CardsAvailable { get; }
        ICardsAvailableProvidor CardsAvailableProvidor { get; }
        bool PINCorrectLength(string pin);
        bool PinValid(string cardNumber, string pinNumber);
        ePINStatus ValidatePIN(string selectedCard, SecureString pinNumber);
        void VendCan();
    }
}
