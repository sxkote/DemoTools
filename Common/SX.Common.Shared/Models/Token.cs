using SX.Common.Shared.Interfaces;
using System;
using System.Linq;

namespace SX.Common.Shared.Models
{
    public class Token : IToken
    {
        public const string ROLE_ADMINISTRATOR = "Administrator";

        public Guid TokenID { get; set; }
        public Guid UserID { get; set; }
        public Guid SubscriptionID { get; set; }
        public string Login { get; set; }
        public string IP { get; set; }
        public string Name { get; set; }
        public string Avatar { get; set; }

        public DateTimeOffset? Date { get; set; }
        public DateTimeOffset? Expire { get; set; }

        public string[] Roles { get; set; }
        public string[] Permissions { get; set; }

        public ParamValueCollection Params { get; set; } = new ParamValueCollection();

        public Token()
        {
            this.TokenID = Guid.NewGuid();
            this.Date = this.Expire = DateTimeOffset.Now;
        }

        public CustomValue this[string name]
        {
            get { return this.GetParam(name)?.Value; }
            set { this.SetParam(name, value); }
        }

        public ParamValue GetParam(string name)
        {
            return this.Params[name];
        }

        public void SetParam(string name, CustomValue value)
        {
            this.Params.Set(new ParamValue(name, value));
        }


        public override string ToString()
        {
            return this.TokenID.ToString();
        }

        public virtual bool IsValid()
        {
            var now = DateTimeOffset.Now;

            if (this.TokenID == Guid.Empty || this.UserID == Guid.Empty)
                return false;

            if (this.Date != null && now < this.Date.Value)
                return false;

            if (this.Expire != null && now > this.Expire.Value)
                return false;

            return true;
        }

        public bool IsInRole(string role)
        {
            if (String.IsNullOrEmpty(role))
                return true;

            if (this.Roles == null)
                return false;

            return this.Roles.Any(r => r.Equals(role, StringComparison.OrdinalIgnoreCase));
        }

        public bool HasPermission(string permisssion)
        {
            if (String.IsNullOrEmpty(permisssion))
                return true;

            if (this.IsAdministrator())
                return true;

            if (this.Permissions == null)
                return false;

            return this.Permissions.Any(p => p.Equals(permisssion, StringComparison.OrdinalIgnoreCase));
        }

        public bool IsAdministrator() => this.IsInRole(Token.ROLE_ADMINISTRATOR);
    }
}
