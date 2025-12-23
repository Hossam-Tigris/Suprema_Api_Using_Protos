using Grpc.Net.Client;
using Gsdk.Time;
using Suprema_Api_Using_Protos.InterFaces;

namespace Suprema_Api_Using_Protos.Services
{
    public class TimeSvc : ITimeService
    {
        private readonly Time.TimeClient TimeClient;
        private readonly uint _deviceId;

        public TimeSvc(GrpcChannel channel, uint deviceId)
        {
            TimeClient = new Time.TimeClient(channel);
            _deviceId = deviceId;
        }

        public async Task<DateTime> GetDeviceTimeAsync(uint deviceID)
        {
            var request = new GetRequest
            {
                DeviceID = deviceID
            };

            var response = await TimeClient.GetAsync(request);
            long unixTime = (long)response.GMTTime;

            return DateTimeOffset.FromUnixTimeSeconds(unixTime).UtcDateTime;
        }

        public async Task<bool> SetSpecificTimeAsync(uint deviceID, int hour, int minute = 0, int second = 0)
        {
            DateTime today = DateTime.Today;

            DateTime targetTime = new DateTime(
                today.Year,
                today.Month,
                today.Day,
                hour,
                minute,
                second,
                DateTimeKind.Local
            );

            long unixNow = new DateTimeOffset(targetTime.ToUniversalTime()).ToUnixTimeSeconds();

            var request = new SetRequest
            {
                DeviceID = deviceID,
                GMTTime = (ulong)unixNow
            };

            try
            {
                var response = await TimeClient.SetAsync(request);

                if (response != null)
                {
                    Console.WriteLine($"Time set successfully to {targetTime}");
                    return true;
                }
                else
                {
                    Console.WriteLine($"Failed to set time. ResultCode:");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error setting device time: {ex.Message}");
                return false;
            }
        }


    }
}

