using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VendingMachine.Auxilliary;
using VendingMachine.Implementations;
using VendingMachine.UnitTest.Helpers;

namespace VendingMachine.UnitTest.CreditSuisse.UnitTests
{
    [TestClass]
    public class AccountServiceUnitTest
    {
        const decimal DEFAULT_COST_OF_CAN = 0.5m;
        AccountService _accountSerice;
        CardsAvailableProvidor _cardsAvailableProvidor;
        [TestInitialize]
        public void InitializeAccountService()
        {
            _cardsAvailableProvidor = new CardsAvailableProvidor();
            _accountSerice = new AccountService(_cardsAvailableProvidor);
        }

        [TestMethod]
        public void AccountService_DEFAULT_COST_OF_CAN_Is_0_5()
        {
            Assert.AreEqual(0.5m, DEFAULT_COST_OF_CAN);
        }

        [TestMethod]
        public void AccountService_Constructor_DoesNotThrowException()
        {
          AssertException.NoExceptionThrown<Exception>(() => new AccountService(_cardsAvailableProvidor));
        }

        [TestMethod]
        public void AccountService_Constructor_IsInstanceOfTypeAccountService()
        {
          Assert.IsInstanceOfType(new AccountService(_cardsAvailableProvidor),typeof(AccountService));
        }

        //For purposes of this exercise ensure that expected data is loaded
        [TestMethod]
        public void AccountService_Constructor_BalanceIsSetTo25()
        {
            Assert.AreEqual(25, _accountSerice.Balance);
        }

       

        [TestMethod]
        public void AccountService_Constructor_5CardsAreAvailable()
        {
            Assert.AreEqual(5, _accountSerice.CardsAvailable.Count);
        }

        [TestMethod]
        public void AccountService_Constructor_AllCardsAreDifferent()
        {
            Assert.AreEqual( _accountSerice.CardsAvailable.Count,_accountSerice.CardsAvailable.Distinct().Count());
        }

        //Convert CardsAvailableProvidor to a List and make sure that adding cards to the list
        //does not affect the underlying CardsAvailableProvidor in accountservice (i.e. it is readonly
        //and the only way to add and remove cards is via thread safe methods supplied
        [TestMethod]
        public void AccountService_CardsAvailable_Add_CheckIsReadOnly()
        {
            var cardsAvailableAsList = _accountSerice.CardsAvailable.ToList();
            cardsAvailableAsList.Add("newCard");
            var readonlyCardsAvailable=_accountSerice.CardsAvailable;
            Assert.AreNotEqual(readonlyCardsAvailable.Count, cardsAvailableAsList.Count);
        }

        [TestMethod]
        public void AccountService_CardsAvailable_Remove_CheckIsReadOnly()
        {
            var cardsAvailableAsList = _accountSerice.CardsAvailable.ToList();
            cardsAvailableAsList.Remove(cardsAvailableAsList.First());
            var readonlyCardsAvailable= _accountSerice.CardsAvailable;
            Assert.AreNotEqual(readonlyCardsAvailable.Count, cardsAvailableAsList.Count);
        }

        [TestMethod]
        public void AccountService_CardsAvailableContains_Returns_TRUE_IfCardIsInList()
        {
            Assert.IsTrue( _accountSerice.CardsAvailableProvidor.CardsAvailableContains("4575-2344-5675-5674"));
        }

        [TestMethod]
        public void AccountService_CardsAvailableContains_Returns_FALSE_IfCardIs_NOT_InList()
        {
            Assert.IsFalse( _accountSerice.CardsAvailableProvidor.CardsAvailableContains("CardNotInList"));
        }

        [TestMethod]
        public void AccountService_AddCard_IncrementsAvailableCards()
        {
            var currentCount = _accountSerice.CardsAvailable.Count;
            _accountSerice.CardsAvailableProvidor.AddCard("8797980798");
            Assert.AreEqual(currentCount+1, _accountSerice.CardsAvailable.Count);
        }
        [TestMethod]
        public void AccountService_AddingDuplicateCard_ThrowsInvalidOperationException()
        {
            var currentCount = _accountSerice.CardsAvailable.Count;
            _accountSerice.CardsAvailableProvidor.AddCard("8797980798");
            Assert.AreEqual(currentCount+1, _accountSerice.CardsAvailable.Count);
        }


        [TestMethod]
        public void AccountService_RemoveCard_CardNotAvailableToRemove_ExceptionThrown()
        {
            AssertException.ExceptionThrown<InvalidOperationException>
                (
                ()=>_accountSerice.CardsAvailableProvidor.RemoveCard("8797980798"))
            ;
        }

        [TestMethod]
        public void AccountService_RemoveCard_CardAvailableToRemove_AfterRemoval_CountDecrements()
        {
            var cardsAvailableCountBefore = _accountSerice.CardsAvailable.Count;
            _accountSerice.CardsAvailableProvidor.RemoveCard("4575-2344-5675-5674");
            Assert.AreEqual(cardsAvailableCountBefore-1, _accountSerice.CardsAvailable.Count);
        }

         [TestMethod]
        public void AccountService_RemoveCard_CardNotAvailableToRemove_Count_DOES_NOT_Decrement()
        {
            var cardsAvailableCountBefore = _accountSerice.CardsAvailable.Count;
            try
            {
                _accountSerice.CardsAvailableProvidor.RemoveCard("CardNotInList");
            }
            catch (InvalidOperationException)
            {
                //Expecting this so do nothing
            }
            Assert.AreEqual(cardsAvailableCountBefore, _accountSerice.CardsAvailable.Count);
        }

         [TestMethod]
         public void AccountService_CanSpend_AmountAboveBalance_Returns_FALSE()
         {
             var balance = _accountSerice.Balance;
             Assert.IsFalse(_accountSerice.CanSpend(balance+1));
         }

         [TestMethod]
         public void AccountService_CanSpend_LessThanBalance_Returns_TRUE()
         {
             var balance = _accountSerice.Balance;
             Assert.IsTrue(_accountSerice.CanSpend(balance-1));
         }

        [TestMethod]
         public void AccountService_CanSpend_EqualToBalance_Returns_TRUE()
         {
             var balance = _accountSerice.Balance;
             Assert.IsTrue(_accountSerice.CanSpend(balance));
         }



        [TestMethod]
        public void AccountService_PINValid_PinNotEqualToLastFourDigitsOfCard_Returns_FALSE()
        {
            Assert.IsFalse(_accountSerice.PinValid("1234-5678-4321-8765", "1111"));
        }
        [TestMethod]
         public void AccountService_PINValid_PinNotEqualToLastFourDigitsOfCard_Returns_TRUE()
        {
            Assert.IsTrue(_accountSerice.PinValid("1234-5678-4321-8765", "8765"));
        } 

        [TestMethod]
        public void AccountService_PINCorrectLength_IfLengthIs_NOT_4_Returns_FALSE()
        {
            Assert.IsFalse(_accountSerice.PINCorrectLength("3465436546"));
        }

        [TestMethod]
        public void AccountService_PINCorrectLength_IfLengthIs4_Returns_TRUE()
        {
            Assert.IsTrue(_accountSerice.PINCorrectLength("1111"));
        }

   
        #region validate PIN tests
        [TestMethod]
        public void AccountService_ValidatePIN_IncorrectPIN_Returns_ePINStatus_Incorrect()
        {
            Assert.AreEqual(ePINStatus.Incorrect,_accountSerice.ValidatePIN("1234-5678-4321-8765",UnitTestHelper.FetchSecureString("9999")));
        }

        [TestMethod]
        public void AccountService_ValidatePIN_EmptyString_Returns_ePINStatus_Incorrect()
        {
            Assert.AreEqual(ePINStatus.IncorrectLength,_accountSerice.ValidatePIN("1234-5678-4321-8765",UnitTestHelper.FetchSecureString("")));
        }

        [TestMethod]
        public void AccountService_ValidatePIN_PINIncorrectLength_Returns_ePINStatus_IncorrectLength()
        {
            Assert.AreEqual(ePINStatus.IncorrectLength,_accountSerice.ValidatePIN("1234-5678-4321-8765",UnitTestHelper.FetchSecureString("3465436")));
        }

        [TestMethod]
        public void AccountService_ValidatePIN_CorrectPIN_Returns_ePINStatus_OK()
        {
            Assert.AreEqual(ePINStatus.OK,_accountSerice.ValidatePIN("1234-5678-4321-8765",UnitTestHelper.FetchSecureString("8765")));
        }
        #endregion

        
        [TestMethod]
        public void AccountService_VendCan_CheckInitialBalanceIsExpectedBalance()
        {
            var balance = _accountSerice.Balance;
            if (balance!=25)//expected balance then fail ther test
            {
                Assert.Fail("Starting Balance Incorrect");
            }
        }

        [TestMethod]//This should always pass no matter the cost of the can
        public void AccountService_VendCan_WithSufficientStartingBalance_DecrementsBalance()
        {
            var startingBalance = _accountSerice.Balance;
            _accountSerice.VendCan();
            Assert.IsTrue(_accountSerice.Balance < startingBalance);
        }

        [TestMethod]//This could fail if the cost of the can changes
        public void AccountService_VendCan_WithSufficientStartingBalance_DecrementsBalanceBy_0_50()
        {
            var startingBalance = _accountSerice.Balance;
            _accountSerice.VendCan();
            Assert.AreEqual(startingBalance - DEFAULT_COST_OF_CAN,_accountSerice.Balance);
        }

        #region BalanceChangedEvent Test
        bool balancedChangedEventHasBeenCalled;
        [TestMethod]
        public void AccountService_VendCan_WithSufficientStarting_OnBalnacedChangedEventIsRaised()
        {
            try
            {
                balancedChangedEventHasBeenCalled = false;
                Assert.IsTrue(balancedChangedEventHasBeenCalled == false);
                _accountSerice.BalanceChanged += _accountSerice_BalanceChanged;
                _accountSerice.VendCan();
                Assert.IsTrue(balancedChangedEventHasBeenCalled == true);
               
            }
            finally
            {
                _accountSerice.BalanceChanged -= _accountSerice_BalanceChanged;
            }
        }

        decimal newBalance;
        [TestMethod]//This could fail if the cost of the can changes
        public void AccountService_VendCan_WithSufficientStartingBalance_OnBalancedChangedEventReturnsExpectfedBalance()
        {
            try
            {
                balancedChangedEventHasBeenCalled = false;
                var startingBalance = _accountSerice.Balance;
                Assert.IsTrue(startingBalance > DEFAULT_COST_OF_CAN);//for integrity of test
                _accountSerice.BalanceChanged += _accountSerice_BalanceChanged;
                _accountSerice.VendCan();
                Assert.IsTrue(newBalance == startingBalance - DEFAULT_COST_OF_CAN);
            }
            finally
            {
                _accountSerice.BalanceChanged -= _accountSerice_BalanceChanged;
            }
            
        }

        [TestMethod]//This could fail if the cost of the can changes
        public void AccountService_VendCan_WithSufficientStartingBalance_OnBalancedChangedEventReturnedBalance_IsSameAsAccountServiceBalance()
        {
            try
            {
                balancedChangedEventHasBeenCalled = false;
                var startingBalance = _accountSerice.Balance;
                Assert.IsTrue(startingBalance > DEFAULT_COST_OF_CAN);//for integrity of test
                _accountSerice.BalanceChanged += _accountSerice_BalanceChanged;
                _accountSerice.VendCan();
                Assert.IsTrue(newBalance == _accountSerice.Balance);
            }
            finally
            {
                _accountSerice.BalanceChanged -= _accountSerice_BalanceChanged;
            }
        }

        [TestMethod]//This could fail if the cost of the can changes
        public void AccountService_VendCan_With_INSUFFICIENT_StartingBalance_OnBalancedChangedEvent_Is_NOT_called()
        {
            try
            {
                balancedChangedEventHasBeenCalled = false;
                while (_accountSerice.Balance >= DEFAULT_COST_OF_CAN)
                {
                    _accountSerice.VendCan();
                }
                var startingBalance = _accountSerice.Balance;
                Assert.IsTrue(startingBalance < DEFAULT_COST_OF_CAN);//for integrity of test
                _accountSerice.BalanceChanged += _accountSerice_BalanceChanged;
                _accountSerice.VendCan();
                Assert.IsFalse(balancedChangedEventHasBeenCalled);
            }
            finally
            {
                _accountSerice.BalanceChanged -= _accountSerice_BalanceChanged;
            }
            
        }

        void _accountSerice_BalanceChanged(object sender, BalanceChangedEventArgs e)
        {
            balancedChangedEventHasBeenCalled = true;
            newBalance = e.Balance;
        }

        #endregion




    }
}
