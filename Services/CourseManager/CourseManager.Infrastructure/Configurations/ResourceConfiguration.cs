using CourseManager.Domain.Entities;
using CourseManager.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseManager.Infrastructure.Configurations
{
    public class ResourceConfiguration : IEntityTypeConfiguration<Resource>
    {
        public void Configure(EntityTypeBuilder<Resource> builder)
        {
            builder.HasKey(r => r.Id);
            builder.Property(r => r.Type).IsRequired()
                .HasConversion(
                    t => t.ToString(),
                    tdb => (ResourceType)Enum.Parse(typeof(ResourceType), tdb)
                );
        }
    }
}
