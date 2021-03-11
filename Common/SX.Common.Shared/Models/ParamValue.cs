using SX.Common.Shared.Interfaces;
using System;

namespace SX.Common.Shared.Models
{
    public class ParamValue : INamed
    {
        public string Name { get; set; }
        public CustomValue Value { get; set; }

        protected ParamValue() { }

        public ParamValue(string name, CustomValue value)
        {
            Name = name;
            Value = value;
        }

        public override string ToString() => $"{this.Name ?? ""} = {this.Value?.ToString() ?? "NULL"}";

        public override int GetHashCode() => this.ToString().GetHashCode();

        public override bool Equals(object obj)
        {
            if (Object.ReferenceEquals(obj, null))
                return false;

            if (Object.ReferenceEquals(obj, this))
                return true;

            var item = obj as ParamValue;
            if (item == null)
                return false;

            return this.Name.Equals(item.Name) && this.Value.Equals(item.Value);
        }
    }

}
