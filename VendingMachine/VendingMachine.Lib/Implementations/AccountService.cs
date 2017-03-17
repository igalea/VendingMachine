using System;
using System.Collections.Generic;
using System.Security;
using VendingMachine.Auxilliary;
using VendingMachine.Interfaces;

namespace VendingMachine.Implementations
{
    public class AccountService : IAccountService
    {
        //Hard-coded but would be configurable
        const decimal COST_OF_CAN = 0.50m;
        const int PIN_LENGTH = 4;
        const int STARTING_BALANCE = 25;


        readonly ICardsAvailableProvidor _cardsAvailableProvidor;
        public  AccountService(ICardsAvailableProvidor cardsAvailableProvidor)
        {
            if (cardsAvailableProvidor==null)
            {
                throw new ArgumentNullException();
            }
            _cardsAvailableProvidor = new CardsAvailableProvidor();
            
            //Doesn't make sense to allow externally setting the balance (except for testing hence internal setter below)
            //so for this project set the balance here - in reality this would call an external balance providor (e.g. database)
            Balance = STARTING_BALANCE;
        }

        public ICardsAvailableProvidor CardsAvailableProvidor
        {
            get { return _cardsAvailableProvidor; }
        }

        //Provide alias so preferably don't need to use above
        public IReadOnlyCollection<string> CardsAvailable
        {
            get { return _cardsAvailableProvidor.CardsAvailable; }
        }


        public bool CanSpend( decimal amountToSpend)
        {
            return (Balance >= amountToSpend);
        }

        //************************************************ 
        //For purposes of this test Using very simple PIN 
        //PIN EQUALS last 4 digits of card number, 
        //************************************************  
        //In production would have this stored in an encrypted data structure
        public bool PinValid(string cardNumber,string pinNumber)
        {   
            return cardNumber.Substring(cardNumber.Length - PIN_LENGTH, PIN_LENGTH) == pinNumber;
        }

        public bool PINCorrectLength(string pin)
        {
            return pin.Length == PIN_LENGTH;
        }

        public ePINStatus ValidatePIN(string selectedCard,SecureString pinNumber)
        {
            var pin = pinNumber.ConvertToUnsecureString();
            if (!PINCorrectLength(pin))
                return ePINStatus.IncorrectLength;

            if (PinValid(selectedCard, pin))  
                return ePINStatus.OK;
            
            return ePINStatus.Incorrect;
        }

        public void VendCan()
        {
            if (CanSpend(COST_OF_CAN))
            {
                Balance -= COST_OF_CAN;
                OnBalanceChanged(new BalanceChangedEventArgs(Balance));
            }
        }

       //potentially could have contention here so lock
        object balanceLocker = new object();
        decimal _balance;
        public decimal Balance 
        { 
            get
            {
                lock(balanceLocker)
                {
                    return _balance;
                }
            }
          internal set
          {
                lock(balanceLocker)
                {
                    _balance = value;
                }
          }
        }

        internal void FireBalanceChanged(decimal balance)
        {
            OnBalanceChanged(new BalanceChangedEventArgs(balance));
        }

        #region events that can be subscribed to
        public event EventHandler<BalanceChangedEventArgs> BalanceChanged;  
        internal void OnBalanceChanged(BalanceChangedEventArgs e)
            {
                // Make copy of handler in order to avoid race condition
                // should last subscriber unsubscribe between
                //  null check and the event being raised.
                EventHandler<BalanceChangedEventArgs> handler = BalanceChanged;

                // Null if no subscribers so don't raise the event if this is the case
                if (handler != null)
                {
                    //raise the event.
                    handler(this, e);
                }
            }
        #endregion
    }
}
