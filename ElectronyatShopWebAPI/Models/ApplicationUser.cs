// ElectronyatShopElectronyatShopWebAPIApplicationUser.cs
// 202412720:46
// eladwyeladwy

using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace ElectronyatShopWebAPI.Models;

public class ApplicationUser : IdentityUser
{
    [Required]
    public string FirstName { get; set; }

    [Required]
    public string LastName { get; set; }

    public string FullName => $"{ FirstName } { LastName }";

    [Required]
    public string Address { get; set; }

    public ICollection<Order>? Orders { get; set; }
}