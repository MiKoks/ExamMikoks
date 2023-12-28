using Domain;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.Models;

public class ApartmentViewModel
{
    public Apartment ApartmentVmodel { get; set; } = default!;
    public SelectList? LeaseSelectList { get; set; }
    public SelectList? PropertySelectList { get; set; }
    
}