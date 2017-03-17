using System.Security;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using VendingMachine.Gui.Interfaces;
using VendingMachine.Gui.ViewModels;
using VendingMachine.Interfaces;

namespace VendingMachine.Gui.Views
{
    /// <summary>
    /// Interaction logic for VendingMachine.xaml
    /// </summary>
    public partial class VendingMachine : Window,IPassword,ISelectedCard
    {
        IAccountService _accountService;

        public VendingMachine()
        {
            InitializeComponent();

        }

        public VendingMachine(string cardNumber, IAccountService accountService)
        {
            InitializeComponent();
            DataContext = new VendingMachineViewModel (cardNumber,accountService);
            SelectedCard = cardNumber;
        }

        //Note - this is specifically adds behaviour to the user control so left in code behind
        //(in the absence of a password box that only allow numbers)
        void pwdPIN_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            //if (e.Text=="\r")
            //{
            //    int i = 1;
            //}
            Regex regex = new Regex("[0-9]+");
            if (!regex.IsMatch(e.Text))
            {
                e.Handled = true;
            }
        }

        public SecureString Password
        {
            get 
            { 
                return pwdPIN.SecurePassword; 
            } 
        }

        public string SelectedCard { get; private set; }

    
    }
}
