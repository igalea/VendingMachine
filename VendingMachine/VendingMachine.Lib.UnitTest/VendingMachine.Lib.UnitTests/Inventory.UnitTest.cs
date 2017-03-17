using System;
using VendingMachine.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VendingMachine.Implementations;
using VendingMachine.UnitTest.Helpers;

namespace VendingMachine.UnitTest.CreditSuisse.UnitTests
{
    [TestClass]
    public class Inventory_Test
    {

              
        [TestMethod]
        public void Inventory_Constructor_25CansAsParameter_DoesNotThrowException()
        {
           AssertException.NoExceptionThrown<Exception>(() => new Inventory(25));
        }

        [TestMethod]
        public void Inventory_Constructor_25CansAsParameter_IsInstanceOfTypeInventory()
        {
          Assert.IsInstanceOfType(new Inventory(25),typeof(Inventory));
        }

        //Would parameterize in NUnit
        [TestMethod]
        public void Inventory_Constructor_0CansAsParameter_DoesNotThrowException()
        {
            AssertException.NoExceptionThrown<Exception>(() => new Inventory(0));
        }
        [TestMethod]
        public void Inventory_Constructor_NegativeCansAsParameter_ThrowsException()
        {
            AssertException.ExceptionThrown<ArgumentOutOfRangeException>(() => new Inventory(-5));
        }
        [TestMethod]
        public void Inventory_Constructor_26CansAsParameter_ThrowsArgumentOutOfRangeException()
        {
           AssertException.ExceptionThrown<ArgumentOutOfRangeException>(() => new Inventory(26));
        }

        [TestMethod]
        public void Inventory_Constructor_25CansAsParameter_NumberOfCansEqualTo25()
        {
           Inventory inventory = new Inventory(25);
            Assert.AreEqual(25, inventory.NumberOfCans);
        }

        [TestMethod]
        public void Inventory_Constructor_25CansAsParameter_ReturnsCanVendIsTrue()
        {
           Inventory inventory = new Inventory(25);
           Assert.IsTrue(inventory.CanVend);
        }

    

        [TestMethod]
        public void Inventory_25CansAsParameter_VendMethod_DecrementsNumberOfCansBy1()
        {
           Inventory inventory = new Inventory(25);
           Assert.AreEqual(25, inventory.NumberOfCans); //duplicate of test above but making we start off witht he correct number before vending
           inventory.Vend();
           Assert.AreEqual(24, inventory.NumberOfCans);
        }

        [TestMethod]
        public void Inventory_VendWithZeroCans_NumberOfCansDoesNotBecomeNegative()
        {
           Inventory inventory = new Inventory(0);
           Assert.AreEqual(0, inventory.NumberOfCans); //duplicate of test above but making we start off witht he correct number before vending
           inventory.Vend();
           Assert.AreEqual(0, inventory.NumberOfCans);
        }


    }
}
