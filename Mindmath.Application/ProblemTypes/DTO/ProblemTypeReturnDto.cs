﻿namespace Mindmath.Service.ProblemTypes.DTO
{
	public record ProblemTypeReturnDto
	{
		public Guid Id { get; init; }
		public string Name { get; init; }
		public string Description { get; init; }
		public int NumberOfInputs { get; init; }
		public bool Active { get; init; }
	}
}
