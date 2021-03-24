using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using APICore.Common.DTO.Request;
using APICore.Data.Entities;
using APICore.Data.Entities.Enums;
using APICore.Data.UoW;
using APICore.Services.Exceptions;
using APICore.Services.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace APICore.Services.Impls
{
    public class LogService : ILogService
    {
        private readonly IUnitOfWork _uow;

        private readonly IStringLocalizer<IAccountService> _localizer;

        public LogService(IUnitOfWork uow, IStringLocalizer<IAccountService> localizer)
        {
            _uow = uow ?? throw new ArgumentNullException(nameof(uow));
            _localizer = localizer ?? throw new ArgumentNullException(nameof(localizer));
        }
        public async Task AddLogAsync(AddLogRequest addLogRequest)
        {
            var log = new Log();
            log.EventType = (EventTypeEnum)addLogRequest.EventType;
            log.LogType = (LogTypeEnum)addLogRequest.LogType;
            log.CreatedAt = DateTime.UtcNow;
            log.Description = addLogRequest.Description;
            log.UserId = addLogRequest.UserId; 
            log.Module = addLogRequest.Module;
            log.App = addLogRequest.App;          

            await _uow.LogRepository.AddAsync(log);
            await _uow.CommitAsync();
        }

        public async Task<PaginatedList<Log>> GetPaginatedListAsync(IQueryable<Log> result, int page, int perPage)
        {
            return await PaginatedList<Log>.CreateAsync(result, page, perPage);
        }

        public async Task<IQueryable<Log>> GetLogsAsync(int? page, int? perPage, string sortOrder, int logType=-1, int eventTypeLog=-1, int userId=0)
        {
            var result = _uow.LogRepository.GetAll();

            if (userId != 0)
            {
                result=result.Where(l => l.UserId==userId);
            }
            if (logType != -1)
            {
                result = result.Where(l => l.LogType == (LogTypeEnum)logType);
            }
            if (eventTypeLog != -1)
            {
                result = result.Where(l => l.EventType == (EventTypeEnum)eventTypeLog);
            }
            if (!String.IsNullOrWhiteSpace(sortOrder))
            {
                // sort order section
                switch (sortOrder)
                {
                    case "logType_desc":
                        result = result.OrderByDescending(l => l.LogType);
                        break;

                    case "logType_asc":
                        result = result.OrderBy(l => l.LogType);
                        break;

                    case "eventLogType_desc":
                        result = result.OrderByDescending(l => l.EventType);
                        break;

                    case "eventLogType_asc":
                        result = result.OrderBy(l => l.EventType);
                        break;

                    case "createAt_desc":
                        result = result.OrderByDescending(l => l.CreatedAt);
                        break;

                    case "createAt_asc":
                        result = result.OrderBy(u => u.CreatedAt);
                        break;
                }
            }

            return await Task.FromResult(result);
        }

        public async Task<IQueryable<Log>> GetLogsByUserSerialAsync(int? page, int? perPage, string sortOrder, string serialUser, int logType = -1, int eventTypeLog = -1)
        {
            
            var user = await _uow.UserRepository.FirstOrDefaultAsync(u => u.Identity == serialUser);
               
            if (user == null)
                {
                    throw new UserNotFoundException(_localizer);
                } 
            var result = GetLogsAsync(page, perPage, sortOrder, logType, eventTypeLog, user.Id);

            return await result;
        }
       
    }
}
