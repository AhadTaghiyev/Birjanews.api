using System;
using System.ComponentModel.DataAnnotations;

namespace Birjanews.Api.Dtos
{
	public class UpdateDto
	{
        [Required]
        public string UserId { get; set; }
        [Required]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        public string? ExsistPassword { get; set; }

        [DataType(DataType.Password)]
        public string NewPassword { get; set; }
    }
}

