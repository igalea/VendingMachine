using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VendingMachine.Implementations;
using VendingMachine.UnitTest.Helpers;

namespace VendingMachine.UnitTest.CreditSuisse.UnitTests
{
    [TestClass]
    public class CardsAvailableProvidorUnitTest
    {
        CardsAvailableProvidor _cardsAvailableProvidor = new CardsAvailableProvidor();

        [TestMethod]
        public void AccountService_Constructor_5CardsAreAvailable()
        {
            Assert.AreEqual(5, _cardsAvailableProvidor.CardsAvailable.Count);
        }

        [TestMethod]
        public void AccountService_Constructor_AllCardsAreDifferent()
        {
            Assert.AreEqual( _cardsAvailableProvidor.CardsAvailable.Count,_cardsAvailableProvidor.CardsAvailable.Distinct().Count());
        }

        //Convert CardsAvailableProvidor to a List and make sure that adding cards to the list
        //does not affect the underlying CardsAvailableProvidor in accountservice (i.e. it is readonly
        //and the only way to add and remove cards is via thread safe methods supplied
        [TestMethod]
        public void AccountService_CardsAvailable_Add_CheckIsReadOnly()
        {
            var cardsAvailableAsList = _cardsAvailableProvidor.CardsAvailable.ToList();
            cardsAvailableAsList.Add("newCard");
            var readonlyCardsAvailable=_cardsAvailableProvidor.CardsAvailable;
            Assert.AreNotEqual(readonlyCardsAvailable.Count, cardsAvailableAsList.Count);
        }

        [TestMethod]
        public void AccountService_CardsAvailable_Remove_CheckIsReadOnly()
        {
            var cardsAvailableAsList = _cardsAvailableProvidor.CardsAvailable.ToList();
            cardsAvailableAsList.Remove(cardsAvailableAsList.First());
            var readonlyCardsAvailable= _cardsAvailableProvidor.CardsAvailable;
            Assert.AreNotEqual(readonlyCardsAvailable.Count, cardsAvailableAsList.Count);
        }

        [TestMethod]
        public void AccountService_CardsAvailableContains_Returns_TRUE_IfCardIsInList()
        {
            Assert.IsTrue( _cardsAvailableProvidor.CardsAvailableContains("6364-5345-7534-7965"));
        }

        [TestMethod]
        public void AccountService_CardsAvailableContains_Returns_FALSE_IfCardIs_NOT_InList()
        {
            Assert.IsFalse( _cardsAvailableProvidor.CardsAvailableContains("CardNotInList"));
        }

        [TestMethod]
        public void AccountService_AddCard_IncrementsAvailableCards()
        {
            var currentCount = _cardsAvailableProvidor.CardsAvailable.Count;
            _cardsAvailableProvidor.AddCard("8797980798");
            Assert.AreEqual(currentCount+1, _cardsAvailableProvidor.CardsAvailable.Count);
        }
        [TestMethod]
        public void AccountService_AddingDuplicateCard_ThrowsInvalidOperationException()
        {
            var currentCount = _cardsAvailableProvidor.CardsAvailable.Count;
            _cardsAvailableProvidor.AddCard("8797980798");
            Assert.AreEqual(currentCount+1, _cardsAvailableProvidor.CardsAvailable.Count);
        }


        [TestMethod]
        public void AccountService_RemoveCard_CardNotAvailableToRemove_ExceptionThrown()
        {
            AssertException.ExceptionThrown<InvalidOperationException>
                (
                ()=>_cardsAvailableProvidor.RemoveCard("8797980798"))
            ;
        }

        [TestMethod]
        public void AccountService_RemoveCard_CardAvailableToRemove_AfterRemoval_CountDecrements()
        {
            var cardsAvailableCountBefore = _cardsAvailableProvidor.CardsAvailable.Count;
            _cardsAvailableProvidor.RemoveCard("6364-5345-7534-7965");
            Assert.AreEqual(cardsAvailableCountBefore-1, _cardsAvailableProvidor.CardsAvailable.Count);
        }

         [TestMethod]
        public void AccountService_RemoveCard_CardNotAvailableToRemove_Count_DOES_NOT_Decrement()
        {
            var cardsAvailableCountBefore = _cardsAvailableProvidor.CardsAvailable.Count;
            try
            {
                _cardsAvailableProvidor.RemoveCard("CardNotInList");
            }
            catch (InvalidOperationException)
            {
                //Expecting this so do nothing
            }
            Assert.AreEqual(cardsAvailableCountBefore, _cardsAvailableProvidor.CardsAvailable.Count);
        }
    }
}
