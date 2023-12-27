using Domain;

namespace WebApp.dto;

public class ApartmentDTO  : BaseEntity
{
    public Guid PropertyId { get; set; }
    public PropertyDTO? Property { get; set; }
    public int FloorNumber { get; set; }
    public int RoomCount { get; set; }
    public decimal MonthlyRent { get; set; }
    public bool Status { get; set; }
    public Guid? CurrentLeaseId { get; set; }
    public LeaseDTO? CurrentLease { get; set; }
}