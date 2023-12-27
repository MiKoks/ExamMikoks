namespace Domain;

public class ServiceConsumption : BaseEntity
{
    public Guid LeaseId { get; set; }
    public Lease? Lease { get; set; }
    public Guid ServiceId { get; set; }
    public Service? Service { get; set; }
    public decimal QuantityUsed { get; set; }
    public decimal Rate { get; set; }
    public decimal TotalCost { get; set; }
}