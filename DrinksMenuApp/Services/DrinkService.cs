using System.Text.Json;
using DrinksMenuApp.Models;
namespace DrinksMenuApp.Services;

public class DrinksService : IDrinksService
{
    private readonly HttpClient _httpClient;

    public DrinksService()
    {
        _httpClient = new HttpClient();
    }

    public async Task<ApiResponse> GetDrinksByCategory(string drinkName)
    {
        var response = await _httpClient.GetStringAsync(
            $"https://www.thecocktaildb.com/api/json/v1/1/search.php?s={drinkName}"
        );

        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

        var data = JsonSerializer.Deserialize<ApiResponse>(response, options);
        
        return data;
    }
}
