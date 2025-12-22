using Gsdk.Display;

namespace Suprema_Api_Using_Protos.InterFaces
{
    public interface IDisplayService
    {
        Task<DisplayConfig> GetConfigAsync(uint deviceID);
        Task<bool> SetConfigAsync(uint deviceID, DisplayConfig config);
        Task<bool> UpdateSoundAsync(uint deviceID, SoundIndex index, string soundFilePath);
        Task<bool> SetVolumeAsync(uint deviceID, uint volume = 50);
        Task<bool> UpdateNoticeAsync(uint deviceID, string notice);
        Task<bool> SetDeviceBackgroundAsync(uint deviceID, string imagePath);
    }
}