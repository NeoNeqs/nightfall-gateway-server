using Godot;

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
            var error = SetupDTLS("user://DTLS");
            if (error != Error.Ok)
            {
                ClientLogger.GetLogger().Error($"An error has occurred while setting up DTLS. Error: {error}");
            }
            GetTree().Connect("connection_failed", this, nameof(ConnetionFailed));
            GetTree().Connect("connected_to_server", this, nameof(ConnectionSuccessful));
            GetTree().Connect("server_disconnected", this, nameof(ServerDisconnected));
        }

        public override void _Ready()
        {
            _port = ClientConfiguration.Singleton.GetPort(4444);
            _ipAddress = ClientConfiguration.Singleton.GetIpAddress("localhost");
            var creationError = CreateClient(_ipAddress, _port);
            if (creationError != Error.Ok)
            {
                ClientLogger.GetLogger().Error($"Could not connect to {_ipAddress}:{_port}");
                GetTree().Quit(-(int)creationError);
            }
        }

        private void ConnetionFailed()
        {
            ClientLogger.GetLogger().Error($"Connection to {_ipAddress}:{_port} failed!");
        }

        private void ConnectionSuccessful()
        {
            ClientLogger.GetLogger().Verbose($"Successfully conected to {_ipAddress}:{_port}");
        }

        private void ServerDisconnected()
        {
            ClientLogger.GetLogger().Warn($"Disconnected from {_ipAddress}:{_port}.");
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