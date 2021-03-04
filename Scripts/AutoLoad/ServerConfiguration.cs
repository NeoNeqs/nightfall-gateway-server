using ServersUtils.Scripts.Configurations;

namespace GatewayServer.Scripts.AutoLoad
{
    public class ServerConfiguration : StandartServerConfiguration
    {
        private static ServerConfiguration _singleton;
        public static ServerConfiguration Singleton => _singleton;


        private ServerConfiguration() : base()
        {
            _singleton = this;
        }
    }
}