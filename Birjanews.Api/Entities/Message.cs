﻿using System;
namespace Birjanews.Api.Entities
{
	public class Message:BaseEntity
	{
		public string Name { get; set; }
		public string Email { get; set; }
		public string Phone { get; set; }
		public string Text { get; set; }
    }
}

