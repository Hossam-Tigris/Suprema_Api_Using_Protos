using Microsoft.AspNetCore.Mvc;
using Suprema_Api_Using_Protos.Helper;
using Suprema_Api_Using_Protos.Services;
using Suprema_Api_Using_Protos.DTOs;
using Gsdk.Tna;

namespace Suprema_Api_Using_Protos.Controllers
{
    [ApiController]
    [Route("api/tna")]
    public class TnaController : ControllerBase
    {
        private readonly DeviceManager _manager;

        public TnaController(DeviceManager manager)
        {
            _manager = manager;
        }

        [HttpGet("{deviceId}/config")]
        public async Task<IActionResult> GetConfig(uint deviceId)
        {
            var device = CheckDevice.GetDeviceOrThrow(_manager, deviceId);

            var config = await device.Services
                .CreateTnaSvc()
                .GetConfigAsync(deviceId);

            return Ok(new ApiResponse<TNAConfig>(
                config
                ,
                true
                ,
                "TNA config loaded"));
        }

        [HttpPost("{deviceId}/config")]
        public async Task<IActionResult> SetConfig(
            uint deviceId,
            [FromBody] TnaConfigRequest dto)
        {
            var device = CheckDevice.GetDeviceOrThrow(_manager, deviceId);

            var config = new TNAConfig
            {
                Mode = dto.Mode,
                Key = dto.Key,
                IsRequired = dto.IsRequired
            };

            config.Schedules.AddRange(dto.Schedules);
            config.Labels.AddRange(dto.Labels);

            await device.Services
                .CreateTnaSvc()
                .SetConfigAsync(deviceId, config);

            return Ok(new ApiResponse<object>(
                Array.Empty<object>(),
                true,
                "TNA config updated"
                ));
        }

        [HttpGet("{deviceId}/logs")]
        public async Task<IActionResult> GetLogs(
            uint deviceId,
            [FromQuery] uint startEventId = 0,
            [FromQuery] uint max = 100)
        {
            var device = CheckDevice.GetDeviceOrThrow(_manager, deviceId);

            var logs = await device.Services
                .CreateTnaSvc()
                .GetLogsAsync(deviceId, startEventId, max);

            return Ok(new ApiResponse<object>(
                logs.TNAEvents,
                true,
                "Logs loaded"
                ));
        }
    }
}
