using System.Collections.Generic;

namespace VendingMachine.Interfaces
{
    public interface ICardsAvailableProvidor
    {
        IReadOnlyCollection<string> CardsAvailable { get; }
        void AddCard(string cardNumber);
        void RemoveCard(string cardNumber);
        bool CardsAvailableContains(string cardNumber);
    }
}