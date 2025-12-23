using Gsdk.Face;
using Suprema_Api_Using_Protos.Services;

namespace Suprema_Api_Using_Protos.InterFaces
{
    public interface IUserService
    {
        Task<bool> AddUserAsync(uint deviceID, uint userID, string name);
        Task<bool> DeleteUserAsync(uint deviceID, uint userID);
        Task<bool> UpdateUserAsync(uint deviceID, uint userID, string newName);

        Task<bool> AddCardAsync(uint deviceID, uint userID, byte[] cardBytes);
        Task<bool> RemoveAllCardsFromUserAsync(uint deviceID, uint userID);

        Task<bool> AddFaceToUserAsync(uint deviceID, uint userID, FaceData faceData);
        Task<bool> ScanVerifyAndAssignFingerAsync(uint deviceID, uint userID, FingerSvc fingerService);
        Task<bool> SetUserPinAsync(uint deviceID, uint userID, string pin);
    }

}
