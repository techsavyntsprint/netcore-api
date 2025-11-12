using APICore.Common.DTO.Request;
using APICore.Data.Entities;
using APICore.Services.Utils;
using System.Linq;
using System.Threading.Tasks;

namespace APICore.Services
{
   public interface ILogService
    {
        Task AddLogAsync(AddLogRequest logRequest);
        Task<PaginatedList<Log>> GetPaginatedListAsync(IQueryable<Log> result, int page, int perPage);
        Task<IQueryable<Log>> GetLogsAsync(int ? page, int? perPage, string sortOrder, int logType=-1, int eventTypeLog=-1, int userId = 0);
        Task<IQueryable<Log>> GetLogsByUserSerialAsync(int? page, int? perPage, string sortOrder, string serialUser, int logType = -1, int eventTypeLog = -1);
        
    }
}
