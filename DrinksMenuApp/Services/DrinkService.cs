using System.Text.Json;
using DrinksMenuApp.Models;
using Spectre.Console;
using DrinksMenuApp.Visuals;
namespace DrinksMenuApp.Services;

public class DrinkService : IDrinksService
{
    private readonly HttpClient _httpClient;
    private readonly string _category;

    public DrinkService(HttpClient httpClient, string category)
    {
        _httpClient = httpClient;
        _category = category;
    }

    // Small test to see if the GetCategories method works
    // Take the data from GetCategories method, which returns a list
    // and output the data
    public async Task OutputDataFromGetCategories()
    {
        var categories = await GetCategories();

        foreach (var category in categories)
        {
            Console.WriteLine(category);
        }
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

    // Ask user which category of drink they would like to see more about
    public async Task<List<Drink>> GetAllDrinksByCategory()
    {
        var category = await VerifyCategory();

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
    
    // Show full details of the drink
    public async Task<List<Drink>> GetFullDrinkDetailsById()
    {
        // string id = AnsiConsole.Ask<string>("Enter id of drink: ");
        string id = await VerifyId();
        var url = $"https://www.thecocktaildb.com/api/json/v1/1/lookup.php?i={id}";

        var response = await _httpClient.GetAsync(url);

        if (!response.IsSuccessStatusCode)
        {
            Console.WriteLine($"Error fetching drink by id {id}");
            return new List<Drink>();
        }

        var json = await response.Content.ReadAsStringAsync();
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        var drinkResponse = JsonSerializer.Deserialize<DrinkResponse>(json, options);

        return drinkResponse?.Drinks!;
    }

    // Outputs the details of the drinks based on the id
    public async Task OutputFullDrinkDetails()
    {

        var drinks = await GetFullDrinkDetailsById();
        foreach (var drink in drinks)
        {
            Tables.DrinkDetails(drink.strDrink!, drink.strCategory!, drink.strAlcoholic!, drink.strInstructions!);
        }
    }
    
    // Verify the ID string for the individual drink
    public async Task<string> VerifyId()
    {
        var drinksList = await GetAllDrinksByCategory();

        List<string> newList = [];

        foreach (var drink in drinksList)
        {
            newList.Add(drink.idDrink!);
        }

        string chosenId;

        while (true)
        {
            chosenId = AnsiConsole.Ask<string>("Enter the id of a drink: ");
            if (newList.Contains(chosenId))
            {
                break;
            }
            AnsiConsole.MarkupLine($"[red]{chosenId} is not a valid id.[/]");
        }
        return chosenId;
    }

    // Verify the input from the user to see if they enter the right category. If not, ask again.
    public async Task<string> VerifyCategory()
    {
        List<string> validCategories = await GetCategories();

        while (true)
        {
            // chosenCategory = AnsiConsole.Ask<string>("Enter a drink category: ");
            if (validCategories.Contains(_category))
            {
                break;
            }

            AnsiConsole.MarkupLine($"[red]{_category} is not a valid category.[/]");
        }
        return _category;
    }
}
