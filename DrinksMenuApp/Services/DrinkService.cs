using System.Text.Json;
using DrinksMenuApp.Models;
using Spectre.Console;

namespace DrinksMenuApp.Services;

public class DrinkService : IDrinksService
{
    private readonly HttpClient _httpClient;

    public DrinkService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    // Small test to see if the GetCategories method works
    // Take the data from GetCategories method, which returns a list
    // and output the data
    public async Task OutputDataFromGetCategories()
    {
        /* var httpClient = new HttpClient();

        var testClass = new DrinkService(httpClient); */

        var categories = await GetCategories();

        foreach (var category in categories)
        {
            Console.WriteLine(category);
        }
    }

    // Get the categories from the actual api and return it as a list
    public async Task<List<string>> GetCategories()
    {
        string url = "https://www.thecocktaildb.com/api/json/v1/1/list.php?c=list";

        using var response = await _httpClient.GetAsync(url);

        if (!response.IsSuccessStatusCode)
        {
            Console.WriteLine($"HTTP Error: {response.StatusCode}");
            return new List<string>();
        }

        var json = await response.Content.ReadAsStringAsync();
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        var categoryResponse = JsonSerializer.Deserialize<CategoryResponse>(json, options);

        return categoryResponse?.Drinks?.Select(c => c.StrCategory!).ToList() ?? new List<string>();
    }

    // Small test to output the drinks based off of the user choosing a specific category
    public async Task OutputAllDrinksFromCategoryType()
    {
        var drinks = await GetAllDrinksByCategory();

        foreach (var drink in drinks)
        {
            Console.WriteLine($"ID: {drink.idDrink}");
            Console.WriteLine($"Drink: {drink.strDrink}");
            Console.WriteLine("---------------------");
        }
    }

    // Ask user which category of drink they would like to see more about
    public async Task<List<Drink>> GetAllDrinksByCategory()
    {
        string category = AnsiConsole.Ask<string>(
            "Enter the category that you would like to see: "
        );
        var url = $"https://www.thecocktaildb.com/api/json/v1/1/filter.php?c={category}";

        var response = await _httpClient.GetAsync(url);

        if (!response.IsSuccessStatusCode)
        {
            Console.WriteLine($"HTTP Error: {response.StatusCode}");
            return new List<Drink>();
        }

        var json = await response.Content.ReadAsStringAsync();
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        var drinkResponse = JsonSerializer.Deserialize<DrinkResponse>(json, options);

        return drinkResponse?.Drinks ?? new List<Drink>();
    }
}
