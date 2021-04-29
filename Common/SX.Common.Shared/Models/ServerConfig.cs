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

        public virtual int DefaultPort => 80;

        public ServerConfig() { }

        public ServerConfig(string connectionString)
        {
            var connectionParams = ParamValueCollection.Split(connectionString, ';');

            this.ParseConnectionStringParams(connectionParams);
        }

        protected virtual void ParseConnectionStringParams(ParamValueCollection parametres)
        {
            if (parametres == null)
                return;

            this.Server = parametres.GetValue("Server") ?? "";

            if (parametres.Contains("SSL"))
            {
                var value = parametres.GetText("SSL");
                this.SSL = !String.IsNullOrWhiteSpace(value) && value.Equals("true", CommonService.StringComparison);
            }

            if (parametres.Contains("Port"))
            {
                var value = parametres.GetText("Port");
                this.Port = String.IsNullOrWhiteSpace(value) ? this.DefaultPort : Convert.ToInt32(value);
            }

            this.Login = parametres.GetValue("Login") ?? "";

            this.Password = parametres.GetValue("Password") ?? "";
        }
    }
}
