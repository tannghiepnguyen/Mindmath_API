﻿
using Microsoft.AspNetCore.Identity;

namespace Mindmath.Repository.Models
{
	public class User : IdentityUser
	{
		public string Fullname { get; set; }
		public string Gender { get; set; }
		public DateTime DateOfBirth { get; set; }
		public DateTime CreateAt { get; set; }
		public DateTime UpdateAt { get; set; }
		public string? Avatar { get; set; }
		public bool Active { get; set; }
		public Wallet Wallet { get; set; }
		public ICollection<InputParameter> InputParameters { get; set; }
	}
}