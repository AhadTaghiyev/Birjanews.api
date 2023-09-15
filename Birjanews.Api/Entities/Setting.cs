using System;
namespace Birjanews.Api.Entities
{
	public class Setting:BaseEntity
	{
		public string Adress { get; set; }
		public string Phone { get; set; }
		public string Whatsapp { get; set; }
		public string Email { get; set; }
		public string Facebook { get; set; }
		public string Instagram { get; set; }
		public string Youtube { get; set; }
    }
}

