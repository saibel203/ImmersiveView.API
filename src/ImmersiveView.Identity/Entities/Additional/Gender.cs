namespace ImmersiveView.Identity.Entities.Additional;

public class Gender
{
    public int GenderId { get; set; }
    public string GenderName { get; set; } = string.Empty;

    public ICollection<ApplicationUser> ApplicationUsers { get; set; } = new List<ApplicationUser>();
}