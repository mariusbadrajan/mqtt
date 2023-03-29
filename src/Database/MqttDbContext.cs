using Microsoft.EntityFrameworkCore;

namespace Infrastructure
{
    public class MqttDbContext : DbContext
    {
        public MqttDbContext(DbContextOptions<MqttDbContext> options) : base(options)
        {

        }

        public DbSet<SensorLogs> SensorLogs { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=.\SQLEXPRESS;Database=Mqtt;Trusted_Connection=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(MqttDbContext).Assembly);
        }
    }
}
