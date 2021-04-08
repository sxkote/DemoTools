using SX.Common.Shared.Enums;
using SX.Common.Shared.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SX.Common.Shared.Models
{
    public class Recipient
    {
        public virtual RecipientType Type => DefineRecipientType(this.Address);

        public string Address { get; protected set; }

        public Recipient(string address)
        {
            this.Address = address ?? "";
        }

        public override string ToString()
        {
            return this.Address ?? "";
        }

        public bool IsMultiple()
        {
            return Regex.IsMatch(this.Address, Recipients.SEPARATORS);
        }

        static public RecipientType DefineRecipientType(string address)
        {
            if (String.IsNullOrWhiteSpace(address))
                return RecipientType.Name;

            if (address.Contains("@"))
                return RecipientType.Email;
            if (CommonService.PreparePhoneNumber(address).Length == CommonService.PHONE_LENGTH)
                return RecipientType.Phone;

            return RecipientType.Name;
        }

        static public implicit operator Recipient(string value)
        {
            return new Recipient(value);
        }

        static public implicit operator string(Recipient recipient)
        {
            return recipient?.Address ?? "";
        }
    }

    public class Recipients : List<Recipient>
    {
        public const string SEPARATORS = @",|;";

        public Recipients(IEnumerable<Recipient> collection)
            : base() { this.Add(collection); }

        public Recipients(params Recipient[] recipients)
            : base() { this.Add(recipients); }

        public Recipients(string recipients)
            : base() { this.Add(Recipients.GetRecipients(recipients)); }

        protected bool Contains(string address)
        {
            return this.Any(r => r.Address.Equals(address, CommonService.StringComparison));
        }

        new public void Add(Recipient recipient)
        {
            var address = recipient?.Address;

            if (String.IsNullOrWhiteSpace(address))
                return;

            if (recipient.IsMultiple())
                this.Add(Recipients.GetRecipients(address));
            else if (!this.Contains(address))
                base.Add(recipient);
        }

        public void Add(IEnumerable<Recipient> recipients)
        {
            if (recipients == null)
                return;

            foreach (var r in recipients)
                this.Add(r);
        }

        public override string ToString()
        {
            return this.ToString(";");
        }

        public string ToString(string separator)
        {
            return String.Join(separator, this.Select(r => r.Address));
        }

        static public Recipients GetRecipients(string recipients)
        {
            var result = new Recipients();

            if (String.IsNullOrWhiteSpace(recipients))
                return result;

            var addresses = Regex.Split(recipients, Recipients.SEPARATORS);
            foreach (var address in addresses.Where(a => !String.IsNullOrWhiteSpace(a)))
                result.Add(address);

            return result;
        }


        static public implicit operator Recipients(string value)
        {
            return new Recipients(value);
        }

        static public implicit operator Recipients(Recipient recipient)
        {
            return new Recipients(recipient);
        }
    }
}
