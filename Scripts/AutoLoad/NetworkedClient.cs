using Godot;

using ClientsUtils.Scripts.Services;

namespace GatewayServer.Scripts.AutoLoad
{
    public sealed class NetworkedClient : NetworkedClientService
    {
        private static NetworkedClient _singleton;
        public static NetworkedClient Singleton => _singleton;

        private string _ipAddress;
        private int _port;

        public NetworkedClient()
        {
            _singleton = this;
            //clientPeer.SetDtlsCertificate(ResourceLoader.Load<X509Certificate>("res://Resources/DTLS/basic.crt"));
        }

        public override void _EnterTree()
        {
            base._EnterTree();

            GetTree().Connect("connection_failed", this, nameof(ConnetionFailed));
            GetTree().Connect("connected_to_server", this, nameof(ConnectionSuccessful));
            GetTree().Connect("server_disconnected", this, nameof(ServerDisconnected));
        }

        public override void _Ready()
        {
            _port = ClientConfiguration.Singleton.GetPort(4445);
            _ipAddress = ClientConfiguration.Singleton.GetIpAddress("192.168.100.103");
            CreateClient(_ipAddress, _port);
        }

        private void ConnetionFailed()
        {
            //Logger.Client.Error($"Connection to {_ipAddress}:{_port} failed!");
        }

        private void ConnectionSuccessful()
        {
            //Logger.Client.Debug($"Successfully conected to {_ipAddress}:{_port}");
        }

        private void ServerDisconnected()
        {
            //Logger.Client.Warn($"Disconnected from {_ipAddress}:{_port}.");
        }
    }
}