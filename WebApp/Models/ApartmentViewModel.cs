using Domain;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.Models;

public class ApartmentViewModel
{
    // Assuming these are properties of Domain.Apartment
    public Guid PropertyId { get; set; }
    public int FloorNumber { get; set; }
    public int RoomCount { get; set; }
    public decimal MonthlyRent { get; set; }
    public bool Status { get; set; } // Enum for Apartment status
    public Guid? CurrentLeaseId { get; set; } // Nullable if there can be no current lease

    // Additional properties for dropdown lists
    public IEnumerable<SelectListItem> Properties { get; set; }
    public IEnumerable<SelectListItem> CurrentLeases { get; set; }
    public IEnumerable<SelectListItem> StatusList { get; set; }
    
    public IEnumerable<SelectListItem> Statuses { get; set; }
    
    
}