using Grpc.Core;
using Gsdk.Display;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Suprema_Api_Using_Protos.DTOs;
using Suprema_Api_Using_Protos.Helper;
using Suprema_Api_Using_Protos.Services;

namespace Suprema_Api_Using_Protos.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeviceController : ControllerBase
    {
        private readonly DeviceManager _manager;

        public DeviceController(DeviceManager manager)
        {
            _manager = manager;
        }
        [HttpGet("{deviceId}/Info")]
        public async Task<IActionResult> GetDeviceInfo(uint deviceId)
        {
            var device = CheckDevice.GetDeviceOrThrow(_manager, deviceId);
            try
            {
                var DeviceInformation = await device.Services.CreateDeviceSvc().GetDeviceInfoAsync();

                return Ok(new ApiResponse<object>(
                    data: new { deviceId, DeviceInformation },
                    message: "Device Info retrieved",
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
        [HttpGet("{deviceId}/Log")]
        public async Task<IActionResult> GetDeviceLog(uint deviceId)
        {
            var device = CheckDevice.GetDeviceOrThrow(_manager, deviceId);
            try
            {
                var DeviceLog = await device.Services.CreateEventLogSvc().GetLogsAsync(deviceId);

                return Ok(new ApiResponse<object>(
                    data: new { deviceId, DeviceLog },
                    message: "Device log retrieved",
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

        [HttpGet("{deviceId}/time")]
        public async Task<IActionResult> GetDeviceTime(uint deviceId)
        {
            var device = CheckDevice.GetDeviceOrThrow(_manager, deviceId);
            try
            {
                var time = await device.Services.CreateTimeSvc().GetDeviceTimeAsync(deviceId);

                return Ok(new ApiResponse<object>(
                    data: new { deviceId, time },
                    message: "Device time retrieved",
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

        [HttpPost("{deviceId}/time")]
        public async Task<IActionResult> SetDeviceTime(uint deviceId, [FromBody] SetTimeRequest dto)
        {
            var device = CheckDevice.GetDeviceOrThrow(_manager, deviceId);
            try
            {
                await device.Services.CreateTimeSvc().SetSpecificTimeAsync(deviceId, dto.Hour, dto.Minute);

                return Ok(new ApiResponse<object>(
                    data: null,
                    message: "Time updated",
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

        [HttpGet("{deviceId}/config")]
        public async Task<IActionResult> GetDeviceConfig(uint deviceId)
        {
            var device = CheckDevice.GetDeviceOrThrow(_manager, deviceId);
            try
            {
                var config = await device.Services.CreateDisplaySvc().GetConfigAsync(deviceId);

                return Ok(new ApiResponse<object>(
                    data: config,
                    message: "Device config retrieved",
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

        //[HttpPost("{deviceId}/config")]
        //public async Task<IActionResult> SetDeviceConfig(uint deviceId, [FromBody] DisplayConfigRequest dto)
        //{
        //    var device = CheckDevice.GetDeviceOrThrow(_manager, deviceId);

        //    try
        //    {
        //        var config = await device.Services.CreateDisplaySvc().GetConfigAsync(deviceId);

        //        if (dto.Volume.HasValue) config.Volume = dto.Volume.Value;
        //        if (dto.Language.HasValue) config.Language = dto.Language.Value;
        //        if (dto.Background.HasValue) config.Background = dto.Background.Value;
        //        if (dto.Theme.HasValue) config.Theme = dto.Theme.Value;

        //        if (dto.UseVoice.HasValue) config.UseVoice = dto.UseVoice.Value;
        //        if (dto.DateFormat.HasValue) config.DateFormat = dto.DateFormat.Value;
        //        if (dto.TimeFormat.HasValue) config.TimeFormat = dto.TimeFormat.Value;
        //        if (dto.ShowDateTime.HasValue) config.ShowDateTime = dto.ShowDateTime.Value;

        //        if (dto.MenuTimeout.HasValue) config.MenuTimeout = dto.MenuTimeout.Value;
        //        if (dto.MsgTimeout.HasValue) config.MsgTimeout = dto.MsgTimeout.Value;
        //        if (dto.BacklightTimeout.HasValue) config.BacklightTimeout = dto.BacklightTimeout.Value;

        //        if (dto.UseUserPhrase.HasValue) config.UseUserPhrase = dto.UseUserPhrase.Value;

        //        await device.Services.CreateDisplaySvc().SetConfigAsync(deviceId, config);

        //        return Ok(new ApiResponse<object>(
        //            data: null,
        //            message: "Config updated",
        //            success: true
        //        ));
        //    }
        //    catch (RpcException ex)
        //    {
        //        return StatusCode(502, new ApiResponse<object>(
        //            data: null,
        //            success: false,
        //            message: ex.Status.Detail
        //        ));
        //    }
        //}

        [HttpPost("{deviceId}/config")]
        public async Task<IActionResult> SetDeviceConfig(uint deviceId, [FromBody] DisplayConfigRequest dto)
        {
            var device = CheckDevice.GetDeviceOrThrow(_manager, deviceId);

            try
            {
                var config = await device.Services.CreateDisplaySvc().GetConfigAsync(deviceId);

                var dtoProps = typeof(DisplayConfigRequest).GetProperties();
                var configProps = typeof(DisplayConfig).GetProperties();

                foreach (var prop in dtoProps)
                {
                    var value = prop.GetValue(dto);
                    if (value != null)
                    {
                        var configProp = configProps.FirstOrDefault(p => p.Name == prop.Name);
                        if (configProp != null)
                        {
                            configProp.SetValue(config, value);
                        }
                    }
                }

                await device.Services.CreateDisplaySvc().SetConfigAsync(deviceId, config);

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

        [HttpPost("{deviceId}/volume")]
        public async Task<IActionResult> SetVolume(uint deviceId, [FromBody] uint volume)
        {
            var device = CheckDevice.GetDeviceOrThrow(_manager, deviceId);
            try
            {
                await device.Services.CreateDisplaySvc().SetVolumeAsync(deviceId, volume);

                return Ok(new ApiResponse<object>(
                    data: null,
                    message: "Volume updated",
                    success:true
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

        [HttpPost("{deviceId}/notice")]
        public async Task<IActionResult> UpdateNotice(uint deviceId, [FromBody] string notice)
        {
            var device = CheckDevice.GetDeviceOrThrow(_manager, deviceId);
            try
            {
                await device.Services.CreateDisplaySvc().UpdateNoticeAsync(deviceId, notice);

                return Ok(new ApiResponse<object>(
                    data: null,
                    message: "Notice updated",
                    success:true
                    
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

        [HttpPost("{deviceId}/sound")]
        public async Task<IActionResult> UpdateSound(uint deviceId, [FromBody] SoundRequest dto)
        {
            var device = CheckDevice.GetDeviceOrThrow(_manager, deviceId);
            try
            {
                await device.Services.CreateDisplaySvc().UpdateSoundAsync(deviceId, dto.Index, dto.FilePath);

                return Ok(new ApiResponse<object>(
                    data: null,
                    message: "Sound updated"
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

        [HttpPost("{deviceId}/reboot")]
        public async Task<IActionResult> RebootDevice(uint deviceId)
        {
            var device = CheckDevice.GetDeviceOrThrow(_manager, deviceId);
            try
            {
                await device.Services.CreateDeviceSvc().RebootDeviceAsync(deviceId);

                return Ok(new ApiResponse<object>(
                    data: null,
                    message: "Device rebooted"
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

        [HttpPost("{deviceId}/factory-reset")]
        public async Task<IActionResult> FactoryReset(uint deviceId)
        {
            var device = CheckDevice.GetDeviceOrThrow(_manager, deviceId);
            try
            {
                await device.Services.CreateDeviceSvc().FactoryResetAsync(deviceId);

                return Ok(new ApiResponse<object>(
                    data: null,
                    message: "Factory reset done",
                    success:true
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

        [HttpGet]
        public IActionResult GetDevices()
        {
            try
            {

            var devices = _manager.Devices.Select(d => new { d.DeviceID, d.IP }).ToList();
            return Ok(new ApiResponse<object>(
                data: devices,
                message: "Devices retrieved",
                success:true
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

        [HttpPost("connect")]
        public async Task<IActionResult> Connect([FromBody] ConnectDeviceRequest dto)
        {
            try
            {

            var deviceId = await _manager.ConnectSingleDeviceAsync(dto.Ip, dto.Port, dto.UseSSL);

            return Ok(new ApiResponse<object>(
                data: new { DeviceID = deviceId },
                message: "Device connected",
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

        [HttpDelete("{deviceId}/disconnect")]
        public IActionResult Disconnect(uint deviceId)
        {
            try
            {

            _manager.DisconnectDevice(deviceId);
            return Ok(new ApiResponse<object>(
                data: null,
                message: "Device disconnected",
                success:true
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

        [HttpPost("{deviceId}/reconnect")]
        public async Task<IActionResult> Reconnect(uint deviceId)
        {
            try
            {

            var newId = await _manager.ReconnectDeviceAsync(deviceId);
            return Ok(new ApiResponse<object>(
                data: new { NewDeviceID = newId },
                message: "Device reconnected"
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
    

