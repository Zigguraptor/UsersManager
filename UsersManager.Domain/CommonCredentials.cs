using System.ComponentModel.DataAnnotations;

namespace UsersManager.Domain;

public class CommonCredentials
{
    [Required] public DateTime CreationDateTime { get; set; } = DateTime.Now;
    [Required] public DateTime LastModDateTime { get; set; } = DateTime.Now;
}
