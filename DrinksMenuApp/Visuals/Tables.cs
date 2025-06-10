using Spectre.Console;
namespace DrinksMenuApp.Visuals;

public class Tables
{
    public void CategoryTable()
    {
        string[] categories = {
            "Cocktail",
            "Ordinary Drink",
            "Punch / Party Drink",
            "Shake",
            "Other / Unknown",
            "Cocoa", 
            "Shot",
            "Coffee / Tea",
            "Homemade Liqueur",
            "Beer",
            "Soft Drink"
        };

        string colName = "Menu";

        var table = new Table();
        table.AddColumns(colName);

        foreach (var category in categories)
        {
            table.AddRow(category);
        }

        table.Border(TableBorder.Rounded);
        table.Centered();
        table.Title("[Cyan]Welcome to the Cantina![/]");
        AnsiConsole.Write(table);
    }
}
