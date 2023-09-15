using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Birjanews.Api.Contexts;
using Birjanews.Api.Dtos;
using Birjanews.Api.Entities;
using Birjanews.Api.Extentions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using static System.Net.Mime.MediaTypeNames;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Birjanews.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrganizerController : ControllerBase
    {
        private readonly BirjaDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public OrganizerController(BirjaDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        public async Task<IActionResult> GetOrganizers()
        {
           
            var Organizers = await _context.Organizers.Where(x => x.IsDeleted == false).ToListAsync();
            return Ok(Organizers);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrganizer(int id)
        {
            var organizer = await _context.Organizers.FirstOrDefaultAsync(x => x.IsDeleted == false && x.Id == id);
            if (organizer == null)
            {
                return NotFound();
            }
            return Ok(organizer);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateOrganizer([FromForm]OrganizerPostDto OrganizerDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (OrganizerDto.Image == null)
            {
                return StatusCode(400, new { description = "image file is required" });
            }
            var Organizer = new Organizer
            {
                Title = OrganizerDto.Title,
            };

            if (OrganizerDto.Image != null)
            {
                string image = OrganizerDto.Image.SaveImage(_webHostEnvironment.WebRootPath, "assets/images");
                var imagePath = Path.Combine("assets", "images", image);
                var imageUrl = $"{Request.Scheme}://{Request.Host}/{imagePath}";
                Organizer.Image = image;
                Organizer.ImageUrl = imageUrl;
            }
            _context.Organizers.Add(Organizer);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetOrganizer), new { id = Organizer.Id }, Organizer);
        }
        [HttpPut("{id}")]
        [Authorize]

        public async Task<IActionResult> UpdateOrganizer(int id, [FromForm]OrganizerPostDto OrganizerDto)
        {
            var Organizer = await _context.Organizers.FirstOrDefaultAsync(x => x.IsDeleted == false && x.Id == id);

            if (Organizer == null)
            {
                return NotFound();
            }

            Organizer.Title = OrganizerDto.Title;

            if (OrganizerDto.Image != null)
            {
                string image = OrganizerDto.Image.SaveImage(_webHostEnvironment.WebRootPath, "assets/images");
                var imagePath = Path.Combine("assets", "images", image);
                var imageUrl = $"{Request.Scheme}://{Request.Host}/{imagePath}";
                Organizer.Image = image;
                Organizer.ImageUrl = imageUrl;
            }
            _context.Entry(Organizer).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize]

        public async Task<IActionResult> DeleteOrganizer(int id)
        {
            var Organizer = await _context.Organizers.FirstOrDefaultAsync(x=>x.IsDeleted==false&&x.Id==id);
            if (Organizer == null)
            {
                return NotFound();
            }
            Organizer.IsDeleted = true;
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
