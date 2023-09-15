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
    public class MessageController : ControllerBase
    {
        private readonly BirjaDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public MessageController(BirjaDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        public async Task<IActionResult> GetMessages()
        {
            var Messages = await _context.Messages.Where(x => x.IsDeleted == false).ToListAsync();
            return Ok(Messages);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateMessage([FromForm]Message Message)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

       
            _context.Messages.Add(Message);
            await _context.SaveChangesAsync();
            return Ok();
        }
    
        [HttpDelete("{id}")]
        [Authorize]

        public async Task<IActionResult> DeleteMessage(int id)
        {
            var Message = await _context.Messages.FirstOrDefaultAsync(x=>x.IsDeleted==false&&x.Id==id);
            if (Message == null)
            {
                return NotFound();
            }
            Message.IsDeleted = true;
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
