using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace RouteTickrAPI.Entities;

[Index(nameof(Username), IsUnique = true)]
public class User
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required] 
    [MaxLength(100)] 
    public string Username { get; set; } = "";

    [Required]
    [MaxLength(100)]
    public string PasswordHash { get; set; } = "";

    [Required]
    [MaxLength(25)]
    public string Role { get; set; } = "";
}