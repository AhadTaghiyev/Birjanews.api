using System;
using System.Collections.Generic;

namespace Birjanews.Api.Models;

public partial class Contact
{
    public ulong Id { get; set; }

    public string? AddressAz { get; set; }

    public string? Telefon { get; set; }

    public string? Mobil { get; set; }

    public string? Email { get; set; }

    public string? Linkedin { get; set; }

    public string? Facebook { get; set; }

    public string? Instagram { get; set; }

    public string? Youtube { get; set; }

    public string? SeoTitleAz { get; set; }

    public string? SeoKeywordsAz { get; set; }

    public string? SeoDesctioptionAz { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}
