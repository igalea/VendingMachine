Ian Galea 2017

The solution can be built without any external dependencies (deliberately) using .Net Framework 4.5
Usually I would use NUnit and Moq though in this solution I have used MSTest and have been run using Visual Studio TestExplorer
The unit tests are passing and mainly cover 'AccountService','Inventory','CardsAvailableProvidor' and both ViewModels in the WPF application.

Similiarly I have used native WPF to implement MVVM rather than dependency on third-party frameworks

I have commented on design decisions where applicable

'AccountService','Inventory','CardsAvailableProvidor' implement interfaces 'IAccountService','IInventory','ICardsAvailableProvidor' - facilitating use of Mocking Frameworks and
dependency injection.

Build and running the solution will launch the Gui.










