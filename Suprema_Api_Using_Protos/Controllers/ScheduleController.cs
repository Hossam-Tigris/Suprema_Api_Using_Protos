using Grpc.Core;
using Gsdk.Device;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Suprema_Api_Using_Protos.DTOs;
using Suprema_Api_Using_Protos.Helper;
using Suprema_Api_Using_Protos.Services;

namespace Suprema_Api_Using_Protos.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScheduleController : ControllerBase
    {
        private readonly DeviceManager _manager;

        public ScheduleController(DeviceManager manager)
        {
            _manager = manager;
        }

        [HttpPost("weekly")]
        public async Task<IActionResult> AddWeekly(WeeklyScheduleRequest req)
        {
            var device = CheckDevice.GetDeviceOrThrow(_manager, req.DeviceId);
            try
            {
                var days = new Dictionary<int, List<(int, int)>>();

                foreach (var d in req.Days)
                {
                    days[d.Key] = d.Value
                        .Select(p =>
                            (p.StartHour * 60 + p.StartMinute,
                             p.EndHour * 60 + p.EndMinute))
                        .ToList();
                }

                await device.Services.CreateScheduleSvc()
                    .AddWeeklyAsync(req.DeviceId, req.ScheduleId, req.Name, days);


                return Ok(new ApiResponse<object>(
                  data: null,
                  message: "Schedule created",
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
        public async Task<IActionResult> GetSchedules(uint deviceId)
        {
            var device = CheckDevice.GetDeviceOrThrow(_manager, deviceId);

            try
            {
                var list = await device.Services.CreateScheduleSvc()
                         .GetListAsync(deviceId);

                return Ok(new ApiResponse<object>(
                            data: list.Schedules,
                            message: "Schedules retrieved",
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
