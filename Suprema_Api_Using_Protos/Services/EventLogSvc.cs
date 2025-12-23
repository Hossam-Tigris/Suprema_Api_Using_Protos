using Grpc.Core;
using Grpc.Net.Client;
using Gsdk.Event;
using Suprema_Api_Using_Protos.InterFaces;

namespace Suprema_Api_Using_Protos.Services
{

    public class EventLogSvc : IEventLogService
    {
        private readonly Event.EventClient EventClient;
        private readonly uint _deviceId;


        public EventLogSvc(GrpcChannel channel, uint deviceId)
        {
            EventClient = new Event.EventClient(channel);
            _deviceId = deviceId;
        }


        public async Task<GetLogResponse> GetLogsAsync(uint deviceID, uint startEventID = 0, uint maxNumOfLog = 50, string filePath = "DeviceLogs.txt")
        {
            var request = new GetLogRequest
            {
                DeviceID = deviceID,
                StartEventID = startEventID,
                MaxNumOfLog = maxNumOfLog
            };

            try
            {
                var response = await EventClient.GetLogAsync(request);

                Console.WriteLine($"Last {maxNumOfLog} logs from device {deviceID}:");

                using (var writer = new StreamWriter(filePath, append: true))
                {
                    foreach (var log in response.Events)
                    {
                        DateTime logTime = DateTimeOffset.FromUnixTimeSeconds(log.Timestamp).LocalDateTime;

                        string logLine = $" EventID: {log.ID}, UserID: {log.UserID}, EventCode: {log.EventCode}, Time: {logTime}, DeviceID: {log.DeviceID}";
                        Console.WriteLine(logLine);
                        await writer.WriteLineAsync(logLine);
                    }
                }

                Console.WriteLine($"Logs also saved to file: {filePath}");
                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching logs: {ex.Message}");
                return null;
            }
        }




        public async Task SubscribeAsync(Action<EventLog> onEventReceived)
        {
            var request = new SubscribeRealtimeLogRequest
            {
                QueueSize = 10
            };
            request.DeviceIDs.Add(_deviceId);

            using var call = EventClient.SubscribeRealtimeLog(request);

            while (await call.ResponseStream.MoveNext())
            {
                var evt = call.ResponseStream.Current;
                onEventReceived?.Invoke(evt);
            }
        }

        public void EnableMonitoring(uint deviceID)
        {
            EventClient.EnableMonitoring(new EnableMonitoringRequest { DeviceID = deviceID });
        }

        public void EnableMonitoringMulti(IEnumerable<uint> deviceIDs)
        {
            foreach (var id in deviceIDs)
            {
                EventClient.EnableMonitoring(new EnableMonitoringRequest { DeviceID = id });
            }
        }

        public void StartMonitoringSignInMulti(Action<uint, uint> onUserSignIn, IEnumerable<uint> deviceIDs)
        {
            foreach (var id in deviceIDs)
            {
                Task.Run(async () =>
                {
                    var request = new SubscribeRealtimeLogRequest
                    {
                        QueueSize = 10
                    };
                    request.DeviceIDs.Add(id);

                    using var call = EventClient.SubscribeRealtimeLog(request);

                    while (await call.ResponseStream.MoveNext())
                    {
                        var evt = call.ResponseStream.Current;

                        Console.WriteLine($" deviceId={evt.DeviceID} , UserID={evt.UserID},Eventid={evt.ID} ");
                        if (evt.EventCode == 1)
                        {
                            uint userID = uint.Parse(evt.UserID);
                            onUserSignIn?.Invoke(userID, id);
                        }
                    }
                });
            }
        }

    }
}
