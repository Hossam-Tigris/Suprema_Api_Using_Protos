using Grpc.Net.Client;
using Gsdk.Card;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Suprema_Api_Using_Protos.InterFaces;

namespace Suprema_Api_Using_Protos.Services
{
    public class CardSvc : ICardService
    {
        private readonly Card.CardClient cardClient;
        private readonly uint _deviceId;


        public CardSvc(GrpcChannel channel, uint deviceId)
        {
            cardClient = new Card.CardClient(channel);
            _deviceId = deviceId;
        }

        public async Task<byte[]> ScanCardAsync(uint deviceID)
        {
            Console.WriteLine("Please put the card on the device...");

            var response = await cardClient.ScanAsync(
                new ScanRequest { DeviceID = deviceID }
            );

            var csn = response.CardData.CSNCardData;

            if (csn == null || csn.Data == null || csn.Data.Length == 0)
                throw new Exception("Invalid CSN card data");

            byte[] cardBytes = csn.Data.ToByteArray();

            Console.WriteLine("Card scanned successfully");
            Console.WriteLine($"Type : {csn.Type}");
            Console.WriteLine($"Size : {csn.Size}");
            Console.WriteLine($"Card ID (HEX): {BitConverter.ToString(cardBytes).Replace("-", "")}");

            return cardBytes;
        }
        public async Task EraseAllCardsAsync(uint deviceID)
        {
            try
            {
                await cardClient.EraseAsync(
                    new EraseRequest { DeviceID = deviceID }
                );

                Console.WriteLine(" All cards erased from device successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($" Failed to erase cards: {ex.Message}");
            }
        }
    }
}