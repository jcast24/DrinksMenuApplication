using DrinksMenuApp.Models;

namespace DrinksMenuApp.Services;

public interface IDrinksService
{
    Task<List<Drink>> GetDrinksByCategory(string category);
    Task<List<string>> GetValidCategoriesAsync();
    Task<List<Drink>> PromptUserForValidCategory();
    Task ShowDrinks();
    Task LookupDrinkById();
}
