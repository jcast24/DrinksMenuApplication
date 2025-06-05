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

    public async Task ShowDrinks()
    {
        List<Drink> category = await PromptUserForValidCategory();

        if (category.Count > 0)
        {
            foreach (var drink in category)
            {
                AnsiConsole.MarkupLine($"{drink.idDrink} {drink.strDrink} {drink.strCategory}");
            }

            await LookupDrinkById();
        }

        /* string category = AnsiConsole.Ask<string>("What category?: ");

        string encodedCategory = Uri.EscapeDataString(category);

        var response = await GetDrinksByCategory(encodedCategory);

        if (response.Count > 0)
        {
            foreach (var drink in response)
            {
                AnsiConsole.MarkupLine($"{drink.idDrink} {drink.strDrink} {drink.strCategory}");
            }

            await LookupDrinkById();
        } */
    }

    public async Task LookupDrinkById()
    {
        string drinkId = AnsiConsole.Ask<string>("ID of the drink?: ");

        if (string.IsNullOrWhiteSpace(drinkId))
        {
            AnsiConsole.MarkupLine("[yellow]Please enter a valid ID.[/]");
            return;
        }

        string url = $"https://www.thecocktaildb.com/api/json/v1/1/lookup.php?i={drinkId}";

        try
        {
            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                AnsiConsole.MarkupLine($"[red]API request failed: {response.StatusCode}[/]");
                return;
            }

            string json = await response.Content.ReadAsStringAsync();

            // Deserialize the JSON
            var drinkResponse = JsonSerializer.Deserialize<DrinkResponse>(
                json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
            );

            if (drinkResponse?.Drinks == null || drinkResponse.Drinks.Count == 0)
            {
                AnsiConsole.MarkupLine("[yellow]No Drink found with that ID.[/]");
                return;
            }

            foreach (var drink in drinkResponse.Drinks)
            {
                AnsiConsole.WriteLine(
                    $"{drink.idDrink} {drink.strDrink} {drink.strCategory} {drink.strAlcoholic}"
                );
            }
        }
        catch (HttpRequestException e)
        {
            AnsiConsole.MarkupLine($"[red]Error: {e.Message} [/]");
        }
        catch (JsonException e)
        {
            AnsiConsole.MarkupLine($"[red]JSON parsing error: {e.Message}[/]");
        }
        catch (Exception e)
        {
            AnsiConsole.MarkupLine($"[red]Unexpected error: {e.Message}[/]");
        }
    }

    public async Task<List<string>> GetValidCategoriesAsync()
    {
        string url = "https://www.thecocktaildb.com/api/json/v1/1/list.php?c=list";

        try
        {
            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"HTTP Error: {response.StatusCode}");
                return new List<string>();
            }

            var json = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var categoryResponse = JsonSerializer.Deserialize<CategoryResponse>(json, options);

            return categoryResponse?.Drinks?.Select(c => c.StrCategory).ToList()
                ?? new List<string>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error {ex.Message}");
            return new List<string>();
        }
    }

    public async Task<List<Drink>> PromptUserForValidCategory()
    {
        List<string> validCategories = await GetValidCategoriesAsync();

        if (validCategories.Count == 0)
        {
            AnsiConsole.MarkupLine(
                $"[red]Could not retrieve valid categories. Try again later. [/]"
            );
            return new List<Drink>();
        }

        string chosenCategory;

        while (true)
        {
            chosenCategory = AnsiConsole.Ask<string>("Enter a drink category: ");

            if (validCategories.Contains(chosenCategory, StringComparer.OrdinalIgnoreCase))
                break;

            AnsiConsole.MarkupLine(
                $"[red]{chosenCategory} is not a valid category. Please try again: [/]"
            );
        }

        return await GetDrinksByCategory(chosenCategory);
    }

    /* public async Task<List<Drink>> GetDrinksByCategory(string category)
    {
        string url = "https://www.thecocktaildb.com/api/json/v1/1/filter.php?c={category}";

        try {
            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"HTTP Error: {response.StatusCode}");
                return new List<Drink>();
            }

            var json = await response.Content.ReadAsStringAsync();

            var drinkResponse = JsonSerializer.Deserialize<DrinkResponse>(json, new JsonSerializerOptions {PropertyNameCaseInsensitive = true});

            return drinkResponse?.Drinks ?? new List<Drink>();
        } catch (Exception ex)
        {
            Console.WriteLine($"{ex.Message}");
            return new List<Drink>();
        }
    } */

    public async Task<List<Drink>> GetDrinksByCategory(string category)
    {
        /* var validCategories = await GetValidCategoriesAsync();

        if (!validCategories.Contains(category, StringComparer.OrdinalIgnoreCase))
        {
            AnsiConsole.MarkupLine($"[red]{category} is not a valid category.[/]");
            Console.WriteLine("Valid categories include: ");
            validCategories.ForEach(c => Console.WriteLine($"- {c}"));
            return new List<Drink>();
        } */

        var url = $"https://www.thecocktaildb.com/api/json/v1/1/filter.php?c={category}";

        try
        {
            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                AnsiConsole.MarkupLine($"[yellow]HTTP Error: {response.StatusCode}[/]");
                return new List<Drink>();
            }

            var json = await response.Content.ReadAsStringAsync();

            var drinkResponse = JsonSerializer.Deserialize<DrinkResponse>(
                json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
            );

            if (drinkResponse?.Drinks == null || !drinkResponse.Drinks.Any())
            {
                AnsiConsole.MarkupLine("[red]No drinks found for the given category.[/]");
                return new List<Drink>();
            }

            return drinkResponse.Drinks;
        }
        catch (HttpRequestException e)
        {
            AnsiConsole.MarkupLine($"[red]HTTP Request Error: {e.Message}[/]");
        }
        catch (JsonException)
        {
            AnsiConsole.MarkupLine($"[red]No category found.[/]");
        }

        return new List<Drink>();
    }
}
