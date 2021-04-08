using SX.Common.Shared.Services;
using System;

namespace SX.Common.Shared.Models
{
    public class ServerConfig
    {
        public string Server { get; set; }
        public int Port { get; set; }
        public bool SSL { get; set; } = false;

        public string Login { get; set; }
        public string Password { get; set; }

        public virtual int DefaultPort { get { return 80; } }

        public ServerConfig() { }

        public ServerConfig(string connectionString)
        {
            var connectionParams = ParamValueCollection.Split(connectionString, ';');

            this.ParseConnectionStringParams(connectionParams);
        }

        protected virtual void ParseConnectionStringParams(ParamValueCollection parametres)
        {
            if (parametres != null)
            {
                this.Server = parametres.GetValue("Server") ?? "";

                if (parametres.Contains("SSL"))
                    this.SSL = parametres.GetText("SSL").Equals("true", CommonService.StringComparison);

                if (parametres.Contains("Port"))
                    this.Port = Convert.ToInt32(parametres.GetText("Port") ?? this.DefaultPort.ToString());

                this.Login = parametres.GetValue("Login") ?? "";

                this.Password = parametres.GetValue("Password") ?? "";
            }
        }
    }
}
