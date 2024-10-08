﻿using System.ComponentModel.DataAnnotations;

namespace Mindmath.Repository.Models
{
	public class Wallet
	{
		[Key]
		public Guid Id { get; set; }
		public double Balance { get; set; }
		public string? UserId { get; set; }
		public User User { get; set; }
	}
}
