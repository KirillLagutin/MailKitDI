namespace OnlineStore.Models;

public class Phone
{
    public int Id { get; set; }

    public string Brand { get; set; }

    public string Model { get; set; }
    
    public DateTime? ReleaseDate { get; set; }

    public decimal Price { get; set; }
}