using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Birjanews.Api.Contexts;
using Birjanews.Api.Dtos;
using Birjanews.Api.Entities;
using Birjanews.Api.Extentions;
using Birjanews.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static System.Net.Mime.MediaTypeNames;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Birjanews.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdvertisementController : ControllerBase
    {
        private readonly BirjaDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public AdvertisementController(BirjaDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(int pageNumber = 1, int pageSize = 10,bool status=true)
        {
            if (pageNumber < 1 || pageSize <= 0)
            {
                return BadRequest("Invalid pagination parameters.");
            }

            var advertisements = await _context.Advertisements
                .OrderByDescending(x=>x.Id)
                .Where(x => x.IsDeleted == false&&x.Status==status)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            // Additionally, you can also return the total count for client side logic (like pagination controls)
            var totalCount = await _context.Advertisements.CountAsync(x => x.IsDeleted == false);

            var result = new
            {
                TotalCount = totalCount,
                Advertisements = advertisements
            };

            return Ok(result);
        }
        [HttpGet("client")]
        public async Task<IActionResult> GetAllClient(int pageNumber = 1, int pageSize = 10)
        {
            if (pageNumber < 1 || pageSize <= 0)
            {
                return BadRequest("Invalid pagination parameters.");
            }

            var advertisements = await _context.Advertisements
                .Where(x => x.IsDeleted == false&& x.Status == true)
                   .OrderByDescending(x => x.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
             
                .ToListAsync();

            // Additionally, you can also return the total count for client side logic (like pagination controls)
            var totalCount = await _context.Advertisements.CountAsync(x => x.IsDeleted == false&&x.Status==true);

            var result = new
            {
                TotalCount = totalCount,
                Advertisements = advertisements
            };

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var advertisement = await _context.Advertisements.FirstOrDefaultAsync(x => x.IsDeleted == false && x.Id == id);
            if (advertisement == null)
            {
                return NotFound();
            }
            return Ok(advertisement);
        }
        //[HttpGet("s")]
        //public async Task<IActionResult> Insert()
        //{
        //    U109051628BirjanewsazContext con = new();
        //    var adverts = await con.Blogs.ToListAsync();
        //    adverts.ForEach(x =>
        //    {
        //        if (x.PartnerId != null)
        //        {
        //            Advertisement advertisement = new Advertisement
        //            {
        //                Date = x.CreatedDate,
        //                Status = x.Status == 0,
        //                Description = x.TextAz,
        //                DescriptionMini = x.ShortAz,
        //                Image = "jhj",
        //                ImageUrl = "jhj",
        //                IsDeleted = false,
        //                OrganizerId = (int)x.PartnerId,
        //                Title = x.TitleAz,
        //            };
        //            _context.AddAsync(advertisement);
        //        }
              
        //    });
        //    await _context.SaveChangesAsync();
           
        //    return Ok();
        //}

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create([FromForm] AdvertisementPostDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

          
            var advertisement = new Advertisement
            {
                Title = dto.Title,
                Date=dto.Date,
                Status=dto.Status,
                Description=dto.Description,
                DescriptionMini=dto.DescriptionMini,
                ImageUrl= $"https://api.birjanews.az/assets/images/Default.jpeg",
                Image= "Default.jpeg",
                OrganizerId=dto.OrganizerId,
            };
            if (dto.Image != null)
            {
                string image = dto.Image.SaveImage(_webHostEnvironment.WebRootPath, "assets/images");
                var imagePath = Path.Combine("assets", "images", image);
                var imageUrl = $"https://api.birjanews.az/assets/images/{imagePath}";
                advertisement.Image = image;
                advertisement.ImageUrl = imageUrl;
            }
            _context.Advertisements.Add(advertisement);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = advertisement.Id }, advertisement);
        }
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> Update(int id, [FromForm] AdvertisementPostDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingAdvertisement = await _context.Advertisements.FirstOrDefaultAsync(x=>x.Id==id&&x.IsDeleted==false);

            if (existingAdvertisement == null)
            {
                return NotFound("Advertisement not found.");
            }

            existingAdvertisement.Title = dto.Title;
            existingAdvertisement.Date = dto.Date;
            existingAdvertisement.Status = dto.Status;
            existingAdvertisement.Description = dto.Description;
            existingAdvertisement.DescriptionMini = dto.DescriptionMini;

            // Only change the image if a new one is provided
            if (dto.Image != null)
            {
                string image = dto.Image.SaveImage(_webHostEnvironment.WebRootPath, "assets/images");
                var imagePath = Path.Combine("assets", "images", image);
                var imageUrl = $"{Request.Scheme}://{Request.Host}/{imagePath}";

                existingAdvertisement.Image = image;
                existingAdvertisement.ImageUrl = imageUrl;
            }

            existingAdvertisement.OrganizerId = dto.OrganizerId;

            _context.Advertisements.Update(existingAdvertisement);
            await _context.SaveChangesAsync();

            return Ok(existingAdvertisement);
        }

        [HttpDelete("{id}")]
        [Authorize]

        public async Task<IActionResult> Delete(int id)
        {
            var advertisement = await _context.Advertisements.FirstOrDefaultAsync(x => x.IsDeleted == false && x.Id == id);
            if (advertisement == null)
            {
                return NotFound();
            }
            advertisement.IsDeleted = true;
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
