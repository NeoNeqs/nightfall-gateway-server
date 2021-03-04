using Godot;

using ClientsUtils.Scripts.Exceptions;
using ClientsUtils.Scripts.Logging;
using ClientsUtils.Scripts.Services;

namespace GatewayServer.Scripts.AutoLoad
{
    public sealed class NetworkedClient : NetworkedClientService
    {
        private static NetworkedClient _singleton;
        public static NetworkedClient Singleton => _singleton;
        private string _ipAddress;
        private int _port;


        public NetworkedClient() : base()
        {
            _singleton = this;
        }

        public override void _EnterTree()
        {
            base._EnterTree();
            ConnectSignals(this);
        }

        public override void _Ready()
        {
            _port = ClientConfiguration.Singleton.GetPort(4444);
            _ipAddress = ClientConfiguration.Singleton.GetIpAddress("localhost");
            var creationError = CreateClient(_ipAddress, _port);
            if (creationError != Error.Ok)
                throw new CantConnectToServerException(_ipAddress, _port);
        }

        protected override void ConnetionFailed()
        {
            ClientLogger.GetSingleton().Error($"Connection to {_ipAddress}:{_port} failed!");
        }

        protected override void ConnectionSuccessful()
        {
            ClientLogger.GetSingleton().Verbose($"Successfully conected to {_ipAddress}:{_port}");
        }

        protected override void ServerDisconnected()
        {
            ClientLogger.GetSingleton().Warn($"Disconnected from {_ipAddress}:{_port}.");
        }

        protected override string GetCryptoKeyName()
        {
            throw new System.NotSupportedException("CryptoKey can't be used with a client.");
        }

        protected override string GetCertificateName()
        {
            return "ag.crt";
        }
    }
}