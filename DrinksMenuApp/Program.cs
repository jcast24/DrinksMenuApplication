using DrinksMenuApp.Services;
using DrinksMenuApp.Visuals;

namespace DrinksMenuApp;

class Program
{
    static async Task Main(string[] args)
    {
        // Start of the project
        Tables t = new Tables();
        await t.CategoryTable();
        
        HttpClient hc = new HttpClient();
        DrinkService ds = new DrinkService(hc);
        await ds.OutputAllDrinksFromCategoryType();
        await ds.OutputFullDrinkDetails();
    }
}
