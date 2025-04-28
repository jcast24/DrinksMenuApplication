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

    public async Task<ApiResponse> GetDrinksByCategory(string category)
    {
        try
        {
            var response = await _httpClient.GetStringAsync(
                $"https://www.thecocktaildb.com/api/json/v1/1/filter.php?c={category}"
            );

            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            
            var data = JsonSerializer.Deserialize<ApiResponse>(response, options) ?? new ApiResponse();

            return data;

        }
        catch (HttpRequestException e)
        {
            AnsiConsole.MarkupLine($"[red]Request Error: {e.Message}[/]");
        }
        
        return new ApiResponse();
    }
}
