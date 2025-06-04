using DrinksMenuApp.Services;
using Spectre.Console;

public class Menu
{
    public void CategoryTable()
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
        DrinksService ds = new DrinksService();
        CategoryTable();
        await ds.ShowDrinks(); 
    }
}
