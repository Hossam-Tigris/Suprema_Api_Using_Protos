using Suprema_Api_Using_Protos.Services;

namespace Suprema_Api_Using_Protos.Helper
{
    public static class CheckDevice
    {
        public static DeviceSession GetDeviceOrThrow(
            DeviceManager manager,
            uint deviceId)
        {
            var device = manager.GetDevice(deviceId);
            if (device == null)
                throw new KeyNotFoundException("Device not connected");

            return device;
        }
    }
}
