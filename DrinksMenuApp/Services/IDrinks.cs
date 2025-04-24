using DrinksMenuApp.Models;

namespace DrinksMenuApp.Services;

public interface IDrinksService
{
    Task<ApiResponse> GetDrinksByCategory(string drinkName);
}
