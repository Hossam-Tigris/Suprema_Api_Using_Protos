using Grpc.Core;
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
    public class UserController : ControllerBase
    {
        private readonly DeviceManager _manager;

        public UserController(DeviceManager manager)
        {
            _manager = manager;
        }

        [HttpPost("{deviceId}/user")]
        public async Task<IActionResult> AddUser(uint deviceId, [FromBody] UserRequest dto)
        {
            var device = CheckDevice.GetDeviceOrThrow(_manager, deviceId);

            try
            {


            await device.Services.CreateUserSvc().AddUserAsync(deviceId, dto.UserId, dto.Name);

                return Ok(new ApiResponse<object>(
                            data: null,
                            message: "User added",
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

        [HttpPut("{deviceId}/user")]
        public async Task<IActionResult> UpdateUser(uint deviceId, [FromBody] UserRequest dto)
        {
            var device = CheckDevice.GetDeviceOrThrow(_manager, deviceId);

            try
            {
            var session = _manager.Devices.FirstOrDefault(d => d.DeviceID == deviceId);
            if (session == null) return NotFound("Device not connected");

            await session.Services.CreateUserSvc().UpdateUserAsync(deviceId, dto.UserId, dto.Name);

                return Ok(new ApiResponse<object>(
                           data: null,
                           message: "User updated",
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

        [HttpDelete("{deviceId}/user/{userId}")]
        public async Task<IActionResult> DeleteUser(uint deviceId, uint userId)
        {
            var device = CheckDevice.GetDeviceOrThrow(_manager, deviceId);

            try
            {
                await device.Services.CreateUserSvc().DeleteUserAsync(deviceId, userId);
                return Ok(new ApiResponse<object>(
                            data: null,
                            message: "User deleted",
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

        [HttpPost("{deviceId}/user/{userId}/card")]
        public async Task<IActionResult> AddUserCard(uint deviceId, uint userId, [FromBody] CardRequest dto)
        {
            var device = CheckDevice.GetDeviceOrThrow(_manager, deviceId);

            try
            {
            await device.Services.CreateUserSvc().AddCardAsync(deviceId, userId, dto.CardBytes);
                return Ok(new ApiResponse<object>(
                         data: null,
                         message: "Card added",
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

        [HttpPost("{deviceId}/user/{userId}/finger")]
        public async Task<IActionResult> AddUserFinger(uint deviceId, uint userId)
        {
            var device = CheckDevice.GetDeviceOrThrow(_manager, deviceId);

            try
            {
                bool result = await device.Services.CreateUserSvc()
               .ScanVerifyAndAssignFingerAsync(deviceId, userId, device.Services.CreateFingerSvc());

                if (!result)
                {
                    return BadRequest(new ApiResponse<object>(
                         data: null,
                         message: "Finger assigned failed",
                         success: false

                         ));
                }
                return Ok(new ApiResponse<object>(
                    data: null,
                    message: "Finger assigned ",
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

        [HttpPost("{deviceId}/user/{userId}/face")]
        public async Task<IActionResult> AddUserFace(uint deviceId, uint userId)
        {
            var device = CheckDevice.GetDeviceOrThrow(_manager, deviceId);

            try
            {
                var faceData = await device.Services.CreateFaceSvc().ScanFaceAsync(deviceId);
                if (faceData.Templates.Count > 0)
                {
                    await device.Services.CreateUserSvc().AddFaceToUserAsync(deviceId, userId, faceData);
                    return Ok(new ApiResponse<object>(
                    data: null,
                    message: "Face assigned",
                    success: true

                    ));
                }



                return BadRequest(
                    new ApiResponse<object>(
                    data: null,
                    message: "Face scan failed",
                    success: false));

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

        [HttpDelete("{deviceId}/user/{userId}/cards")]
        public async Task<IActionResult> RemoveAllUserCards(uint deviceId, uint userId)
        {
            var device = CheckDevice.GetDeviceOrThrow(_manager, deviceId);

            try
            {
            await device.Services.CreateUserSvc().RemoveAllCardsFromUserAsync(deviceId, userId);

                return Ok(new ApiResponse<object>(
                             data: null,
                             message: "All cards removed",
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
    


    [HttpPost("{deviceId}/scan-card")]
        public async Task<IActionResult> ScanCard(uint deviceId)
        {
            var device = CheckDevice.GetDeviceOrThrow(_manager, deviceId);
            try
            {
                var cardData = await device.Services.CreateCardSvc().ScanCardAsync(deviceId);

                if (cardData == null || cardData.Length == 0)
                    return BadRequest(new ApiResponse<object>(
                    data: null,
                    message: "No card detected or scan failed",
                    success: false));
                        

                var cardHex = BitConverter.ToString(cardData).Replace("-", "");

                return Ok(new ApiResponse<object>(
                         data: new {deviceId,card = cardHex},
                         message: "card Scanned",
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