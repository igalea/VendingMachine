using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VendingMachine.Auxilliary;
using VendingMachine.Gui.ViewModels;
using VendingMachine.UnitTest.Helpers;

namespace VendingMachine.UnitTest.Gui.UnitTests
{
    [TestClass]
    public class VendingMachineMainWindowUnitTest
    {
        private const decimal DEFAULT_BALANCE = 25;

        private MainWindowViewModel _mainWindowViewModel;

        [TestInitialize]
        public void Initialize()
        {
            _mainWindowViewModel = new MainWindowViewModel();
        }

        [TestMethod]
        public void MainWindowViewModel_Contructor_DoesNotThrowException()
        {
            AssertException.NoExceptionThrown<Exception>(() => new MainWindowViewModel());
        }

        [TestMethod]
        public void MainWindowViewModel_Contructor_MainWindowViewModelIsOPfExpectedType()
        {
            Assert.IsInstanceOfType(_mainWindowViewModel, typeof (MainWindowViewModel));
        }

        [TestMethod]
        public void MainWindowViewModel_Initialization_BalanceIsSetTo_DEFAULT_BALANCE()
        {
            Assert.AreEqual(DEFAULT_BALANCE.ToString(), _mainWindowViewModel.Balance);
        }

        [TestMethod]
        public void MainWindowViewModel_Initialization_RetrievedCardsAndPolulatedList_AvailableCardsGreaterThanZero()
        {

            Assert.IsTrue(_mainWindowViewModel.AvailableCards.Count > 0);
        }

        [TestMethod]
        public void MainWindowViewModel_LaunchVendingWithNoCardSelected_AvailableCardsCount_StaysTheSame()
        {
            var initialCount = _mainWindowViewModel.AvailableCards.Count;
            _mainWindowViewModel.LaunchVendingMachine("");
            Assert.AreEqual(initialCount, _mainWindowViewModel.AvailableCards.Count);
        }

        [TestMethod]
        public void MainWindowViewModel_LaunchVendingWithCardSelected_AvailableCardsCount_Decrements()
        {
            var initialCount = _mainWindowViewModel.AvailableCards.Count;
            _mainWindowViewModel.SelectedCard = _mainWindowViewModel.AvailableCards.First();
            _mainWindowViewModel.LaunchVendingMachine(_mainWindowViewModel.SelectedCard);
            Assert.AreEqual(initialCount - 1, _mainWindowViewModel.AvailableCards.Count);
        }

        [TestMethod]
        public void MainWindowViewModel_LaunchVendingWithCardSelected_WhenReplacingCard_AvailableCardsCountIncrements()
        {
            var initialCount = _mainWindowViewModel.AvailableCards.Count;
            _mainWindowViewModel.SelectedCard = _mainWindowViewModel.AvailableCards.First();
            _mainWindowViewModel.LaunchVendingMachine(_mainWindowViewModel.SelectedCard);

            //Now replace it
            _mainWindowViewModel.ReplaceSelectedCard(_mainWindowViewModel.SelectedCard);
            Assert.AreEqual(initialCount, _mainWindowViewModel.AvailableCards.Count);
        }

        #region test Blance Changed events
        bool balanceChangedEventFired;
        private decimal newBalance;
        [TestMethod]
        public void VendingMachineMainWindowViewModel_VendingACan_FiresBalanceChangedEventThatIsCaptured()
        {
            Assert.IsFalse(balanceChangedEventFired);
            _mainWindowViewModel._accountService.BalanceChanged += _accountService_BalanceChanged;
            _mainWindowViewModel._accountService.VendCan(); //will change the balance
            Assert.IsTrue(balanceChangedEventFired);
            _mainWindowViewModel._accountService.BalanceChanged -= _accountService_BalanceChanged;
        }

        [TestMethod]
        public void VendingMachineMainWindowViewModel_VendingACan_FiresBalanceChangedEventThatChangesTheBalance()
 {
            var initialBalance = _mainWindowViewModel.Balance;
            _mainWindowViewModel._accountService.BalanceChanged += _accountService_BalanceChanged;
            _mainWindowViewModel._accountService.VendCan(); //will change the balance
            Assert.IsTrue(initialBalance!=_mainWindowViewModel.Balance);
            _mainWindowViewModel._accountService.BalanceChanged -= _accountService_BalanceChanged;
        }

        private void _accountService_BalanceChanged(object sender, Auxilliary.BalanceChangedEventArgs e)
        {
            balanceChangedEventFired = true;
            newBalance = e.Balance;
        }
        #endregion
    }
}
