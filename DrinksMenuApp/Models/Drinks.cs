namespace DrinksMenuApp.Models;

public class DrinkResponse
{
    public List<Drink> Drinks { get; set; } = new List<Drink>();
}

public class Drink {
    public string idDrink { get; set; } = String.Empty;
    public string strDrink { get; set; } = String.Empty;
    public string strCategory { get; set; } = String.Empty;
    public string strAlcoholic { get; set; } = String.Empty;
}
