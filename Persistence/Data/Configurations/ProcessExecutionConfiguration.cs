using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Data.Configurations
{
    public class ProcessExecutionConfiguration : IEntityTypeConfiguration<ProcessExecution>
    {
        public void Configure(EntityTypeBuilder<ProcessExecution> builder)
        {
            builder.HasKey(pe => pe.Id);

            builder.Property(pe => pe.StepName)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(pe => pe.PerformedById)
                   .IsRequired();

            builder.Property(pe => pe.Action)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(pe => pe.Status)
                   .HasDefaultValue(ExecutionStatus.Completed);

            builder.Property(pe => pe.Comments)
                   .HasMaxLength(1000);

            builder.Property(pe => pe.ExecutedAt)
                   .IsRequired();

            // Relationship with Process
            builder.HasOne(pe => pe.Process)
                   .WithMany(p => p.Executions)
                   .HasForeignKey(pe => pe.ProcessId)
                   .OnDelete(DeleteBehavior.Cascade);

            // Relationship with WorkflowStep
            builder.HasOne(pe => pe.WorkflowStep)
                   .WithMany(ws => ws.ProcessExecutions)
                   .HasForeignKey(pe => pe.WorkflowStepId)
                   .OnDelete(DeleteBehavior.Restrict);

            // Relationship with User who performed the action
            builder.HasOne(pe => pe.PerformedBy)
                   .WithMany(u => u.ProcessExecutions)
                   .HasForeignKey(pe => pe.PerformedById)
                   .OnDelete(DeleteBehavior.Restrict);

            // Indexes
            builder.HasIndex(pe => pe.ProcessId);
            builder.HasIndex(pe => pe.PerformedById);
            builder.HasIndex(pe => pe.ExecutedAt);
            builder.HasIndex(pe => new { pe.ProcessId, pe.StepName });
        }
    }
}
