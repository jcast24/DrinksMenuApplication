using Spectre.Console;
using DrinksMenuApp.Services;
using DrinksMenuApp.Models;
namespace DrinksMenuApp.Visuals;

public class Tables
{
    public async Task CategoryTable()
    {
        var httpClient = new HttpClient();
        DrinkService ds = new DrinkService(httpClient);

        var categories = await ds.GetCategories();

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
