using DrinksMenuApp.Models;
using DrinksMenuApp.Services;
using Spectre.Console;

public class Menu
{
    public void MenuTable()
    {
        string colName = "Menu";

        string[] choices =
        {
            "Cocktail",
            "Ordinary Drink",
            "Punch / Party Drink",
            "Shake",
            "Other / Unknown",
            "Cocoa",
            "Shot",
            "Coffee/Tea",
            "Homemade Liqueur",
            "Beer",
            "Soft Drink",
        };

        var table = new Table();
        table.AddColumns(colName);

        foreach (var choice in choices)
        {
            table.AddRow(choice);
        }

        table.Border(TableBorder.Rounded);
        // table.Centered();

        AnsiConsole.Write(table);
    }

    public async Task ShowMenu()
    {
        AnsiConsole.MarkupLine("[cyan]Welcome to the Cantina![/]");
        MenuTable();

        string category = AnsiConsole.Ask<string>("What category?: ");
        string encodedCategory = Uri.EscapeDataString(category);

        DrinksService ds = new DrinksService();

        ApiResponse response = await ds.GetDrinksByCategory(encodedCategory);

        if (response != null)
        {
            foreach (var d in response.Drinks)
            {
                Console.WriteLine($"{d.idDrink} {d.strDrink} {d.strCategory}");
            }
        } 
        else 
        {
            AnsiConsole.MarkupLine("[red]Drink Category not found.[/]");
        }
    }
}
