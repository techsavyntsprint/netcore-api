using APICore.API.BasicResponses;
using APICore.Common.DTO.Request;
using APICore.Common.DTO.Response;
using APICore.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;

namespace APICore.API.Controllers
{
    [Route("api/log")]
    public class LogController : Controller
    {
        private readonly ILogService _logService;
        private readonly IMapper _mapper;

        public LogController(ILogService logService, IMapper mapper)
        {
            _logService = logService ?? throw new ArgumentNullException(nameof(logService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }


        /// <summary>
        /// List all logs. Requires authentication.
        /// </summary>       
        /// <param name="page">The page to be displayed. 1 by default.</param>
        /// <param name="perPage">The number of logs to be displayed per page. 10 by default.</param>
        /// <param name="sortOrder"></param>
        /// <param name="logType">
        /// Type of log. Possible values:-1: All, 0-APPLICATION, 1-SYSTEM, 2-SECURITY
        /// </param>
        /// <param name="eventTypeLog">
        /// Type of event log. Possible values:-1: All, 0-INFORMATION, 1-WARNING, 2-ERROR
        /// </param>
        [HttpGet()]
        [Authorize]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAllLogs( int? page, int? perPage, string sortOrder, int logType, int eventTypeLog)
        {
            var pag = page ?? 1;
            var perPag = perPage ?? 10;

            var totalResults = await _logService.GetLogsAsync( pag, perPag, sortOrder, logType, eventTypeLog);

            var result = await _logService.GetPaginatedListAsync(totalResults, pag, perPag);

            HttpContext.Response.Headers.Add("PagingData", JsonConvert.SerializeObject(result.GetPaginationData));
            HttpContext.Response.Headers["Access-Control-Expose-Headers"] = "PagingData";
            HttpContext.Response.Headers["Access-Control-Allow-Headers"] = "PagingData";

            var logList = _mapper.Map<IEnumerable<LogResponse>>(result);

            return Ok(new ApiOkResponse(logList));
           
        }



        /// <summary>
        /// List current user's logs. Requires authentication.
        /// </summary>     
        /// <param name="logType">
        /// Type of log. Possible values:-1: All, 0-APPLICATION, 1-SYSTEM, 2-SECURITY
        /// </param>
        /// <param name="eventTypeLog">
        /// Type of event log. Possible values:-1: All, 0-INFORMATION, 1-WARNING, 2-ERROR
        /// </param>
        /// <param name="page">The page to be displayed. 1 by default.</param>
        /// <param name="perPage">The number of logs to be displayed per page. 10 by default.</param>
        /// <param name="sortOrder"></param>
        [HttpGet("current-user-logs")]
        [Authorize]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetCurrentUserLogs(int logType, int eventTypeLog, int? page, int? perPage, string sortOrder)
        {
            var pag = page ?? 1;
            var perPag = perPage ?? 10;
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.UserData)?.Value;

            var totalResults = await _logService.GetLogsAsync(pag, perPag, sortOrder, logType, eventTypeLog, int.Parse(userId));

            var result = await _logService.GetPaginatedListAsync(totalResults, pag, perPag);

            HttpContext.Response.Headers.Add("PagingData", JsonConvert.SerializeObject(result.GetPaginationData));
            HttpContext.Response.Headers["Access-Control-Expose-Headers"] = "PagingData";
            HttpContext.Response.Headers["Access-Control-Allow-Headers"] = "PagingData";

            var logList = _mapper.Map<IEnumerable<LogResponse>>(result);

            return Ok(new ApiOkResponse(logList));

        }

        /// <summary>
        /// List logs by a specified user. Requires authentication.
        /// </summary>
        /// <param name="logType">
        /// Type of log. Possible values:-1: All, 0-APPLICATION, 1-SYSTEM, 2-SECURITY
        /// </param>
        /// <param name="eventTypeLog">
        /// Type of event log. Possible values:-1: All, 0-INFORMATION, 1-WARNING, 2-ERROR
        /// </param>
        /// /// <param name="page">The page to be displayed. 1 by default.</param>
        /// <param name="perPage">The number of logs to be displayed per page. 10 by default.</param>
        /// <param name="sortOrder"></param>
        /// <param name="serialUser"></param> The serial user is a user identity.
        [HttpGet("logs-by-user")]
        [Authorize]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetLogsByUserId(int logType, int eventTypeLog, int? page, int? perPage, string sortOrder, string serialUser)
        {
            var pag = page ?? 1;
            var perPag = perPage ?? 10;

            var totalResults = await _logService.GetLogsByUserSerialAsync(pag, perPag, sortOrder, serialUser, logType, eventTypeLog);

            var result = await _logService.GetPaginatedListAsync(totalResults, pag, perPag);

            HttpContext.Response.Headers.Add("PagingData", JsonConvert.SerializeObject(result.GetPaginationData));
            HttpContext.Response.Headers["Access-Control-Expose-Headers"] = "PagingData";
            HttpContext.Response.Headers["Access-Control-Allow-Headers"] = "PagingData";

            var logList = _mapper.Map<IEnumerable<LogResponse>>(result);

            return Ok(new ApiOkResponse(logList));

        }
    }
}
