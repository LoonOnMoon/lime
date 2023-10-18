using System.ComponentModel.DataAnnotations;

namespace Lime.Domain.Entities;

public class User
{
    [Key]
    public required string Id { get; set; }

    public required string Organization { get; set; }
}