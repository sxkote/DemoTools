using Newtonsoft.Json;
using SX.Common.Domain.Entities;
using SX.Common.Shared.Classes.Json;
using SX.Common.Shared.Exceptions;
using SX.Common.Shared.Interfaces;
using SX.Common.Shared.Services;
using System;

namespace DemoTools.Core.Domain.Entities
{
    public class AuthToken : EntityGuid
    {
        public Guid UserID { get; protected set; }
        public DateTimeOffset? Expire { get; private set; }
        public string Data { get; private set; }


        protected AuthToken() { }

        public override string ToString()
        {
            return ID.ToString();
        }

        public void Cancel()
        {
            this.Expire = CommonService.Now;
        }

        public T GetToken<T>()
            where T : class, IToken
        {
            if (String.IsNullOrWhiteSpace(this.Data))
                return null;

            return JsonConvert.DeserializeObject<T>(this.Data, new JsonSerializerSettings()
            {
                ContractResolver = new JsonPrivateContractResolver(),
                Converters = { new JsonCustomValueConverter() }
            });
        }

        static public AuthToken Create(IToken token)
        {
            if (token == null)
                throw new CustomArgumentException("Не задан токен для генерации авторизационной записи!");

            return new AuthToken()
            {
                ID = token.TokenID,
                UserID = token.UserID,
                Expire = token.Expire,
                Data = Serialize(token)
            };
        }

        static private string Serialize(IToken token)
        {
            return JsonConvert.SerializeObject(token, Formatting.Indented, new JsonCustomValueConverter());
        }
    }
}
