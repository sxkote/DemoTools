using SX.Common.Domain.Entities;
using SX.Common.Shared.Exceptions;
using SX.Common.Shared.Services;
using System;

namespace DemoTools.Core.Domain.Entities
{
    public class Activity : EntityGuid
    {
        public const int DEFAULT_EXPIRE_MINUTES = 24 * 60; // 24 hours

        public Guid? PersonID { get; private set; }
        public DateTimeOffset Date { get; protected set; }
        public DateTimeOffset Expire { get; protected set; }
        public string Type { get; protected set; }
        public string Data { get; protected set; }
        public string Pin { get; protected set; }

        protected Activity()
        {
            this.Date = this.Expire = CommonService.Now;
            this.Type = "";
            this.Data = "";
            this.Pin = "";
        }

        public T GetObject<T>()
        {
            if (String.IsNullOrEmpty(this.Data))
                return default(T);

            return CommonService.Deserialize<T>(this.Data);
        }

        public void ChangeObject<T>(T data)
        {
            if (data == null)
                throw new CustomArgumentException("Activity Data is empty!");

            this.Data = CommonService.Serialize(data);
        }

        public void ChangePin(string pin)
        {
            if (!string.IsNullOrWhiteSpace(this.Pin))
                throw new CustomExecutionException("Activity Pin is already exists!");

            this.Pin = pin;
        }

        public bool CheckPin(string pin)
        {
            return this.Pin.Equals(pin, CommonService.StringComparison);
        }

        public bool IsValid()
        {
            var now = CommonService.Now;
            return this.Date <= now && now <= this.Expire;
        }

        public bool IsValid(Guid personID)
        {
            return this.PersonID.HasValue && this.PersonID.Value == personID && this.IsValid();
        }

        public void Discard()
        {
            this.Expire = CommonService.Now.AddSeconds(-1);
        }

        static public Activity Create<T>(DateTimeOffset? expire = null, string type = "", T data = default(T), string pin = "", Guid? personID = null)
        {
            var now = CommonService.Now;

            return new Activity()
            {
                ID = CommonService.NewGuid,
                PersonID = personID,
                Date = now,
                Expire = expire == null || !expire.HasValue ? now.AddMinutes(DEFAULT_EXPIRE_MINUTES) : expire.Value,
                Type = type ?? "",
                Data = data == null ? "" : CommonService.Serialize(data),
                Pin = pin ?? "",
            };
        }
    }
}
