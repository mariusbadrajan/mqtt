using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure
{
    public class SensorLogsConfig : IEntityTypeConfiguration<SensorLogs>
    {
        public void Configure(EntityTypeBuilder<SensorLogs> builder)
        {
            builder
               .ToTable("SensorLogs");

            builder
                .HasKey(pk => pk.Id);
        }
    }
}
