namespace OnlineStore.Models;

public class Tab
{
    public string Brand { get; set; }

    public double Diagonal { get; set; }

    public decimal Price { get; set; }

    public Tab(string brand, double diagonal, decimal price)
    {
        Brand = brand;
        Diagonal = diagonal;
        Price = price;
    }
}