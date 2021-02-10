using APICore.API.BasicResponses;
using APICore.Common.DTO.Request;
using APICore.Common.DTO.Response;
using APICore.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Threading.Tasks;

namespace APICore.API.Controllers
{
    [Route("api/setting")]
    public class SettingController : Controller
    {
        private readonly ISettingService _settingService;
        private readonly IMapper _mapper;

        public SettingController(ISettingService settingService, IMapper mapper)
        {
            _settingService = settingService ?? throw new ArgumentNullException(nameof(settingService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        /// <summary>
        /// Add a setting. Requires authentication.
        /// </summary>
        /// <param name="setting">
        /// Setting request object. Include key and value. Key is unique in database.
        /// </param>
        [HttpPost]
        [Route("set-setting")]
        [Authorize]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        public async Task<IActionResult> SetSetting([FromBody] SettingRequest setting)
        {
            var result = await _settingService.SetSettingAsync(setting);

            var settingResponse = _mapper.Map<SettingResponse>(result);
            return Ok(new ApiOkResponse(settingResponse));
        }

        /// <summary>
        /// Get setting. Requires authentication.
        /// </summary>
        /// <param name="key">Setting key.</param>
        [HttpGet()]
        [Authorize]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetSetting(string key)
        {
            var result = await _settingService.GetSettingAsync(key);

            return Ok(new ApiOkResponse(result));
        }
    }
}