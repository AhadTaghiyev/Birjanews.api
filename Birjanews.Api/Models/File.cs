using System;
using System.Collections.Generic;

namespace Birjanews.Api.Models;

public partial class File
{
    public ulong Id { get; set; }

    public string File1 { get; set; } = null!;

    public string? NameAz { get; set; }

    public string Assign { get; set; } = null!;

    public int Status { get; set; }

    public int? PartnerId { get; set; }

    public int? ElanId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}
