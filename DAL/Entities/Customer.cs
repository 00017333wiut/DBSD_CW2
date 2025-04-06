using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CW2.DAL.Entities;

public partial class Customer
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

    // Navigation properties (keep these in Entity only)
    public virtual CustomerContact? CustomerContact { get; set; }
    public virtual ICollection<Promotion> Promotions { get; set; } = new List<Promotion>();
    public virtual ICollection<RentalRecord> RentalRecords { get; set; } = new List<RentalRecord>();
}