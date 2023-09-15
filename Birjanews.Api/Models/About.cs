using System;
using System.Collections.Generic;

namespace Birjanews.Api.Models;

public partial class About
{
    public ulong Id { get; set; }

    public string? TitleAz { get; set; }

    public string? TextAz { get; set; }

    public string? SeoTitleAz { get; set; }

    public string? SeoKeywordsAz { get; set; }

    public string? SeoDesctioptionAz { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}
