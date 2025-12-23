using Microsoft.AspNetCore.Mvc;
using Suprema_Api_Using_Protos.Helper;
using Suprema_Api_Using_Protos.Services;

[ApiController]
[Route("api/health")]
public class HealthController : ControllerBase
{
    private readonly DeviceManager _manager;

    public HealthController(DeviceManager manager)
    {
        _manager = manager;
    }

    [HttpGet]
    public IActionResult Health()
    {
        if (!_manager.GatewayConnected)
        {
            return StatusCode(502,
                new ApiResponse<object>(
                    data: Array.Empty<object>(),
                    success: false,
                    message: "Gateway is not reachable"
                ));
        }

        return Ok(
            new ApiResponse<object>(
                data: new
                {
                    gatewayConnected = true,
                    connectedDevices = _manager.Devices.Count,
                    devices = _manager.Devices.Select(d => new
                    {
                        d.DeviceID,
                        d.IP
                    })
                },
                success: true,
                message: "Service is healthy"
            ));
    }
}
