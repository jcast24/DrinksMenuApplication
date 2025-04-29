using System.Text.Json;
using DrinksMenuApp.Models;
using Spectre.Console;

namespace DrinksMenuApp.Services;

public class DrinksService : IDrinksService
{
    private readonly HttpClient _httpClient;

    public DrinksService()
    {
        _httpClient = new HttpClient();
    }

    public async Task<string> LookupDrinkById()
    {
        string drinkId = AnsiConsole.Ask<string>("ID of the drink?: ");
        string url = $"https://www.thecocktaildb.com/api/json/v1/1/lookup.php?i={drinkId}";

        try
        {
            var response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();

                // Deserialize the JSON
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var drinkResponse = JsonSerializer.Deserialize<DrinkResponse>(json, options);
                if (drinkResponse != null)
                {
                    foreach (var drink in drinkResponse.Drinks)
                    {
                        AnsiConsole.MarkupLine(
                            $"{drink.idDrink} {drink.strDrink} {drink.strCategory} {drink.strAlcoholic}"
                        );
                    }
                }
            }
        }
        catch (HttpRequestException e)
        {
            AnsiConsole.MarkupLine($"[red]Error: {e.Message} [/]");
        }
        return "Drink not found";
    }

    public async Task ShowDrinks()
    {
        string category = AnsiConsole.Ask<string>("What category?: ");
        string encodedCategory = Uri.EscapeDataString(category);

        var response = await GetDrinksByCategory(encodedCategory);

        if (response != null)
        {
            foreach (var drink in response)
            {
                AnsiConsole.MarkupLine($"{drink.idDrink} {drink.strDrink} {drink.strCategory}");
            }
        }
        else
        {
            AnsiConsole.MarkupLine("[red]Drink category not found.[/]");
        }
    }

    public async Task<List<Drink>> GetDrinksByCategory(string category)
    {
        string url = $"https://www.thecocktaildb.com/api/json/v1/1/filter.php?c={category}";

        try
        {
            var response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();

                // Deserialize the JSON
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

                var drinkResponse = JsonSerializer.Deserialize<DrinkResponse>(json, options);

                return drinkResponse?.Drinks ?? new List<Drink>();
            }
            else
            {
                AnsiConsole.WriteLine($"[red]No category found![/]");
                return new List<Drink>();
            }
        }
        catch (HttpRequestException e)
        {
            AnsiConsole.MarkupLine($"[red]Request Error: {e.Message}[/]");
            return new List<Drink>();
        }
    }
}
