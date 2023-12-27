using Domain;

namespace WebApp.dto;

public class PropertyDTO : BaseEntity
{
    public string Address { get; set; }
    public string? PictureUrl { get; set; }
    
}