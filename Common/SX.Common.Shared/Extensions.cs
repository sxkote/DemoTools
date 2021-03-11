using SX.Common.Shared.Exceptions;
using SX.Common.Shared.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Reflection;
using SX.Common.Shared.Attributes;

namespace SX.Common.Shared
{
    public static class Extensions
    {
        public static byte[] ReadFully(this Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }

        public static async Task<byte[]> ReadFullyAsync(this Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = await input.ReadAsync(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }

        public static void SetValue(this Type type, object obj, string name, object value)
        {
            type.GetProperty(name).SetValue(obj, value);
        }

        public static void SetValue<TObject, TProperty>(this Type typeT, TObject obj, Expression<Func<TObject, TProperty>> property, object value)
        {
            var propertyMemberExpression = property.Body as MemberExpression;
            if (propertyMemberExpression == null)
                return;

            var propertyName = propertyMemberExpression.Member.Name;

            typeT.GetProperty(propertyName)
                .SetValue(obj, value);
        }

        public static void SetValue<TObject, TProperty>(TObject obj, Expression<Func<TObject, TProperty>> property, object value)
        {
            SetValue(typeof(TObject), obj, property, value);
        }

        public static IEnumerable<string> SplitFormatted(this string input, params char[] separators)
        {
            var sb = new StringBuilder();
            bool quoted = false;
            bool apostrophed = false;

            foreach (char c in input)
            {
                if (quoted || apostrophed)
                {
                    if (quoted && c == '"')
                        quoted = false;
                    else if (apostrophed && c == '\'')
                        apostrophed = false;

                    sb.Append(c);
                }
                else
                {
                    if (separators.Contains(c))
                    {
                        yield return sb.ToString();
                        sb.Length = 0;
                        continue;
                    }

                    if (c == '"')
                        quoted = true;
                    else if (c == '\'')
                        apostrophed = true;

                    sb.Append(c);
                }
            }

            if (quoted || apostrophed)
                throw new CustomArgumentException("Unterminated quotation mark.");

            yield return sb.ToString();
        }

        public static XmlDocument ToXmlDocument(this XDocument xDocument)
        {
            var xmlDocument = new XmlDocument();
            using (var xmlReader = xDocument.CreateReader())
            {
                xmlDocument.Load(xmlReader);
            }
            return xmlDocument;
        }

        public static XDocument ToXDocument(this XmlDocument xmlDocument)
        {
            using (var nodeReader = new XmlNodeReader(xmlDocument))
            {
                nodeReader.MoveToContent();
                return XDocument.Load(nodeReader);
            }
        }

        public static int DaysInThisYear(this DateTime date)
        {
            return DateTime.IsLeapYear(date.Year) ? 366 : 365;
        }

        public static string RemoveHtml(this string text, string replaceHTML = "", string replaceBR = "\n")
        {
            return String.IsNullOrWhiteSpace(text) ? "" : CommonService.RemoveHtml(text);
        }

        static public bool IsHtml(this string text)
        {
            if (String.IsNullOrWhiteSpace(text))
                return false;

            return Regex.IsMatch(text, @"<\/p>|<\/div>|<\/html>|<\/body>", RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.RightToLeft);
        }

        static public DateTime EndOfDay(this DateTime date)
        {
            return date.Date.AddHours(23).AddMinutes(59).AddSeconds(59);
        }

        static public DateTime FirstDayOfMonth(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, 1);
        }

        static public int DaysInMonth(this DateTime date)
        {
            return DateTime.DaysInMonth(date.Year, date.Month);
        }

        static public DateTime LastDayOfMonth(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.DaysInMonth());
        }

        static public List<T> Remove<T>(this ICollection<T> collection, Func<T, bool> condition)
        {
            if (collection == null)
                return null;

            var remove = collection.Where(condition).ToList();

            foreach (var item in remove)
                collection.Remove(item);

            return remove;
        }

        public static IEnumerable<string> GetWithOrder(this Enum enumValue)
        {
            return enumValue.GetType().GetWithOrder();
        }

        public static IEnumerable<string> GetWithOrder(this Type type)
        {
            if (!type.IsEnum)
            {
                throw new ArgumentException("Type must be an enum");
            }

            return type.GetFields()
                    .Where(field => field.IsStatic)
                    .Select(field => new
                    {
                        field,
                        attribute = field.GetCustomAttribute<EnumOrderAttribute>()
                    })
                    .Select(fieldInfo => new
                    {
                        name = fieldInfo.field.Name,
                        order = fieldInfo.attribute != null ? fieldInfo.attribute.Order : Int32.MaxValue
                    })
                    .OrderBy(field => field.order)
                    .Select(field => field.name);
        }

        public static decimal AsCurrency(this decimal amount)
        {
            decimal r = (amount - Math.Truncate(amount)); // 0.345
            r = r * 10; // 3.45
            r = r - Math.Truncate(r); // 0.45
            r = r * 10; // 4.5
            decimal second = Math.Truncate(r); // 4
            r = 10 * (r - Math.Truncate(r)); // 5
            r = Math.Truncate(r);

            if (r != 5)
                return Math.Round(amount, 2);
            else
            {
                if (second % 2 == 0)
                    return Math.Floor(amount * 100) / 100;
                else
                    return Math.Floor(amount * 100) / 100 + 0.01m;
            }
        }

        public static string ToCurrencyString(this decimal summ, string currency = " р.")
        {
            return summ.ToString($"#,0.00") + currency ?? "";
        }

        public static string ToCurrencyText(this decimal summ)
        {
            return CommonService.RusCurrencyToText(summ);
        }

        public static void ForEach<T>(this IEnumerable<T> collection, Action<T> action)
        {
            collection.ToList().ForEach(action);
        }
    }
}
