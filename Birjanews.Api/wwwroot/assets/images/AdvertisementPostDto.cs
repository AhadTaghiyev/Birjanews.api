using System;
namespace Birjanews.Api.Dtos
{
	public class AdvertisementGetDto
    {
        public string Title { get; set; }
        public string DescriptionMini { get; set; }
        public string Description { get; set; }
        public IFormFile? Image { get; set; }
    }
}

