using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure
{
    public class SensorLogsRepository : ISensorLogsRepository
    {
        private readonly MqttDbContext _context;
        private readonly DbSet<SensorLogs> _dbSet;

        public SensorLogsRepository(MqttDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<SensorLogs>();
        }

        public async Task AddAsync(SensorLogs sensorLogs)
        {
            await _dbSet.AddAsync(sensorLogs);
        }

        public async Task<SensorLogs> GetByIdAsync(Guid id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<IEnumerable<SensorLogs>> GetAllAsync(Expression<Func<SensorLogs, bool>>? expression = null)
        {
            IQueryable<SensorLogs> query = _dbSet;

            if (expression != null)
            {
                query = query.Where(expression);
            }

            return await query.AsNoTracking().ToListAsync();
        }

        public void Update(SensorLogs sensorLogs)
        {
            _dbSet.Update(sensorLogs);
        }

        public void Delete(SensorLogs sensorLogs)
        {
            _dbSet.Remove(sensorLogs);
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }
    }
}
