using Domain.Enums;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;

namespace Persistence.Data.Configurations
{
    public class ProcessConfiguration : IEntityTypeConfiguration<Process>
    {
        public void Configure(EntityTypeBuilder<Process> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.InitiatorId)
                   .IsRequired();

            builder.Property(p => p.Status)
                   .HasDefaultValue(ProcessStatus.Active);

            builder.Property(p => p.CurrentStep)
                   .HasMaxLength(200);

            builder.Property(p => p.Notes)
                   .HasMaxLength(1000);

            builder.Property(p => p.StartedAt)
                   .IsRequired();

            // Relationship with Workflow
            builder.HasOne(p => p.Workflow)
                   .WithMany(w => w.Processes)
                   .HasForeignKey(p => p.WorkflowId)
                   .OnDelete(DeleteBehavior.Restrict);

            // Relationship with Initiator
            builder.HasOne(p => p.Initiator)
                   .WithMany(u => u.InitiatedProcesses)
                   .HasForeignKey(p => p.InitiatorId)
                   .OnDelete(DeleteBehavior.Restrict);

            // Relationship with Current Assignee
            builder.HasOne(p => p.CurrentAssignee)
                   .WithMany()
                   .HasForeignKey(p => p.CurrentAssigneeId)
                   .OnDelete(DeleteBehavior.SetNull);

            // Indexes
            builder.HasIndex(p => p.Status);
            builder.HasIndex(p => p.InitiatorId);
            builder.HasIndex(p => p.CurrentAssigneeId);
            builder.HasIndex(p => p.StartedAt);
            builder.HasIndex(p => new { p.WorkflowId, p.Status });
        }
    }
}
