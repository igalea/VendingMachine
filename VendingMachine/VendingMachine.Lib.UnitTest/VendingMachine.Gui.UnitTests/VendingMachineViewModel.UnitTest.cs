using System;
using System.Linq;
using System.Security;
using System.Text;
using System.Collections.Generic;
using VendingMachine.Gui;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VendingMachine.Auxilliary;
using VendingMachine.Gui.ViewModels;
using VendingMachine.Implementations;
using VendingMachine.UnitTest.Helpers;

namespace VendingMachine.UnitTest.Gui.UnitTests
{
 
    [TestClass]
    public class VendingMachineViewModelUnitTest
    {
        VendingMachineViewModel _vendingMachineViewModel;

        //Note if in production would make accounts service IAccountsService
        //and use Moq particularly if dependency on external datasource
        AccountService _accountService;

        public VendingMachineViewModelUnitTest()
        {
            CardsAvailableProvidor cardsAvailableProvidor = new CardsAvailableProvidor();
            _accountService = new AccountService(cardsAvailableProvidor);
            _vendingMachineViewModel = new VendingMachineViewModel("1233-4265-5634-1234", _accountService);
        }


        [TestMethod]
        public void VendingMachineViewModel_OnInitialisation_BalancesIsBlank()
        {
            Assert.IsTrue(string.IsNullOrEmpty(_vendingMachineViewModel.Balance));
        }

        [TestMethod]
        public void VendingMachineViewModel_OnInitialisation_PINStatusIsBlank()
        {
            Assert.IsTrue(string.IsNullOrEmpty(_vendingMachineViewModel.PINStatus));
        }

        [TestMethod]//Must be correctly set so that main window knows which vending machine has been unloaded and can return card to the available listbox
        public void VendingMachineViewModel_OnInitialisation_SelectedCardHasBeenSet()
        {
            Assert.AreEqual(_vendingMachineViewModel.SelectedCard,"1233-4265-5634-1234");
        }

        [TestMethod]
        public void VendingMachineViewModel_OnInitialisation_PinStatusBlank()
        {
            Assert.IsTrue(string.IsNullOrEmpty(_vendingMachineViewModel.PINStatus));
        }

        [TestMethod]
        public void VendingMachineViewModel_ValidatePIN_NullPin_DoesNotThowExcpetion()
        {
             AssertException.NoExceptionThrown<Exception>(() => _vendingMachineViewModel.ValidatePIN(null));
        }
        [TestMethod]
        public void VendingMachineViewModel_ValidatePIN_NullPin_ReturnPINStatusAsIncorrectLength()
        {
            Assert.AreEqual(ePINStatus.IncorrectLength, _vendingMachineViewModel.ValidatePIN(null));
        }

        [TestMethod]
        public void VendingMachineViewModel_EmptyPin_ReturnPINStatusAsIncorrectLength()
        {
            Assert.AreEqual(ePINStatus.IncorrectLength,_vendingMachineViewModel.ValidatePIN(UnitTestHelper.FetchSecureString("")));
        }

        [TestMethod]//This is actually guarded for in the gui but best to check
        //this anyway in case the control is changed 
        public void VendingMachineViewModel_PinWithLetters_DoesNotThrowException()
        {
             AssertException.NoExceptionThrown<Exception>(() => _vendingMachineViewModel.ValidatePIN(UnitTestHelper.FetchSecureString("sdfs")));
        }

        [TestMethod]//This is guarded for in the gui but best to have this as a unit test
        // anyway in case the control is changed for one that doesn't
        public void VendingMachineViewModel_PinWithLetters_ReturnPINStatusAsIncorrect()
        {
                  Assert.AreEqual(ePINStatus.Incorrect,_vendingMachineViewModel.ValidatePIN(UnitTestHelper.FetchSecureString("sfss")));
        }

        [TestMethod]
        public void VendingMachineViewModel_ValidatePIN_PinLessThan4Characters_ReturnsCorrectStatus_IncorrectLeghth()
        {
            var secureString = UnitTestHelper.FetchSecureString("123");
            Assert.AreEqual(ePINStatus.IncorrectLength,_vendingMachineViewModel.ValidatePIN(secureString));
        }

        [TestMethod]
        public void VendingMachineViewModel_ValidatePIN_PinLessMore4Characters_ReturnsPINStatus_IncorrectLength()
        {
            var secureString = UnitTestHelper.FetchSecureString("12345");
            Assert.AreEqual(ePINStatus.IncorrectLength,_vendingMachineViewModel.ValidatePIN(secureString));
        }

        [TestMethod]
        public void VendingMachineViewModel_ValidatePIN_4CharactersButIncorrect_ReturnsPINStatus_Incorrect()
        {
            var secureString = UnitTestHelper.FetchSecureString("9999");
            Assert.AreEqual(ePINStatus.Incorrect,_vendingMachineViewModel.ValidatePIN(secureString));
        }
        
        [TestMethod]
        public void VendingMachineViewModel_ValidatePIN_CorrectPIN_ReturnsPINStatus_OK()
        {
            var secureString = UnitTestHelper.FetchSecureString("1234");
            Assert.AreEqual(ePINStatus.OK,_vendingMachineViewModel.ValidatePIN(secureString));
        }

        #region expected PINStatus Messages Shown
        [TestMethod]
        public void VendingMachineViewModel_IncorrectLengthPIN_PINStatusShowsExpectedMessage_PINMustBe4Digits()
        {
            var secureString = UnitTestHelper.FetchSecureString("123");
            _vendingMachineViewModel.ValidatePIN(secureString);
            Assert.AreEqual("PIN must be 4 digits",_vendingMachineViewModel.PINStatus);
        }

        [TestMethod]
        public void VendingMachineViewModel_IncorrectPIN_PINStatusShowsExpectedMessage_InvalidPIN()
        {
            var secureString = UnitTestHelper.FetchSecureString("9999");
            _vendingMachineViewModel.ValidatePIN(secureString);
            Assert.AreEqual("Invalid PIN",_vendingMachineViewModel.PINStatus);
        }

        [TestMethod]
        public void VendingMachineViewModel_CorrectPIN_PINStatusShowsExpectedMessage_OK()
        {
            var secureString = UnitTestHelper.FetchSecureString("1234");
            _vendingMachineViewModel.ValidatePIN(secureString);
            Assert.AreEqual("OK",_vendingMachineViewModel.PINStatus);
        }
        #endregion

         #region expected Balance Shown on PIN Entry
        [TestMethod]
        public void VendingMachineViewModel_IncorrectLengthPIN_BalanceHidden()
        {
            var secureString = UnitTestHelper.FetchSecureString("123");
            _vendingMachineViewModel.ValidatePIN(secureString);
            Assert.IsTrue(string.IsNullOrEmpty(_vendingMachineViewModel.Balance));
        }

        [TestMethod]
        public void VendingMachineViewModel_IncorrectPIN_BalanceHidden()
        {
            var secureString = UnitTestHelper.FetchSecureString("9999");
            _vendingMachineViewModel.ValidatePIN(secureString);
             Assert.IsTrue(string.IsNullOrEmpty(_vendingMachineViewModel.Balance));
        }

        [TestMethod]
        public void VendingMachineViewModel_CorrectPIN_BalanceShown()
        {
            var secureString = UnitTestHelper.FetchSecureString("1234");
            _vendingMachineViewModel.ValidatePIN(secureString);
            Assert.AreEqual("25",_vendingMachineViewModel.Balance);
        }
        #endregion

    }
}
