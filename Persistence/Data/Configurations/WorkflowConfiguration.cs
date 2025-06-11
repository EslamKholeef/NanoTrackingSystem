using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace Persistence.Data.Configurations
{
    public class WorkflowConfiguration : IEntityTypeConfiguration<Workflow>
    {
        public void Configure(EntityTypeBuilder<Workflow> builder)
        {
            builder.HasKey(w => w.Id);

            builder.Property(w => w.Name)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(w => w.Description)
                   .HasMaxLength(1000);

            builder.Property(w => w.CreatedAt)
                   .IsRequired();

            builder.Property(w => w.IsActive)
                   .HasDefaultValue(true);

            // Indexes
            builder.HasIndex(w => w.Name);
            builder.HasIndex(w => w.CreatedAt);
            builder.HasIndex(w => w.IsActive);
        }
    }
}
