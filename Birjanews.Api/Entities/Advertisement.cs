using System;
namespace Birjanews.Api.Entities
{
	public class Advertisement:BaseEntity
	{
		public string Title { get; set; }
		public string DescriptionMini { get; set; }
		public string Description { get; set; }
		public string Image { get; set; }
        public string ImageUrl { get; set; }
		public bool Status { get; set; }
		public DateTime Date { get; set; }
		public int OrganizerId { get; set; }
		public Organizer Organizer { get; set; }
    }
}

