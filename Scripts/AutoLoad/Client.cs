using Godot;
using NightFallGatewayServer.Scripts.AutoLoad;
using static NightFallGatewayServer.Scripts.NodeExtensions;

namespace NightFallGatewayServer
{
    public sealed class Client : Node
    {

        private NetworkedMultiplayerENet clientPeer;

        // TODO: Move to external configuration
        private readonly string ipAddress = "192.168.100.103";
        // TODO: Move to external configuration
        private const int port = 4444;

        private static Client singleton;

        public static Client Singleton => singleton;

        public Client()
        {
            singleton = this;
            clientPeer = new NetworkedMultiplayerENet();
            // TODO: Buy a certificate signed by thirdparty
////#if DEBUG
////            clientPeer.DtlsVerify = false;
////#endif
            clientPeer.UseDtls = true;
            clientPeer.SetDtlsCertificate(ResourceLoader.Load<X509Certificate>("res://Resources/DTLS/basic.crt"));
        }

        public override void _EnterTree()
        {
            ConnectSignal(GetTree(), "connection_failed", this, nameof(ConnetionFailed));
            ConnectSignal(GetTree(), "connected_to_server", this, nameof(ConnectionSuccessful));
            ConnectSignal(GetTree(), "server_disconnected", this, nameof(ServerDisconnected));
        }

        public override void _Ready()
        {
            clientPeer.CreateClient(ipAddress, port);
            GetTree().NetworkPeer = clientPeer;
        }

        private void ConnetionFailed()
        {
            Logger.Client.Error($"Connection to {ipAddress}:{port} failed!");
        }

        private void ConnectionSuccessful()
        {
            Logger.Client.Debug($"Successfully conected to {ipAddress}:{port}");
        }

        private void ServerDisconnected()
        {
            Logger.Client.Warn($"Disconnected from {ipAddress}:{port}.");
        }
    }
}