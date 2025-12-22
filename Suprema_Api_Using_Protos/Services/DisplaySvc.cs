using Google.Protobuf;
using Grpc.Net.Client;
using Gsdk.Display;
using Suprema_Api_Using_Protos.InterFaces;

namespace Suprema_Api_Using_Protos.Services
{
    public class DisplaySvc : IDisplayService
    {
        private readonly Display.DisplayClient DisplayClient;
        private readonly uint _deviceId;


        public DisplaySvc(GrpcChannel channel, uint deviceId)
        {
            DisplayClient = new Gsdk.Display.Display.DisplayClient(channel);
            _deviceId = deviceId;
        }

        public async Task<bool> SetDeviceBackgroundAsync(uint deviceID, string imagePath)
        {
            if (!File.Exists(imagePath))
            {
                Console.WriteLine($"Image file not found: {imagePath}");
                return false;
            }

            byte[] imageBytes = File.ReadAllBytes(imagePath);

            var request = new UpdateBackgroundImageRequest
            {
                DeviceID = deviceID,
                PNGImage = ByteString.CopyFrom(imageBytes)
            };

            try
            {
                var response = await DisplayClient.UpdateBackgroundImageAsync(request);

                if (response != null)
                {
                    Console.WriteLine("Background updated successfully!");
                    return true;
                }
                else
                {
                    Console.WriteLine($"Failed to update background. ResultCode:0");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error setting background: {ex.Message}");
                return false;
            }
        }

        public async Task<DisplayConfig> GetConfigAsync(uint deviceID)
        {
            var request = new GetConfigRequest { DeviceID = deviceID };

            var response = await DisplayClient.GetConfigAsync(request);
            return response.Config;
        }

        public async Task<bool> SetConfigAsync(uint deviceID, DisplayConfig config)
        {
            var request = new SetConfigRequest
            {
                DeviceID = deviceID,
                Config = config
            };

            try
            {
                var response = await DisplayClient.SetConfigAsync(request);
                Console.WriteLine("Device configuration updated successfully!");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to set device config: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> UpdateSoundAsync(uint deviceID, SoundIndex index, string soundFilePath)
        {
            if (!File.Exists(soundFilePath))
            {
                Console.WriteLine($"Sound file not found: {soundFilePath}");
                return false;
            }

            byte[] waveData = File.ReadAllBytes(soundFilePath);

            var request = new UpdateSoundRequest
            {
                DeviceID = deviceID,
                Index = index,
                WaveData = ByteString.CopyFrom(waveData)
            };

            try
            {
                var response = await DisplayClient.UpdateSoundAsync(request);
                Console.WriteLine($"Sound {index} updated successfully!");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating sound {index}: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> SetVolumeAsync(uint deviceID, uint volume = 50)
        {
            if (volume > 100) volume = 100;

            var config = await GetConfigAsync(deviceID);
            config.Volume = volume;

            return await SetConfigAsync(deviceID, config);
        }


        public async Task<bool> UpdateNoticeAsync(uint deviceID, string notice)
        {
            if (string.IsNullOrWhiteSpace(notice))
            {
                Console.WriteLine("Notice message is empty.");
                return false;
            }

            var request = new UpdateNoticeRequest
            {
                DeviceID = deviceID,
                Notice = notice
            };

            try
            {
                var response = await DisplayClient.UpdateNoticeAsync(request);
                Console.WriteLine("Notice updated successfully!");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating notice: {ex.Message}");
                return false;
            }
        }
    }
}