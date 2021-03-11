using SX.Common.Shared.Exceptions;
using SX.Common.Shared.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace SX.Common.Shared.Models
{
    public class ParamValueCollection : NamedCollection<ParamValue>
    {
        public const string TEXT_PARAM_VALUE_VARIABLE_PATTERN = @"(#|(?<=\s)@|^@)(?<paramname>[\w\-\.\d]+)(?<!\.)(#)?";

        private string _prefix = "";

        public string Prefix
        {
            get { return _prefix; }
            protected set { _prefix = String.IsNullOrEmpty(value) ? "" : value.Trim('.'); }
        }

        public ParamValueCollection()
            : base() { }

        public ParamValueCollection(string prefix)
            : base() { this.Prefix = prefix ?? ""; }

        public ParamValueCollection(IEnumerable<ParamValue> collection, string prefix = "")
            : base(collection) { this.Prefix = prefix ?? ""; }

        public CustomValue GetValue(string name)
        {
            return this.Get(name)?.Value;
        }

        //public T? GetValue<T>(string name)
        //{
        //    var value = this.GetValue(name);
        //    if (value == null)
        //        return null;

        //    Type type = typeof(T);
        //    if (value.Type == CustomValue.ValueType.Number && type == typeof(decimal) || type == typeof(int) || type == typeof(float) || type == typeof(long) || type == typeof(short) || type == typeof(byte))
        //        return (T)(value as CustomValueNumber)._value;

        //}

        public string GetText(string name)
        {
            return this.Get(name)?.Value?.ToString() ?? "";
        }
        public decimal? GetNumber(string name)
        {
            var value = this.GetValue(name);
            return value == null ? null : (value as CustomValueNumber)?._value;
        }
        public DateTime? GetDate(string name)
        {
            var value = this.GetValue(name);
            return value == null ? null : (value as CustomValueDate)?._value;
        }
        public bool? GetBool(string name)
        {
            var value = this.GetValue(name);
            return value == null ? null : (value as CustomValueBool)?._value;
        }

        public void Add(string name, CustomValue value)
        {
            if (String.IsNullOrEmpty(name))
                throw new ArgumentException("Имя параметра не может быть пусто");

            this.Add(new ParamValue((this.Prefix + "." + name).Trim('.'), value));
        }

        public void Add(IEnumerable<ParamValue> values)
        {
            if (values == null)
                return;

            foreach (var value in values)
                this.Add(value.Name, value.Value);
        }

        public void Add(string prefix, IParamValueContainer container)
        {
            if (container != null)
                this.Add(container.GetParamValues(prefix));
        }

        public override bool Set(ParamValue item)
        {
            if (item == null)
                return false;

            //this[item.Name] = item.Value;

            var current = this.Get(item.Name);
            if (current != null)
                this.Remove(current);

            this.Add(new ParamValue(item.Name, item.Value));

            return true;
        }

        public void Set(string name, CustomValue value)
        {
            if (value == null)
                return;

            var current = this.Get(name);
            if (current != null)
                this.Remove(current);

            this.Add(new ParamValue(name, value));
        }

        public string Replace(string text, string replaceMissingParams = null)
        {
            if (String.IsNullOrEmpty(text))
                return "";

            //var result = Regex.Replace(text, @"[#|@](?<paramname>[\w\-\.\d]+)(?<!\.)", match =>
            var result = Regex.Replace(text, TEXT_PARAM_VALUE_VARIABLE_PATTERN, match =>
            {
                var found = match.ToString();
                var paramname = match.Groups["paramname"].Value; //.Trim('.');
                return this.GetText(paramname) ?? replaceMissingParams ?? found;
                //var paramvalue = this.GetValue(paramname);
                //return paramvalue == null ? "" : paramvalue.ToString();
            });

            return result;
        }

        public List<KeyValuePair<string, CustomValue>> GetKeyValuePairs()
        {
            return this.Select(i => new KeyValuePair<string, CustomValue>(i.Name, i.Value)).ToList();
        }

        public List<KeyValuePair<string, string>> GetKeyStringPairs()
        {
            return this.Select(i => new KeyValuePair<string, string>(i.Name, i.ToString())).ToList();
        }

        static public ParamValueCollection GetObjectProperties(object obj, string prefix = "")
        {
            var result = new ParamValueCollection(prefix);

            if (obj == null)
                return result;

            var properties = obj.GetType().GetProperties();
            foreach (var p in properties)
            {
                string name = p.Name;
                var value = p.GetValue(obj, null);

                if (value == null)
                    continue;

                result.Add(GetParamValues(name, value));
            }

            return result;
        }

        static public ParamValueCollection GetParamValues(string name, object value)
        {
            var result = new ParamValueCollection();

            if (value == null)
                return result;

            var type = value.GetType();

            if (type == typeof(string))
            {
                result.Add(name, value.ToString());
            }
            else if (type == typeof(DateTime))
            {
                result.Add(name, ((DateTime)value));
            }
            else if (type == typeof(DateTimeOffset))
            {
                result.Add(name, ((DateTimeOffset)value).DateTime);
            }
            else if (type.IsClass)
            {
                if (!typeof(IEnumerable).IsAssignableFrom(type) && !type.IsArray)
                    result.Add(GetObjectProperties(value, name));
            }
            else
            {
                result.Add(name, value.ToString());
            }

            return result;
        }

        public static ParamValueCollection Split(string text, params char[] separators)
        {
            var result = new ParamValueCollection();

            if (String.IsNullOrWhiteSpace(text))
                return result;

            var items = text.SplitFormatted(separators);
            foreach (var item in items)
            {
                if (String.IsNullOrWhiteSpace(item))
                    continue;

                var split = item.Split('=');
                if (split == null || split.Length != 2)
                    throw new CustomFormatException("Param's Name and Value should be separated by '='");

                var name = split[0].Trim();
                if (name.Length > 2 && name[0] == name[name.Length - 1] && separators.Contains(name[0]))
                    name = name.Substring(1, name.Length - 2);

                var value = split[1].Trim();
                if (value.Length > 2 && value[0] == value[value.Length - 1] && separators.Contains(value[0]))
                    value = value.Substring(1, value.Length - 2);

                result.Add(name, value);
            }

            return result;
        }

        public static ParamValueCollection Split(string input)
        { return ParamValueCollection.Split(input, ';'); }

        public static List<string> GetVariables(string text)
        {
            if (String.IsNullOrEmpty(text))
                return new List<string>();

            return Regex.Matches(text.RemoveHtml(" "), TEXT_PARAM_VALUE_VARIABLE_PATTERN)
                .Cast<Match>()
                .Select(match => match.Value.Replace("#", "").Replace("@", ""))
                .ToList();
        }
    }
}
