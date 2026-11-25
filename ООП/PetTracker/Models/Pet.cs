using System.ComponentModel.DataAnnotations;

namespace PetTracker.Models;

public class Pet
{
    public int Id { get; set; }

    [Required, MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? PhotoUrl { get; set; }

    public int Age { get; set; }

    [MaxLength(500)]
    public string? Diet { get; set; }

    [MaxLength(500)]
    public string? Allergies { get; set; }

    [MaxLength(500)]
    public string? Diseases { get; set; }

    [MaxLength(2000)]
    public string? GeneralInfo { get; set; }
}