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
    public class NewsController : ControllerBase
    {
        private readonly BirjaDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
     

        public NewsController(BirjaDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(int pageNumber = 1, int pageSize = 10)
        {
            if (pageNumber < 1 || pageSize <= 0)
            {
                return BadRequest("Invalid pagination parameters.");
            }

            var News = await _context.News
                .Where(x => x.IsDeleted == false)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            // Additionally, you can also return the total count for client side logic (like pagination controls)
            var totalCount = await _context.News.CountAsync(x => x.IsDeleted == false);

            var result = new
            {
                TotalCount = totalCount,
                News = News
            };

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var News = await _context.News.FirstOrDefaultAsync(x => x.IsDeleted == false && x.Id == id);
            if (News == null)
            {
                return NotFound();
            }
            return Ok(News);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create([FromForm] AdvertisementPostDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (dto.Organizer == 0)
            {
                return BadRequest("Organizer id can not be 0");
            }
          
            var News = new News
            {
                Title = dto.Title,
                Date=dto.Date,
                Status=dto.Status,
                Description=dto.Description,
                DescriptionMini=dto.DescriptionMini,
                ImageUrl= $"{Request.Scheme}://{Request.Host}/Default.jpeg",
                Image= "Default.jpeg",
                OrganizerId=dto.Organizer,
            };
            if (dto.Image != null)
            {
                string image = dto.Image.SaveImage(_webHostEnvironment.WebRootPath, "assets/images");
                var imagePath = Path.Combine("assets", "images", image);
                var imageUrl = $"{Request.Scheme}://{Request.Host}/{imagePath}";
                News.Image = image;
                News.ImageUrl = imageUrl;
            }
         
            try
            {
               await _context.News.AddAsync(News);
                await _context.SaveChangesAsync();

            }
            catch (Exception ex)
            {

            }
            return CreatedAtAction(nameof(Get), new { id = News.Id }, News);
        }
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> Update(int id, [FromForm] AdvertisementPostDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingNews = await _context.News.FirstOrDefaultAsync(x=>x.Id==id&&x.IsDeleted==false);

            if (existingNews == null)
            {
                return NotFound("Advertisement not found.");
            }

            existingNews.Title = dto.Title;
            existingNews.Date = dto.Date;
            existingNews.Status = dto.Status;
            existingNews.Description = dto.Description;
            existingNews.DescriptionMini = dto.DescriptionMini;

            // Only change the image if a new one is provided
            if (dto.Image != null)
            {
                string image = dto.Image.SaveImage(_webHostEnvironment.WebRootPath, "assets/images");
                var imagePath = Path.Combine("assets", "images", image);
                var imageUrl = $"{Request.Scheme}://{Request.Host}/{imagePath}";

                existingNews.Image = image;
                existingNews.ImageUrl = imageUrl;
            }

            existingNews.OrganizerId = dto.Organizer;

            _context.News.Update(existingNews);
            await _context.SaveChangesAsync();

            return Ok(existingNews);
        }

        [HttpDelete("{id}")]
        [Authorize]

        public async Task<IActionResult> Delete(int id)
        {
            var News = await _context.News.FirstOrDefaultAsync(x => x.IsDeleted == false && x.Id == id);
            if (News == null)
            {
                return NotFound();
            }
            News.IsDeleted = true;
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
