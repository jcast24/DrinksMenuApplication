using DrinksMenuApp.Services;
using DrinksMenuApp.Visuals;
using Spectre.Console;

namespace DrinksMenuApp;

class Program
{
    static async Task Main(string[] args)
    {
        // Start of the project
        Tables t = new Tables();
        t.CategoryTable();
        
        HttpClient hc = new HttpClient();

        string category = AnsiConsole.Ask<string>("Please enter a category: ");
        DrinkService ds = new DrinkService(hc, category);


        await ds.OutputAllDrinksFromCategoryType();
        await ds.OutputFullDrinkDetails();
    }
}
