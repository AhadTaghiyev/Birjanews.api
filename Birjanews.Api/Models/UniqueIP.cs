using System;
using System.Collections.Generic;

namespace Birjanews.Api.Models;

public partial class UniqueIP
{
    public ulong Id { get; set; }

    public int ElanId { get; set; }

    public string Ip { get; set; } = null!;

    public int View { get; set; }

    public int Status { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}
