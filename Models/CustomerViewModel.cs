using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CW2.DAL.Entities;

public class CustomerViewModel
{
    [Key]
    public int CustomerId { get; set; } 

    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string? Contact { get; set; }
    public string? Email { get; set; }
    public string? Street { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? PostalCode { get; set; }

    // Remove navigation properties from ViewModel (optional but recommended)
    // These belong in the Entity, not the ViewModel
    // public virtual CustomerContact? CustomerContact { get; set; }
    // public virtual ICollection<Promotion> Promotions { get; set; } = new List<Promotion>();
    // public virtual ICollection<RentalRecord> RentalRecords { get; set; } = new List<RentalRecord>();
}