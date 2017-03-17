using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using VendingMachine.Gui.BaseClasses;
using VendingMachine.Implementations;
using VendingMachine.Interfaces;

namespace VendingMachine.Gui.ViewModels
{
    public class MainWindowViewModel:NotifyPropertyChangedBase
    {
        readonly ICardsAvailableProvidor _cardsAvailableProvidor = new CardsAvailableProvidor();
        internal readonly IAccountService _accountService;

        public MainWindowViewModel()
        {
            _accountService = new AccountService(_cardsAvailableProvidor);
            _accountService.BalanceChanged += _accountService_BalanceChanged;
            AvailableCards = new ObservableCollection<string>();
            Balance = _accountService.Balance.ToString();
            InitializeAvailableCards();
        }

        readonly object availableCardsLocker = new object();
        public ObservableCollection<string> _availableCards;
        public ObservableCollection<string> AvailableCards
        {
            get
            {
                lock (availableCardsLocker)
                {
                    if (_availableCards==null)
                    {
                        _availableCards = new ObservableCollection<string>();
                    }
                    return _availableCards;
                }
            }
            set
            {
                lock (availableCardsLocker)
                {
                    _availableCards = value;
                }
            }
        }

        public string SelectedCard
        { get; set; }

        public string _balance;
        public string Balance
        {
            get
            {
                return _balance;
            }
            set
            {
                _balance = value;
                NotifyPropertyChanged();
            }
        }

        ICommand _useCard;
        public ICommand UseCard
        {
            get { return _useCard ?? new CommandHandler(param=>LaunchVendingMachine(new object()), true); }
        }

        internal void LaunchVendingMachine(object parameter)
        {
            if (AvailableCards.Count==0)
            {
                //not going pop up a message box
                return;
            }

            if (string.IsNullOrEmpty(SelectedCard))
            {
                //wont pop up a message box
                return;
            }

            var vendingMachine = new Views.VendingMachine(SelectedCard,_accountService);
            vendingMachine.Unloaded += vendingMachine_Unloaded;
            vendingMachine.Show();
            AvailableCards.Remove(SelectedCard);
        }

        void vendingMachine_Unloaded(object sender, RoutedEventArgs e)
        {
            var vendingMachine = (Views.VendingMachine) sender;
            ReplaceSelectedCard(vendingMachine.SelectedCard);
            vendingMachine.Unloaded -= vendingMachine_Unloaded;
        }

        internal void ReplaceSelectedCard(string selectedCard)
        {
            AvailableCards.Add(selectedCard);
        }

        void InitializeAvailableCards()
        {
            _accountService.CardsAvailableProvidor.CardsAvailable.ToList().ForEach(AvailableCards.Add);
        }

        void _accountService_BalanceChanged(object sender, Auxilliary.BalanceChangedEventArgs e)
        {
            Balance = e.Balance.ToString();
        }


    }
}
