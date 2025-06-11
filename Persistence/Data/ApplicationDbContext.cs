using Domain.Entities.Identity;
using Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Persistence.Data.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace Persistence.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // DbSets for our domain entities
        public DbSet<Workflow> Workflows { get; set; }
        public DbSet<WorkflowStep> WorkflowSteps { get; set; }
        public DbSet<Process> Processes { get; set; }
        public DbSet<ProcessExecution> ProcessExecutions { get; set; }
        public DbSet<ProcessValidationLog> ProcessValidationLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Apply all configurations
            ApplyEntityConfigurations(builder);

            // Also We Can Use This :) ... Auto-apply all configurations from current assembly
            // builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        private static void ApplyEntityConfigurations(ModelBuilder builder)
        {

            // Workflow Domain Configurations
            builder.ApplyConfiguration(new WorkflowConfiguration());
            builder.ApplyConfiguration(new WorkflowStepConfiguration());
            builder.ApplyConfiguration(new ProcessConfiguration());
            builder.ApplyConfiguration(new ProcessExecutionConfiguration());
            builder.ApplyConfiguration(new ProcessValidationLogConfiguration());

            // Identity Configurations
            builder.ApplyConfiguration(new ApplicationUserConfiguration());
            builder.ApplyConfiguration(new IdentityRoleConfiguration());
            builder.ApplyConfiguration(new IdentityUserRoleConfiguration());
            builder.ApplyConfiguration(new IdentityUserClaimConfiguration());
            builder.ApplyConfiguration(new IdentityUserLoginConfiguration());
            builder.ApplyConfiguration(new IdentityRoleClaimConfiguration());
            builder.ApplyConfiguration(new IdentityUserTokenConfiguration());
        }
    }
}
