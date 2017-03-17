using System;
using System.Collections.Generic;
using VendingMachine.Interfaces;

namespace VendingMachine.Implementations
{
    public class CardsAvailableProvidor : ICardsAvailableProvidor
    {

        public CardsAvailableProvidor()
        {
            InitializeCards();
        }

        internal void InitializeCards()
        {
            //In production would poplulate from a datasource
            AddCard("2435-3452-6657-8644");
            AddCard("6336-5678-3634-3636");
            AddCard("6364-5345-7534-7965");
            AddCard("4575-2344-5675-5674");
            AddCard("5326-5765-8678-6868");
        }

        #region Cards Available

        readonly List<string> _cardsAvailable = new List<string>();
        readonly object cardNumberLocker = new object();       
        //Make sure that adding and removing cards is done through methods here 
        //no direct write access to underlying data structure
        public IReadOnlyCollection <string> CardsAvailable
        {
            get
            {
                lock (cardNumberLocker)
                {
                    return _cardsAvailable.AsReadOnly();
                }
            }
        }

        //Should not give direct access to shared resource _cardsAvailable as a List
        public void AddCard(string cardNumber)
        {
            lock(cardNumberLocker)
            {
                if (_cardsAvailable.Contains(cardNumber))
                    throw new InvalidOperationException(string.Format("Card {0} already in _cardsAvailable", cardNumber));
                
                _cardsAvailable.Add(cardNumber);
            }
        }
        public void RemoveCard(string cardNumber)
        {
            lock(cardNumberLocker)
            {
                if (!_cardsAvailable.Contains(cardNumber))
                    throw new InvalidOperationException(string.Format("Card {0} cannot be removed as it's not in _cardsAvailable", cardNumber));
                _cardsAvailable.Remove(cardNumber);
            }
        }
        public bool CardsAvailableContains(string cardNumber)
        {
            lock(cardNumberLocker)
            {
                return _cardsAvailable.Contains(cardNumber);
            }
        }
        #endregion


    }
}
