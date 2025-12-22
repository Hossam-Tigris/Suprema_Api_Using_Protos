using Grpc.Net.Client;

namespace Suprema_Api_Using_Protos.Services
{
    public class GatewayClient
    {
        public GrpcChannel Channel { get; private set; }

        public void Connect(string caPath, string addr, int port)
        {
            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback =
                HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;

            Channel = GrpcChannel.ForAddress(
                $"https://{addr}:{port}",
                new GrpcChannelOptions { HttpHandler = handler }
            );
        }
    }
}
