﻿namespace Mindmath.Service.Users.DTO
{
	public record UserReturnDto
	{
		public string Id { get; init; }
		public string UserName { get; init; }
		public string Email { get; init; }
		public string PhoneNumber { get; init; }
		public string Fullname { get; init; }
		public string Gender { get; init; }
		public DateTime DateOfBirth { get; init; }
		public string Avatar { get; init; }
	}
}