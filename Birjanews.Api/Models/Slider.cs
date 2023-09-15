using System;
using System.Collections.Generic;

namespace Birjanews.Api.Models;

public partial class Slider
{
    public ulong Id { get; set; }

    public int FileId { get; set; }

    public string? TitleAz { get; set; }

    public string Status { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}
