namespace Domain;

public class Service : BaseEntity
{
    public string Name { get; set; }
    public decimal? Price { get; set; }
    public bool IsFixed { get; set; }
    
    public ICollection<LeaseService>? LeaseServices { get; set; }
}