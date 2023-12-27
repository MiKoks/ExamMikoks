using Domain;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.Models;

public class LeaseServiceViewModel
{
    public LeaseService LeaseServiceVmodel { get; set; } = default!;
    
    public SelectList? LeaseSelectList { get; set; }
    public SelectList? ServiceSelectList { get; set; }
}