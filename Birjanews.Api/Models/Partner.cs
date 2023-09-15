using System;
using System.Collections.Generic;

namespace Birjanews.Api.Models;

public partial class Partner
{
    public uint Id { get; set; }

    public string Name { get; set; } = null!;

    public string FileId { get; set; } = null!;

    public int Status { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}
