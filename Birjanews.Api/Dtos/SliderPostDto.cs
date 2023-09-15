using System;
namespace Birjanews.Api.Dtos
{
	public class SliderPostDto
	{
        public string Title { get; set; }
        public IFormFile? Image { get; set; }
    }
}

