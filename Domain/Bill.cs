namespace Domain;

public class Bill : BaseEntity
{
    public Guid LeaseId { get; set; }
    public Lease? Lease { get; set; }
    public int Year { get; set; }
    public int Month { get; set; }
    public decimal FixedServicesTotal { get; set; }
    public decimal VariableServicesTotal { get; set; }
    public decimal RentAmount { get; set; }
    public decimal TotalAmount { get; set; }
    public bool Status { get; set; }
    
    // Navigation property to Usage
    public ICollection<ServiceConsumption>? ServiceConsumptions { get; set; }
}