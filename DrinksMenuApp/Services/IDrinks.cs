using DrinksMenuApp.Models;

namespace DrinksMenuApp.Services;

public interface IDrinksService
{
    Task<List<Drink>> GetDrinksByCategory(string category);
    Task ShowDrinks();
    Task<string> LookupDrinkById();
}
