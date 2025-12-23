using Gsdk.Schedule;

namespace Suprema_Api_Using_Protos.InterFaces
{
    public interface IScheduleService
    {
        Task AddWeeklyAsync(uint deviceId, uint scheduleId, string name, Dictionary<int, List<(int start, int end)>> days);
        Task<GetListResponse> GetListAsync(uint deviceId);
    }
}
