namespace DrinksMenuApp;

class Program {
    static async Task Main(string[] args)
    {
        Menu menu = new Menu();
        await menu.ShowMenu();
    }
}
