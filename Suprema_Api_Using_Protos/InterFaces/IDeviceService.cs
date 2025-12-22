namespace Suprema_Api_Using_Protos.InterFaces
{
    public interface IDeviceService
    {
        Task RebootDeviceAsync(uint deviceID);
        Task FactoryResetAsync(uint deviceID);
    }
}
