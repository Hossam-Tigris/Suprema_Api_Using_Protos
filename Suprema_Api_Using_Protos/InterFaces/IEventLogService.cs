using Gsdk.Event;

namespace Suprema_Api_Using_Protos.InterFaces
{
    public interface IEventLogService
    {
        Task<GetLogResponse> GetLogsAsync(uint deviceID, uint startEventID = 0, uint maxNumOfLog = 50, string filePath = "DeviceLogs.txt");
    }

}
