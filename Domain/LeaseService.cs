namespace Domain;

public class LeaseService
{
    public Guid LeaseId { get; set; }
    public Lease? Lease { get; set; }

    public Guid ServiceId { get; set; }
    public Service? Service { get; set; }
}