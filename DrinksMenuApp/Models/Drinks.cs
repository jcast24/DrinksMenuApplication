namespace DrinksMenuApp.Models;

public class ApiResponse
{
    public List<Drink> Drinks { get; set; }
}

public class Drink {
    public string idDrink { get; set; }
    public string strDrink { get; set; }
    public string strCategory { get; set; }
    public string strAlcoholic { get; set; }
}
