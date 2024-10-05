using System;
using System.Collections.Generic;

namespace PinewoodTechnologies.Models;

public partial class Customer
{
    public int Id { get; set; }

    public string Forename { get; set; } = null!;

    public string Surname { get; set; } = null!;

    public string? Organisationname { get; set; }

    public string Contactnumber { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string House { get; set; } = null!;

    public string Street { get; set; } = null!;

    public string? Extra { get; set; }

    public string City { get; set; } = null!;

    public string Postcode { get; set; } = null!;
}
