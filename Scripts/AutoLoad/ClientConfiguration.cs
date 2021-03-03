using ClientsUtils.Scripts.Configurations;

namespace GatewayServer.Scripts.AutoLoad
{
    public class ClientConfiguration : StandartClientConfiguration
    {
        private static ClientConfiguration _singleton;
        public static ClientConfiguration Singleton => _singleton;


        public ClientConfiguration() : base()
        {
            _singleton = this;
        }
    }
}