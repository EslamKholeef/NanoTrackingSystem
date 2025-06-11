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
    public class ProcessValidationLogConfiguration : IEntityTypeConfiguration<ProcessValidationLog>
    {
        public void Configure(EntityTypeBuilder<ProcessValidationLog> builder)
        {
            builder.HasKey(pvl => pvl.Id);

            builder.Property(pvl => pvl.StepName)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(pvl => pvl.ValidationEndpoint)
                   .HasMaxLength(500);

            builder.Property(pvl => pvl.ValidationResponse)
                   .HasMaxLength(2000);

            builder.Property(pvl => pvl.ErrorMessage)
                   .HasMaxLength(1000);

            builder.Property(pvl => pvl.ValidatedAt)
                   .IsRequired();

            // Relationship with Process
            builder.HasOne(pvl => pvl.Process)
                   .WithMany(p => p.ValidationLogs)
                   .HasForeignKey(pvl => pvl.ProcessId)
                   .OnDelete(DeleteBehavior.Cascade);

            // Indexes
            builder.HasIndex(pvl => pvl.ProcessId);
            builder.HasIndex(pvl => pvl.ValidatedAt);
            builder.HasIndex(pvl => pvl.ValidationResult);
        }
    }
}
