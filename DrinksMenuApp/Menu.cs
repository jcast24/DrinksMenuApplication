using DrinksMenuApp.Services;
using DrinksMenuApp.Models;
using Spectre.Console;

public class Menu
{
    public async Task ShowMenu()
    {
        // AnsiConsole.Markup("[cyan]Welcome to the Cantina![/]");

        /* string drink = AnsiConsole.Ask<string>("What drink?: ");
        DrinksService ds = new DrinksService();

        ApiResponse response = await ds.GetDrinksByCategory(drink);

        foreach(var d in response.Drinks)
        {
            Console.WriteLine($"{d.idDrink} {d.strDrink} {d.strCategory}");
        } */

        string colName = "Menu";

        string[] choices = {"Ordinary Drinks", "Cocktail", "Milk / Float / Shake", "Other / Unknown", "Cocoa", "Shot", "Coffee / Tea", "Homemade Liqueur", "Punch / Party Drink", "Beer", "Soft Drink / Soda"};

        var table = new Table();
        table.AddColumns(colName);

        foreach(var choice in choices)
        {
            table.AddRow(choice);
        }

        table.Border(TableBorder.Rounded);
        table.Centered();

        AnsiConsole.Write(table);

        // var option = AnsiConsole.Prompt(
        //         new SelectionPrompt<string>().Title("What kind of drink are you looking for?").AddChoices(choices)
        //         );
        //
        // Console.WriteLine($"Your option: {option}");




    }
}
