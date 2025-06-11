using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Data.Configurations
{
    public class WorkflowStepConfiguration : IEntityTypeConfiguration<WorkflowStep>
    {
        public void Configure(EntityTypeBuilder<WorkflowStep> builder)
        {
            builder.HasKey(ws => ws.Id);

            builder.Property(ws => ws.StepName)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(ws => ws.AssignedRole)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(ws => ws.NextStep)
                   .HasMaxLength(200);

            builder.Property(ws => ws.ValidationEndpoint)
                   .HasMaxLength(500);

            builder.Property(ws => ws.RequiresValidation)
                   .HasDefaultValue(false);

            // Relationship with Workflow
            builder.HasOne(ws => ws.Workflow)
                   .WithMany(w => w.Steps)
                   .HasForeignKey(ws => ws.WorkflowId)
                   .OnDelete(DeleteBehavior.Cascade);

            // Optional relationship with specific user
            builder.HasOne(ws => ws.AssignedUser)
                   .WithMany()
                   .HasForeignKey(ws => ws.AssignedUserId)
                   .OnDelete(DeleteBehavior.SetNull);

            // Indexes
            builder.HasIndex(ws => new { ws.WorkflowId, ws.Order });
            builder.HasIndex(ws => ws.AssignedRole);
        }
    }
}
