namespace Domain;

public class Property : BaseEntity
{
    public string Address { get; set; }
    public string? PictureUrl { get; set; }
    
    public ICollection<Apartment>? Apartments { get; set; }
}