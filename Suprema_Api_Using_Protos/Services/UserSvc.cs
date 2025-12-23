using Google.Protobuf;
using Grpc.Net.Client;
using Gsdk.Card;
using Gsdk.Face;
using Gsdk.Finger;
using Gsdk.User;
using Suprema_Api_Using_Protos.InterFaces;
using static Gsdk.User.User;

namespace Suprema_Api_Using_Protos.Services
{
    public class UserSvc : IUserService
    {
        private readonly UserClient UserClient;
        private readonly uint _deviceId;

        public UserSvc(GrpcChannel channel, uint deviceId)
        {
            UserClient = new User.UserClient(channel);
            _deviceId = deviceId;
        }
        public async Task<bool> AddUserAsync(uint deviceID, uint userID, string name)
        {
            var user = new UserInfo
            {
                Hdr = new UserHdr
                {
                    ID = userID.ToString()
                },
                Name = name,
                Photo = Google.Protobuf.ByteString.CopyFrom(File.ReadAllBytes("Assets/userphoto.bmp"))

            };

            var request = new EnrollRequest
            {
                DeviceID = deviceID,
                Users = { user },
                Overwrite = true
            };

            try
            {
                await UserClient.EnrollAsync(request);
                Console.WriteLine($"User {name} added successfully!");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding user: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DeleteUserAsync(uint deviceID, uint userID)
        {
            var request = new Gsdk.User.DeleteRequest
            {
                DeviceID = deviceID,
                UserIDs = { userID.ToString() }
            };

            try
            {
                await UserClient.DeleteAsync(request);
                Console.WriteLine($"User {userID} deleted successfully!");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting user: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> UpdateUserAsync(uint deviceID, uint userID, string newName)
        {
            var user = new UserInfo
            {
                Hdr = new UserHdr
                {
                    ID = userID.ToString()
                },
                Name = newName,
                Photo = Google.Protobuf.ByteString.CopyFrom(File.ReadAllBytes("Assets/userphoto.bmp"))

            };

            var request = new EnrollRequest
            {
                DeviceID = deviceID,
                Users = { user },
                Overwrite = true
            };

            try
            {
                await UserClient.EnrollAsync(request);
                Console.WriteLine($"User {userID} updated successfully!");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating user: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> AddCardAsync(uint deviceID, uint userID, byte[] cardBytes)
        {
            var userCard = new UserCard
            {
                UserID = userID.ToString(),
                Cards =
        {
            new CSNCardData
            {
                Type = Gsdk.Card.Type.CardTypeCsn,
                Data = Google.Protobuf.ByteString.CopyFrom(cardBytes),
                Size = cardBytes.Length
            }
        }
            };

            var request = new SetCardRequest
            {
                DeviceID = deviceID,
                UserCards = { userCard }
            };

            try
            {
                await UserClient.SetCardAsync(request);
                Console.WriteLine($"Card added to user {userID} successfully!");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($" Error adding card: {ex.Message}");
                return false;
            }
        }


        public async Task<bool> AddFaceToUserAsync(uint deviceID, uint userID, Gsdk.Face.FaceData faceData)
        {
            var userFace = new UserFace
            {
                UserID = userID.ToString(),
                Faces =
        {
            new FaceData
            {
                Index = faceData.Index,
                Flag = faceData.Flag,
                Templates = { faceData.Templates },
                ImageData = faceData.ImageData
            }
        }
            };

            var request = new SetFaceRequest
            {
                DeviceID = deviceID,
                UserFaces = { userFace }
            };

            try
            {
                await UserClient.SetFaceAsync(request);
                Console.WriteLine($"Face assigned to user {userID} successfully!");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error assigning face: {ex.Message}");
                return false;
            }
        }

        // لما الcard متبعتهاش او لو بعتها او بعتها فاضيه  بيمسح  الcards بتاعت اليوزر دا 
        public async Task<bool> RemoveAllCardsFromUserAsync(uint deviceID, uint userID)
        {
            var userCard = new UserCard
            {
                UserID = userID.ToString(),
                //Cards
            };

            var request = new SetCardRequest
            {
                DeviceID = deviceID,
                UserCards = { userCard }
            };

            try
            {
                await UserClient.SetCardAsync(request);
                Console.WriteLine($"All cards removed from user {userID} successfully!");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to remove cards from user {userID}: {ex.Message}");
                return false;
            }
        }


        public async Task<bool> ScanVerifyAndAssignFingerAsync(uint deviceID, uint userID, FingerSvc fingerSvc)
        {
            try
            {
                var templates = new List<Google.Protobuf.ByteString>();

                // ===== Scan #1 =====
                Console.WriteLine("Scan finger (1/2)...");
                var scan1 = await fingerSvc.ScanAsync(new Gsdk.Finger.ScanRequest
                {
                    DeviceID = deviceID,
                    TemplateFormat = TemplateFormat.Iso,
                    QualityThreshold = 80
                });

                if (scan1.QualityScore < 80)
                    throw new Exception("Low quality on first scan");

                templates.Add(scan1.TemplateData);

                // ===== Scan #2 =====
                Console.WriteLine("Scan finger again (2/2)...");
                var scan2 = await fingerSvc.ScanAsync(new Gsdk.Finger.ScanRequest
                {
                    DeviceID = deviceID,
                    TemplateFormat = TemplateFormat.Iso,
                    QualityThreshold = 80
                });

                if (scan2.QualityScore < 80)
                    throw new Exception("Low quality on second scan");

                templates.Add(scan2.TemplateData);

                // ===== Verify =====
                var verifyFinger = new FingerData
                {
                    Index = 0,
                    Flag = 1
                };
                verifyFinger.Templates.AddRange(templates);

                await fingerSvc.VerifyAsync(deviceID, verifyFinger);

                // ===== Assign =====
                var enrollFinger = new FingerData
                {
                    Index = 0,
                    Flag = 0
                };
                enrollFinger.Templates.AddRange(templates);

                var userFinger = new UserFinger
                {
                    UserID = userID.ToString(),
                    Fingers = { enrollFinger }
                };

                await UserClient.SetFingerAsync(new SetFingerRequest
                {
                    DeviceID = deviceID,
                    UserFingers = { userFinger }
                });

                Console.WriteLine("Finger enrolled successfully");
                return true;
            }
            catch (Grpc.Core.RpcException ex)
            {
                Console.WriteLine($"gRPC Error: {ex.Status.Detail}");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return false;
            }
        }

        public async Task<byte[]> GetPinHashAsync(string pin)
        {
            var response = await UserClient.GetPINHashAsync(
                new GetPINHashRequest { PIN = pin }
            );

            return System.Text.Encoding.UTF8.GetBytes(response.HashVal);
        }

        public async Task<bool> SetUserPinAsync(uint deviceID, uint userID, string pin)
        {
            try
            {
                var getResp = await UserClient.GetAsync(new Gsdk.User.GetRequest
                {
                    DeviceID = deviceID,
                    UserIDs = { userID.ToString() }
                });

                if (getResp.Users.Count == 0)
                    throw new Exception("User not found");

                var user = getResp.Users[0];

                var pinHashResp = await UserClient.GetPINHashAsync(
                    new GetPINHashRequest { PIN = pin }
                );

                byte[] pinBytes = Convert.FromHexString(pinHashResp.HashVal);

                user.PIN = ByteString.CopyFrom(pinBytes);

                await UserClient.EnrollAsync(new EnrollRequest
                {
                    DeviceID = deviceID,
                    Users = { user },
                    Overwrite = true
                });

                Console.WriteLine($"PIN set successfully for user {userID}");
                return true;
            }
            catch (Grpc.Core.RpcException ex)
            {
                Console.WriteLine($"gRPC Error: {ex.Status.Detail}");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return false;
            }
        }
        public async Task<UserInfo> GetAsync(uint userId)
        {
            var resp = await UserClient.GetAsync(new Gsdk.User.GetRequest
            {
                DeviceID = _deviceId,
                UserIDs = { userId.ToString() }
            });

            return resp.Users.Count > 0 ? resp.Users[0] : null;
        }



        // public async Task<bool> SyncUseridToDevice(DeviceSession source, DeviceSession target, uint userID)
        // {
        //    var getResp = await source.Services
        //        .CreateUserSvc()
        //        .GetAsync(userID);

        //    if (getResp == null)
        //        throw new Exception("User not found on source device");

        //    await target.Services
        //        .CreateUserSvc()
        //        .AddSyncUserAsync(target.DeviceID, getResp);

        //    return true;
        //}






        public async Task<bool> AddSyncUserAsync(uint deviceID, UserInfo user)
        {


            var request = new EnrollRequest
            {
                DeviceID = deviceID,
                Users = { user },
                Overwrite = true
            };

            try
            {
                await UserClient.EnrollAsync(request);
                Console.WriteLine($"User {user.Name} added successfully to {deviceID}!");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding user: {ex.Message}");
                return false;
            }
        }

    }
}



