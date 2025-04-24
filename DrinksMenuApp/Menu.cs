using DrinksMenuApp.Services;
using DrinksMenuApp.Models;
using Spectre.Console;

public class Menu
{
    public async Task ShowMenu()
    {
        // AnsiConsole.Markup("[cyan]Welcome to the Cantina![/]");

        string drink = AnsiConsole.Ask<string>("What drink?: ");
        DrinksService ds = new DrinksService();

        ApiResponse response = await ds.GetDrinksByCategory(drink);

        foreach(var d in response.Drinks)
        {
            Console.WriteLine($"{d.idDrink} {d.strDrink} {d.strCategory}");
        }

    }
}
