namespace Suprema_Api_Using_Protos.InterFaces
{
    public interface ITimeService
    {
        Task<DateTime> GetDeviceTimeAsync(uint deviceID);
        Task<bool> SetSpecificTimeAsync(uint deviceID, int hour, int minute = 0, int second = 0);
    }

}
