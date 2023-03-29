using System.Linq.Expressions;

namespace Infrastructure
{
    public interface ISensorLogsRepository
    {
        Task AddAsync(SensorLogs sensorLogs);
        Task<SensorLogs> GetByIdAsync(Guid id);
        Task<IEnumerable<SensorLogs>> GetAllAsync(Expression<Func<SensorLogs, bool>>? expression = null);
        void Update(SensorLogs sensorLogs);
        void Delete(SensorLogs sensorLogs);
        Task Save();
    }
}
