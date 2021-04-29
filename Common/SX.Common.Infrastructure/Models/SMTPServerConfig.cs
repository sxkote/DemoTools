using SX.Common.Shared.Models;
using SX.Common.Shared.Services;

namespace SX.Common.Infrastructure.Models
{
    public class SMTPServerConfig : ServerConfig
    {
        public const string CONFIG_NAME = "SMTPServerConfig";

        public const string DEFAULT_SERVER_ALIAS = "default";
        public const int DEFAULT_PORT = 25; //587
        public const int DEFAULT_PORT_SSL = 465;

        static public SMTPServerConfig Default => new SMTPServerConfig() { Server = DEFAULT_SERVER_ALIAS };


        public string From { get; set; }

        public override int DefaultPort => this.SSL ? DEFAULT_PORT_SSL : DEFAULT_PORT;

        protected SMTPServerConfig() { }

        public SMTPServerConfig(string connectionString)
            : base(connectionString) { }

        public bool IsDefault()
        {
            return this.Server.Equals(DEFAULT_SERVER_ALIAS, CommonService.StringComparison);
        }

        protected override void ParseConnectionStringParams(ParamValueCollection parametres)
        {
            base.ParseConnectionStringParams(parametres);

            if (parametres != null)
            {
                this.From = parametres.GetValue("From") ?? "";
            }
        }
    }
}
