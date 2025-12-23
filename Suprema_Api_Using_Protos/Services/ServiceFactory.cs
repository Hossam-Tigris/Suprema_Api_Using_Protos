using Grpc.Net.Client;

namespace Suprema_Api_Using_Protos.Services
{
    public class ServiceFactory
    {
        private readonly GrpcChannel _channel;

        private readonly uint _deviceId;

        public ServiceFactory(GrpcChannel channel, uint deviceId)
        {
            _channel = channel;
            _deviceId = deviceId;
        }

        public UserSvc CreateUserSvc()
            => new UserSvc(_channel, _deviceId);

        public FingerSvc CreateFingerSvc()
            => new FingerSvc(_channel, _deviceId);

        public CardSvc CreateCardSvc()
            => new CardSvc(_channel, _deviceId);

        public FaceSvc CreateFaceSvc()
            => new FaceSvc(_channel, _deviceId);

        public TimeSvc CreateTimeSvc()
            => new TimeSvc(_channel, _deviceId);

        public DeviceSvc CreateDeviceSvc()
            => new DeviceSvc(_channel, _deviceId);

        public EventLogSvc CreateEventLogSvc()
            => new EventLogSvc(_channel, _deviceId);

        public DisplaySvc CreateDisplaySvc()
            => new DisplaySvc(_channel, _deviceId);
        public ScheduleSvc CreateScheduleSvc()
        => new ScheduleSvc(_channel);

        public AuthSvc CreateAuthSvc()
            => new AuthSvc(_channel);
    }
}

