using Microsoft.AspNetCore.Identity;

namespace Pipeline.Empresa.Entities;

public class User : IdentityUser
{
    public User()
    {
        LastLogin = DateTime.UtcNow;
    }

    public DateTime? LastLogin { get; set; }

}
