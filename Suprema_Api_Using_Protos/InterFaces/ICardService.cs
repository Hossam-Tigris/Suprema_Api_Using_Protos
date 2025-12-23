namespace Suprema_Api_Using_Protos.InterFaces
{
    public interface ICardService
    {
        Task<byte[]> ScanCardAsync(uint deviceID);
        Task EraseAllCardsAsync(uint deviceID);
    }
}
