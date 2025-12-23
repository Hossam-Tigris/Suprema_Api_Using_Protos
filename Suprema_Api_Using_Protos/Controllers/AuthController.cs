using Grpc.Core;
using Gsdk.Auth;
using Gsdk.Time;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Suprema_Api_Using_Protos.DTOs;
using Suprema_Api_Using_Protos.Helper;
using Suprema_Api_Using_Protos.Services;

namespace Suprema_Api_Using_Protos.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly DeviceManager _manager;

        public AuthController(DeviceManager manager)
        {
            _manager = manager;
        }

        [HttpPost("set")]
        public async Task<IActionResult> SetAuth(uint deviceId,AuthMode mode, uint scheduleId)
        {
            var device = CheckDevice.GetDeviceOrThrow(_manager, deviceId);

            try
            {

            await device.Services.CreateAuthSvc()
                .SetAuthAsync(deviceId, mode, scheduleId);

                return Ok(new ApiResponse<object>(
                                  data: null,
                                  message: "Config updated",
                                  success: true

                              ));
            }
            catch (RpcException ex)
            {
                return StatusCode(502, new ApiResponse<object>(
                    data: null,
                    success: false,
                    message: ex.Status.Detail
                ));
            }
        }

        [HttpGet("{deviceId}")]
        public async Task<IActionResult> GetConfig(uint deviceId)
        {
            var device = CheckDevice.GetDeviceOrThrow(_manager, deviceId);

            try
            {

            var cfg = await device.Services.CreateAuthSvc()
                .GetConfigAsync(deviceId);

            return Ok(new ApiResponse<object>(
                    data: cfg,
                    message: "config retrieved",
                    success: true
                ));

                
            }


            catch (RpcException ex)
            {
                return StatusCode(502, new ApiResponse<object>(
                    data: null,
                    success: false,
                    message: ex.Status.Detail
                ));
            }
        }
    }
}