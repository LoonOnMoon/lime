using Lime.Domain.Entities;

using Microsoft.AspNetCore.Identity;

namespace Lime.Infrastructure.Identity.Models
{
    public class LimeIdentityUser : IdentityUser
    {
        public required User User { get; set; }
    }
}