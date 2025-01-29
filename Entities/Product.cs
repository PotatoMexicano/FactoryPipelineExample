namespace Pipeline.Empresa.Entities;

public class Product
{
    public Int32 Id { get; set; }
    public String? Description { get; set; }
    public Int32 Price { get; set; }
    public Int32 Quantity { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

public class ProductDTO
{
    public Int32? Id { get; set; }
    public String? Description { get; set; }
    public Double? Price { get; set; }
    public Int32? Quantity { get; set; }
    public DateTime? CreatedAt { get; set; }
    public Double? PriceTotal { get; set; }
}

public sealed class RegisterProductDTO(String description, Int32 price, Int32 quantity)
{
    public String? Description { get; set; } = description;
    public Int32 Price { get; set; } = price;
    public Int32 Quantity { get; set; } = quantity;
    public DateTime CreatedAt { get; } = DateTime.UtcNow;
}
