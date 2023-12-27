using Domain;

namespace WebApp.dto;

public class LeaseDTO  : BaseEntity
{
    public Guid ApartmentId { get; set; }
    public Guid AppUserId { get; set; }
    public AppUser? AppUser { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
    public decimal MonthlyRent { get; set; }
    public string ServicesIncluded { get; set; }
    
    public ICollection<LeaseService> LeaseServices { get; set; }
}