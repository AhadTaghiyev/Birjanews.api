using System;
using System.Collections.Generic;

namespace Birjanews.Api.Models;

public partial class Role
{
    public ulong Id { get; set; }

    public string Name { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}
