﻿using System.ComponentModel.DataAnnotations;

namespace Mindmath.Domain.Models
{
	public class ProblemType
	{
		[Key]
		public Guid Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public DateOnly CreatedAt { get; set; }
		public DateOnly UpdatedAt { get; set; }
		public Guid? TopicId { get; set; }
		public Topic Topic { get; set; }
		public ICollection<InputParameter> InputParameters { get; set; } = new List<InputParameter>();
		public ICollection<Solution> Solutions { get; set; } = new List<Solution>();
	}
}
