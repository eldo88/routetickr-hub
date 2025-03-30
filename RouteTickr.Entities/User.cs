using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace RouteTickr.Entities;

public class User : IdentityUser
{
    public virtual ICollection<Tick> Ticks { get; set; } = new List<Tick>();
}