namespace DrinksMenuApp.Services;

public interface IDrinksService
{
    Task<List<string>> GetCategories();
    Task OutputDataFromGetCategories();
    Task OutputAllDrinksFromCategoryType();
    Task<string> VerifyCategory();
}
