using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using VendingMachine.Auxilliary;
using VendingMachine.Gui.BaseClasses;
using VendingMachine.Interfaces;

namespace VendingMachine.Gui.ViewModels
{
    public class VendingMachineViewModel:NotifyPropertyChangedBase
    {
        const decimal COST_OF_CAN = 0.5m;
        readonly IAccountService _accountService;
        public VendingMachineViewModel(string cardNumber, IAccountService accountService)
        {
            if (accountService==null)
            {
                throw new ArgumentNullException("accountServer");
            }
            _accountService = accountService;
            _accountService.BalanceChanged += AccountServiceBalanceChanged; 
            SelectedCard = cardNumber;
            _pinStatus = "";
            _canVend = false;
        }

        void AccountServiceBalanceChanged(object sender, BalanceChangedEventArgs e)
        {
            Balance = e.Balance.ToString();
        }

        public string SelectedCard
        {
            get;
            set;
        }

        string _balance;
        public string Balance
        {
           get { return _balance; }
          set
          {
              _balance = value;
              CanVend = _accountService.CanSpend(COST_OF_CAN);
              NotifyPropertyChanged();
          }
        }
       
        string _pinStatus;
        public string PINStatus
        {
          get { return _pinStatus; }
          set
          {
              _pinStatus = value;
              NotifyPropertyChanged();
          }
        }
       
        bool _canVend;
        public bool CanVend
        {
          get { return _canVend; }
          set
          {
              _canVend = value;
              NotifyPropertyChanged();
          }
        }


        ICommand _enterPINCommand;
        public ICommand EnterPINCommand
        {
            get
            {
                 return _enterPINCommand ?? (_enterPINCommand = new CommandHandler(EnterPIN,true)); 
            }

        }

       
        internal void EnterPIN(object parameter)
        {
            if (parameter==null)
            {
                return;
            }
            var vendingMachineView = (Views.VendingMachine)parameter;
            ValidatePIN(vendingMachineView.Password);
        }

        public ePINStatus ValidatePIN(SecureString password)
        {
            var status = _accountService.ValidatePIN(SelectedCard, password);
            switch (status)
            {
                case ePINStatus.OK:
                    SetInitialBalance();
                    PINStatus = "OK";
                    return status;
                case ePINStatus.IncorrectLength:
                    PINStatus = "PIN must be 4 digits";
                    return status;
                case ePINStatus.Incorrect:
                    PINStatus = "Invalid PIN";
                    return status;
                default://In case have set up another PINStatus set up in AccountService but not handling it here...
                    throw new ArgumentOutOfRangeException("Unexpected PINStatus " + status.ToString());
            }
            
        }


        ICommand _vendCanCommand;
        public ICommand VendCanCommand
        {
            get
            {
                 return _vendCanCommand ?? (_vendCanCommand = new CommandHandler(param=>VendCan(),true)); 
            }
        }
        void VendCan()
        {
            _accountService.VendCan();
        }

        ICommand _removeCardCommand;
        public ICommand RemoveCardCommand
        {
            get
            {
                 return _removeCardCommand ?? (_removeCardCommand = new CommandHandler(RemoveCard,true)); 
            }
        }
        internal void RemoveCard(object parameter)
        {
            var vendingMachineView = (Views.VendingMachine)parameter;
            vendingMachineView.Close();
        }


        internal void SetInitialBalance()
        {
            Balance = _accountService.Balance.ToString();
        }



    }

}
