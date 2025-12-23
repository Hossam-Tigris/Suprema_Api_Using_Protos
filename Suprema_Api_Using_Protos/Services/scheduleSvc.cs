using Grpc.Net.Client;
using Gsdk.Schedule;
using Suprema_Api_Using_Protos.InterFaces;

namespace Suprema_Api_Using_Protos.Services
{
    public class ScheduleSvc : IScheduleService
    {
        private readonly Schedule.ScheduleClient _client;

        public ScheduleSvc(GrpcChannel channel)
        {
            _client = new Schedule.ScheduleClient(channel);
        }

        public async Task AddWeeklyAsync(uint deviceId, uint scheduleId, string name, Dictionary<int, List<(int start, int end)>> days)
        {
            var weekly = new WeeklySchedule();

            for (int i = 0; i < 7; i++)
            {
                var day = new DaySchedule();

                if (days.ContainsKey(i))
                {
                    foreach (var p in days[i])
                    {
                        day.Periods.Add(new TimePeriod
                        {
                            StartTime = p.start,
                            EndTime = p.end
                        });
                    }
                }

                weekly.DaySchedules.Add(day);
            }

            var schedule = new ScheduleInfo
            {
                ID = scheduleId,
                Name = name,
                Weekly = weekly
            };

            await _client.AddAsync(new AddRequest
            {
                DeviceID = deviceId,
                Schedules = { schedule }
            });
        }

        public async Task<GetListResponse> GetListAsync(uint deviceId)
        {
            return await _client.GetListAsync(new GetListRequest
            {
                DeviceID = deviceId
            });
        }
    }
}