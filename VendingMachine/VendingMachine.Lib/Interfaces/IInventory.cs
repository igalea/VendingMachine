namespace VendingMachine.Interfaces
{
    interface IInventory
    {
        bool CanVend { get; }
        int NumberOfCans { get; }
        void Vend();
    }
}
