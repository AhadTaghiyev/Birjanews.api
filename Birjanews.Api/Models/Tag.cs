using System;
using System.Collections.Generic;

namespace Birjanews.Api.Models;

public partial class Tag
{
    public ulong Id { get; set; }

    public string Name { get; set; } = null!;

    public string ElanId { get; set; } = null!;

    public int Status { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}
