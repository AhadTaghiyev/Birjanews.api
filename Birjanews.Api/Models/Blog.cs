using System;
using System.Collections.Generic;

namespace Birjanews.Api.Models;

public partial class Blog
{
    public ulong Id { get; set; }

    public int? PartnerId { get; set; }

    public string TitleAz { get; set; } = null!;

    public string TextAz { get; set; } = null!;

    public string ShortAz { get; set; } = null!;

    public string? SeoTitleAz { get; set; }

    public string? SeoKeywordsAz { get; set; }

    public string? SeoDesctioptionAz { get; set; }

    public DateTime CreatedDate { get; set; }

    public int View { get; set; }

    public int Status { get; set; }

    public int Type { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}
