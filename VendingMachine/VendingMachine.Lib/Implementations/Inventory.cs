using System;
using VendingMachine.Interfaces;

namespace VendingMachine.Implementations
{
    /// <summary>
    /// Keeps track of number of cans in inventory
    /// </summary>
    public class Inventory : IInventory
    {
        //Ideally configurable in config file or database table
        //but keeping in line with spec
        const int MAX_NUMBER_OF_CANS = 25;

        public Inventory(int numberOfCans)
        {
            
            if (numberOfCans<0||numberOfCans>MAX_NUMBER_OF_CANS)
            {
                throw new ArgumentOutOfRangeException(NumberOfCansOutOfRangeMessage(numberOfCans));
            }
            NumberOfCans = numberOfCans;
        }

        public int NumberOfCans
        {
            get; 
            private set;
        }

        public void Vend()
        {
            if (CanVend)
            {
                NumberOfCans--;
            }
        }

        public bool CanVend
        {
            get { return NumberOfCans > 0; }
        }

        #region helper method
        string NumberOfCansOutOfRangeMessage(int numberOfCans) 
        {
            return string.Format("'numberOfCans' parameter must be between 0 and {0} inclusive ({1} supplied)", MAX_NUMBER_OF_CANS,numberOfCans);
        }
        #endregion

    }
}
