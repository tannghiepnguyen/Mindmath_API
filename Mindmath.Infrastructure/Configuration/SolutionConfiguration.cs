﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mindmath.Repository.Models;

namespace Mindmath.Infrastructure.Configuration
{
	public class SolutionConfiguration : IEntityTypeConfiguration<Solution>
	{
		public void Configure(EntityTypeBuilder<Solution> builder)
		{
			builder.HasKey(x => x.Id);
			builder.Property(x => x.Link).IsRequired();
			builder.Property(x => x.Description).IsRequired();
			builder.Property(x => x.CreatedAt);
			builder.Property(x => x.UpdatedAt);
			builder.Property(x => x.DeletedAt);
			builder.Property(x => x.Active).IsRequired();
			builder.HasOne(x => x.InputParameter).WithOne(x => x.Solution).HasForeignKey<Solution>(x => x.InputParameterId).OnDelete(DeleteBehavior.Restrict);
			builder.HasOne(x => x.Transaction).WithOne(x => x.Solution).HasForeignKey<Solution>(x => x.TransactionId).OnDelete(DeleteBehavior.Restrict);

		}
	}
}
