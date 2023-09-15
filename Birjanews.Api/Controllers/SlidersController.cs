using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Birjanews.Api.Contexts;
using Birjanews.Api.Dtos;
using Birjanews.Api.Entities;
using Birjanews.Api.Extentions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static System.Net.Mime.MediaTypeNames;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Birjanews.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SlidersController : ControllerBase
    {
        private readonly BirjaDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public SlidersController(BirjaDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        public async Task<IActionResult> GetSliders()
        {
            var sliders = await _context.Sliders.ToListAsync();
            return Ok(sliders);
        }
        [HttpGet("{id}")]
        [Authorize]

        public async Task<IActionResult> GetSlider(int id)
        {
            var slider = await _context.Sliders.FindAsync(id);
            if (slider == null)
            {
                return NotFound();
            }
            return Ok(slider);
        }

        [HttpPost]
        [Authorize]

        public async Task<IActionResult> CreateSlider([FromForm]SliderPostDto sliderDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (sliderDto.Image == null)
            {
                return StatusCode(400, new { description = "image file is required" });
            }
            var slider = new Slider
            {
                Title = sliderDto.Title,
            };

            if (sliderDto.Image != null)
            {
                string image = sliderDto.Image.SaveImage(_webHostEnvironment.WebRootPath, "assets/images");
                var imagePath = Path.Combine("assets", "images", image);
                var imageUrl = $"{Request.Scheme}://{Request.Host}/{imagePath}";
                slider.Image = image;
                slider.ImageUrl = imageUrl;
            }
            _context.Sliders.Add(slider);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetSlider), new { id = slider.Id }, slider);
        }
        [HttpPut("{id}")]
        [Authorize]

        public async Task<IActionResult> UpdateSlider(int id, [FromForm]SliderPostDto sliderDto)
        {
            var slider = await _context.Sliders.FindAsync(id);

            if (slider == null)
            {
                return NotFound();
            }

            slider.Title = sliderDto.Title;

            if (sliderDto.Image != null)
            {
                string image = sliderDto.Image.SaveImage(_webHostEnvironment.WebRootPath, "assets/images");
                var imagePath = Path.Combine("assets", "images", image);
                var imageUrl = $"{Request.Scheme}://{Request.Host}/{imagePath}";
                slider.Image = image;
                slider.ImageUrl = imageUrl;
            }
            _context.Entry(slider).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize]

        public async Task<IActionResult> DeleteSlider(int id)
        {
            var slider = await _context.Sliders.FindAsync(id);
            if (slider == null)
            {
                return NotFound();
            }
            _context.Sliders.Remove(slider);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
