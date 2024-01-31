using ImmersiveView.Identity.Entities.Additional;
using Microsoft.AspNetCore.Identity;

namespace ImmersiveView.Identity.Entities;

public class ApplicationUser : IdentityUser
{
    public string ImagePath { get; set; } = string.Empty;

    public int? GenderId { get; set; }
    public Gender? Gender { get; set; }
}