using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Domain;

public class AppUser : IdentityUser<Guid>
{

    [MaxLength(128)]
    [MinLength(1)]
    public string FirstName { get; set; } = default!;
    [MaxLength(128)]
    [MinLength(1)]
    public string LastName { get; set; } = default!;
    
    public string FullName => $"{FirstName} {LastName}";
    
    /*public ICollection<GroupMessages>? GroupMessagesCollection { get; set; }*/
    
}