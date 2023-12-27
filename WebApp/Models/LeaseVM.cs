using Domain;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.Models;

public class LeaseVM
{
    public Lease LeaseVmodel { get; set; } = default!;
    public SelectList? AppartmentList { get; set; }
    public SelectList? UserSelectList { get; set; }
}