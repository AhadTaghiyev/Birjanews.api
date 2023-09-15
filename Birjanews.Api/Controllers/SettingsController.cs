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
    public class SettingsController : ControllerBase
    {
        private readonly BirjaDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public SettingsController(BirjaDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        public async Task<IActionResult> GetSetting()
        {
            var setting = await _context.Settings.FirstOrDefaultAsync();
            return Ok(setting);
        }

    
    
        [HttpPut()]
        [Authorize]

        public async Task<IActionResult> UpdateSetting([FromBody] SettingPostDto setting)
        {
            var Updatesetting = await _context.Settings.FirstOrDefaultAsync();
            Updatesetting.Adress = setting.Adress;
            Updatesetting.Email = setting.Email;
            Updatesetting.Phone = setting.Phone;
            Updatesetting.Whatsapp = setting.Whatsapp;
            Updatesetting.Youtube = setting.Youtube;
            Updatesetting.Instagram = setting.Instagram;
            Updatesetting.Facebook = setting.Facebook;
            _context.Entry(Updatesetting).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
