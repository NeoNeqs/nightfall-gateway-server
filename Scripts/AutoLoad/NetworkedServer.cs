using ServersUtils.Scripts.Exceptions;
using ServersUtils.Scripts.Logging;
using ServersUtils.Scripts.Services;

using SharedUtils.Scripts.Common;

namespace GatewayServer.Scripts.AutoLoad
{
    public sealed class NetworkedServer : NetworkedServerService
    {
        private static NetworkedServer _singleton;
        public static NetworkedServer Singleton => _singleton;


        private NetworkedServer() : base()
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
            var port = ServerConfiguration.Singleton.GetPort(4445);
            var maxClients = ServerConfiguration.Singleton.GetMaxClients(100);
            var error = CreateServer(port, maxClients);
            if (error != ErrorCode.Ok)
            {
                throw new CantCreateServerException(port);
            }
        }

        protected override void PeerConnected(int id)
        {
            ServerLogger.GetSingleton().Info($"Peer {id} has connected");
        }

        protected override void PeerDisconnected(int id)
        {
            ServerLogger.GetSingleton().Info($"Peer {id} has disconnected");
        }

        protected override string GetCertificateName()
        {
            return "cg.crt";
        }

        protected override string GetCryptoKeyName()
        {
            return "cg.key";
        }
    }
}