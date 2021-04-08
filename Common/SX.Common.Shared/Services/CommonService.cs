using Newtonsoft.Json;
using SX.Common.Shared.Classes;
using SX.Common.Shared.Exceptions;
using SX.Common.Shared.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Linq;

namespace SX.Common.Shared.Services
{
    public static class CommonService
    {
        public const int PHONE_LENGTH = 11;
        public const StringComparison StringComparison = System.StringComparison.OrdinalIgnoreCase;

        public static CultureInfo CultureInfo => new CultureInfo("ru-RU");

        [ThreadStatic]
        private static Random _randomizer;

        public static Random Randomizer
        {
            get
            {
                if (_randomizer == null)
                    _randomizer = new Random(DateTime.UtcNow.Millisecond);
                return _randomizer;
            }
        }

        static public DateTimeOffset Now => DateTimeOffset.Now;
        //{ get { return TimeZoneInfo.ConvertTime(DateTimeOffset.Now, TimeZoneInfo.FindSystemTimeZoneById("Russian Standard Time")); } }

        static public Guid NewGuid => System.Guid.NewGuid();
        static public Guid ParseGuid(string input) => Guid.Parse(input);
        static public Guid? TryParseGuid(string input) => String.IsNullOrWhiteSpace(input) ? (Guid?)null : ParseGuid(input);

        static public DateTime? ParseDateTime(string input)
        {
            if (String.IsNullOrWhiteSpace(input))
                return null;

            if (DateTime.TryParse(input, out DateTime result))
                return result;

            return null;
        }

        static public DateTime? ParseDateTimeExact(string input, params string[] formats)
        {
            if (String.IsNullOrWhiteSpace(input))
                return null;

            if (DateTime.TryParseExact(input, formats, CultureInfo, DateTimeStyles.None, out DateTime result))
                return result;

            return null;
        }

        static public JsonSerializerSettings JsonSettings
        {
            get
            {
                var settings = new JsonSerializerSettings()
                {
                    Formatting = Formatting.Indented,

                    //DateTimeZoneHandling = Newtonsoft.Json.DateTimeZoneHandling.Utc,
                    DateFormatString = JsonDateTimeConverter.DateTimeOffsetFormat,
                    DateParseHandling = DateParseHandling.DateTimeOffset,

                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,

                    NullValueHandling = NullValueHandling.Ignore,

                    Converters = CommonService.JsonConverters,
                };

                //settings.Converters.Add(new StringEnumConverter() { AllowIntegerValues = true });
                //settings.Converters.Add(new CustomJsonDateTimeConverter());
                //settings.Converters.Add(new CustomJsonCustomValueConverter());

                return settings;
            }
        }

        static public List<JsonConverter> JsonConverters = new List<JsonConverter>();

        static public string Serialize(object obj)
        {
            return JsonConvert.SerializeObject(obj, Formatting.Indented, CommonService.JsonSettings);
        }

        static public T Deserialize<T>(string data)
        {
            return JsonConvert.DeserializeObject<T>(data, CommonService.JsonSettings);
        }

        static public List<Type> DomainEventTypes = new List<Type>();
        static public X509Certificate2 FindCertificateFromStore(string serialNumber)
        {
            if (string.IsNullOrWhiteSpace(serialNumber))
                throw new CustomArgumentException("Для поиска сертификата в хранилище сертификатов укажите корректно серийный номер!");

            var certificate = GetCerificateFromStore(serialNumber, StoreName.My, StoreLocation.CurrentUser);
            if (certificate == null)
                certificate = GetCerificateFromStore(serialNumber, StoreName.My, StoreLocation.LocalMachine);

            return certificate;
        }

        public static bool IsTimeOfDay(this TimeSpan timeOfDay)
        {
            if (timeOfDay.TotalMilliseconds < 0)
                return false;

            if (timeOfDay.TotalHours >= 24)
                return false;

            return true;
        }

        static public DateTime RemoveMilliseconds(this DateTime now)
        {
            return new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second);
        }

        static public Type GetDomainEventType(string name)
        {
            if (String.IsNullOrEmpty(name))
                return null;

            try
            {
                return CommonService.DomainEventTypes.FirstOrDefault(t => t.Name.Equals(name, CommonService.StringComparison));
            }
            catch { return null; }
        }

        static private X509Certificate2 GetCerificateFromStore(string serialNumber, StoreName storeName, StoreLocation storeLocation, bool validOnly = false)
        {
            if (string.IsNullOrWhiteSpace(serialNumber))
                throw new CustomArgumentException("Для поиска сертификата в хранилище сертификатов укажите корректно серийный номер!");

            X509Certificate2 certificate = null;

            var store = new X509Store(storeName, storeLocation);
            store.Open(OpenFlags.ReadOnly);
            var coll = store.Certificates.Find(X509FindType.FindBySerialNumber, serialNumber, validOnly);
            if (coll.Count > 0)
                certificate = coll[0];

            store.Close();

            return certificate;
        }

        static public string[] Split(string text, params string[] separators)
        {
            if (separators == null || separators.Length <= 0)
                return text.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
            else
                return text.Split(separators, StringSplitOptions.RemoveEmptyEntries);
        }

        public static IEnumerable<DateTime> SplitDateTimeInterval(DateTime from, DateTime to, DateTime start, TimeSpan period)
        {
            var result = new List<DateTime>();

            if (from > to)
                return result;

            var dateTime = start;

            while (to >= dateTime)
            {
                if (from <= dateTime)
                    result.Add(dateTime);

                dateTime += period;
            }

            return result;
        }

        static public DateTime Min(DateTime d1, DateTime d2) => d1 < d2 ? d1 : d2;

        static public DateTime Max(DateTime d1, DateTime d2) => d1 > d2 ? d1 : d2;

        static public string HashPassword(string password, int level = 11)
        {
            return BCrypt.Net.BCrypt.HashPassword(password, level);
        }

        static public bool VerifyPassword(string password, string hash)
        {
            try { return BCrypt.Net.BCrypt.Verify(password, hash); }
            catch { return false; }
        }

        static public string GeneratePin(int length = 6)
        { return CoderService.GenerateCode(length, baseLength: 10, capitalize: false); }

        static public string GeneratePassword(int length = 8, bool capitalize = true)
        { return CoderService.GenerateCode(length, capitalize: capitalize); }

        static public string GenerateCode(int length = 6, int baseLength = CoderService.MaxBaseLength, bool capitalize = false)
        { return CoderService.GenerateCode(length, baseLength, capitalize); }

        //static public string GenerateCode(string prefix = "", byte subscriptionID = 0, int length = 6)
        //{
        //    var service = new CoderService(36);

        //    string codePrefix = String.IsNullOrEmpty(prefix) ? "" : (prefix.TrimEnd('-') + "-");

        //    string codeSubscr = subscriptionID <= 0 ? "" : (subscriptionID.ToString() + "-");
        //    if (String.IsNullOrEmpty(codePrefix) && !String.IsNullOrEmpty(codeSubscr))
        //        codeSubscr = "S" + codeSubscr;

        //    string codeDate = service.Encode(CommonService.Now.DateTime);

        //    string codeRandom = service.Generate(length <= 0 ? 6 : length);

        //    return String.Format("{0}{1}{2}-{3}", codePrefix, codeSubscr, codeDate, codeRandom);
        //}

        static public DateTime GenerateRandomDate(DateTime dateFrom, DateTime dateTo)
        {
            var days = (dateTo - dateFrom).Days;
            return days > 0 ? dateFrom.AddDays(Randomizer.Next(days)) : dateFrom;
        }

        static public decimal GenerateRandomDecimal(decimal min, decimal max)
        {
            return (decimal)Randomizer.Next((int)Math.Floor(min), (int)Math.Floor(max))
                + (decimal)Randomizer.Next(0, 100) / 100m;
        }

        static public string GenerateEntityCode(string prefix = "", int length = 8)
        {
            var service = new CoderService(36);

            string codePrefix = String.IsNullOrEmpty(prefix) ? "" : (prefix.TrimEnd('-') + "-");

            string codeDate = service.Encode(CommonService.Now.DateTime);

            string codeRandom = service.Generate(length <= 0 ? 8 : length);

            return String.Format("{0}{1}-{2}", codePrefix, codeDate, codeRandom);
        }

        //static public string RusCurrencyToString(double value)
        //{
        //    return RusCurrency.Str(value);
        //}

        static public string RusCurrencyToText(decimal value)
        {
            return RusCurrency.Str(Convert.ToDouble(value));
        }
        static public string CurrencyToText(decimal value, short currency = 810)
        {
            string cur = "RUR";
            switch (currency)
            {
                case 840: cur = "USD"; break;
                case 978: cur = "EUR"; break;
                default: cur = "RUR"; break;
            }
            return RusCurrency.Str(Convert.ToDouble(value), cur);
        }

        /// <summary>
        /// Возвращает поле сертификата (CN=..., O=...)
        /// </summary>
        /// <param name="certificate"></param>
        /// <param name="pattern">CN или O</param>
        /// <returns></returns>
        static public string GetCertificateProperty(X509Certificate2 certificate, string pattern)
        {
            string result = "";
            string Name = certificate.SubjectName.Name;

            pattern = pattern.ToLower();

            string[] parts = Name.Split(new char[1] { ',' });
            for (int i = 0; i < parts.Length; i++)
                if (parts[i].Trim().ToLower().StartsWith(pattern + "="))
                {
                    result = parts[i].Replace(pattern + "=", "").Replace(pattern.ToUpper() + "=", "").Trim();
                    break;
                }

            if (result.StartsWith("\"") && result.EndsWith("\""))
                result = result.Substring(1, result.Length - 2);

            result = result.Replace("\"\"", "\"");

            return result;
        }

        static public string RusCurrencyToString(double value, string currency)
        {
            return RusCurrency.Str(value, currency.Trim());
        }

        static public string RusCurrencyToString(double value, bool male, string seniorOne, string seniorTwo, string seniorFive, string juniorOne, string juniorTwo, string juniorFive)
        {
            return RusCurrency.Str(value, male, seniorOne, seniorTwo, seniorFive, juniorOne, juniorTwo, juniorFive);
        }


        static public string GetSHA256(byte[] data)
        {
            if (data == null || data.Length <= 0)
                return "";

            using (var sha256Hash = SHA256.Create())
                return GetHashByAlgorithm(sha256Hash, data);
        }

        static public string GetMD5(byte[] data)
        {
            if (data == null || data.Length <= 0)
                return "";

            using (var md5Hash = MD5.Create())
                return GetHashByAlgorithm(md5Hash, data);
        }

        static public string GetMD5(string text)
        {
            return GetMD5(Encoding.UTF8.GetBytes(text));
        }

        static public string GetHashByAlgorithm(HashAlgorithm algorithm, byte[] data)
        {
            byte[] hash = algorithm.ComputeHash(data);

            var sb = new StringBuilder();

            for (int i = 0; i < hash.Length; i++)
                sb.Append(hash[i].ToString("x2"));

            return sb.ToString();
        }


        static public Task<string> GetMD5Async(byte[] data)
        {
            return Task.Run<string>(() => { return GetMD5(data); });
        }

        static public Task<string> GetMD5Async(string text)
        {
            return Task.Run<string>(() => { return GetMD5(text); });
        }

        static public long? IsNumber(object o)
        {
            if (o is long) return (long)o;
            if (o is int) return (long)(int)o;
            if (o is short) return (long)(short)o;
            if (o is byte) return (long)(byte)o;

            long id = 0;
            if (Int64.TryParse(o.ToString(), System.Globalization.NumberStyles.Any, System.Globalization.NumberFormatInfo.InvariantInfo, out id))
                return id;

            return null;
        }

        public static int? ExtractInt(this string input)
        {
            var extractResult = ExtractIntArray(input);
            return extractResult.Any()
                ? (int?)extractResult.First()
                : null;
        }

        public static int[] ExtractIntArray(this string input)
        {
            return Regex.Matches(input, @"\d+")
                .Cast<Match>()
                .Select(v => Convert.ToInt32(v.Value))
                .ToArray();
        }

        public static bool HasDigits(string input)
        {
            return Regex.IsMatch(input, @"\d+");
        }

        public static bool HasOnlyDigits(string input)
        {
            return Regex.IsMatch(input, @"^\d+$");
        }

        public static bool HasEnCharacters(string input)
        {
            return Regex.IsMatch(input, @"[A-Za-z]+");
        }

        public static string AddSqlLikeSym(string input)
        {
            if (input.StartsWith("%") == false)
                input = $"%{input}";

            if (input.EndsWith("%") == false)
                input = $"{input}%";

            return input;
        }

        public static string PreparePhoneNumber(string phone)
        {
            if (string.IsNullOrEmpty(phone))
                return "";

            var result = phone
                 .ToLower()
                 .Replace("+", "")
                 .Replace("-", "")
                 .Replace("(", "")
                 .Replace(")", "")
                 .Replace("/", "")
                 .Replace("\\", "")
                 .Replace(" ", "")
                 .Trim();

            if (result.StartsWith("8") && result.Length == PHONE_LENGTH)
                result = "7" + result.Substring(1);
            else if (result.Length == PHONE_LENGTH - 1)
                result = string.Concat("7", result);

            return result;
        }

        public static decimal? PrepareTaxValue(decimal? tax = null)
        {
            if (tax == null || tax == 0)
                return null;

            if (tax < 1)
                return 1 + tax;

            if (tax > 2)
                return 1m + (tax / 100);

            if (tax > 1 && tax < 2)
                return tax;

            return 0;
        }

        public static string MaskAddress(string address)
        {
            if (String.IsNullOrWhiteSpace(address))
                return "";

            if (address.Length <= 1)
                return address;

            if (address.Contains("@"))
            {
                if (address.Length <= 4)
                    return address.Substring(0, 2).PadRight(address.Length, '*');
                else
                    return address.Substring(0, 4).PadRight(address.Length, '*');
            }
            else
            {
                if (address.Length <= 4)
                    return address.Substring(address.Length - 2, 2).PadLeft(address.Length, '*');
                else
                    return address.Substring(address.Length - 4, 4).PadLeft(address.Length, '*');
            }
        }

        public static string GetChanges(string name, object oldValue, object newValue)
        {
            var oldVal = oldValue?.ToString();
            var newVal = newValue?.ToString();

            if (String.IsNullOrEmpty(oldVal))
                oldVal = "(пусто)";

            if (String.IsNullOrEmpty(newVal))
                newVal = "(пусто)";

            return $"{name ?? ""}: {oldVal} -> {newVal}";
        }

        //public static decimal AsCurrency(this decimal amount)
        //{
        //    return Math.Floor(amount * 100) / 100;
        //}

        //public static decimal AsCurrency(this double amount)
        //{
        //    return Math.Floor((decimal)amount * 100) / 100;
        //}

        public static string GetSHA1(string data)
        {
            SHA1 sha = SHA1.Create();
            var sb = new StringBuilder();
            byte[] bt = sha.ComputeHash(Encoding.UTF8.GetBytes(data));
            for (int i = 0; i < bt.Length; i++)
                sb.Append(bt[i].ToString("x2"));

            return sb.ToString();
        }

        public static string GetFootprint(string data)
        {
            var sha1 = GetSHA1(data);

            return string.Concat(sha1.Substring(0, 3), sha1.Substring(sha1.Length - 3)).ToUpper();
        }

        public static string ToAbbreviation(string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            //если input - одно слово из заглавных букв, то это уже аббревиатура
            if (Regex.IsMatch(input, @"^[А-ЯЁ]+$"))
                return input;

            //выбираем все слова длинною больше 5 символов
            var matches = Regex.Matches(input, @"[А-ЯЁа-яё]{5,}").Cast<Match>();

            return String.Join("", matches.Select(word => word.Value.First()).ToArray()).ToUpper();
        }

        public static double CalculatePercent(float x, float y, int roundTo = 0)
        {
            return Math.Round((double)((x / y) * 100), roundTo);
        }

        public static int CalculatePercentToInt(float x, float y, int roundTo = 0)
        {
            return Convert.ToInt32(CalculatePercent(x, y, roundTo));
        }

        public static double CountOfDays(DateTime dateFrom, DateTime dateTo, bool count = true)
        {
            double result = 0;

            DateTime date_from_real = dateFrom.Date;
            DateTime date_to_real = dateTo.Date;
            DateTime current_date = date_from_real;

            // если нужно просто узнать количество дней
            if (count)
                return (date_to_real - date_from_real).Days;

            while (current_date < date_to_real)
            {
                if (current_date.Year == date_to_real.Year && current_date.Month == date_to_real.Month && current_date.Day == date_to_real.Day)
                    break;

                current_date = current_date.AddDays(1);

                if (count)
                    result++;
                else
                    result += 1.0 / current_date.DaysInThisYear();
            }

            return result;
        }


        public static string GetErrorMessage(Exception ex, int innerLevel = 10, bool appendStackTrace = true, bool openAggregateExceptions = true)
        {
            if (ex == null)
                return "";

            // обработка AggregateException
            if (openAggregateExceptions && ex is AggregateException)
            {
                var agrEx = ex as AggregateException;
                if (agrEx != null && agrEx.InnerExceptions.Count > 0)
                {
                    var sb = new StringBuilder();
                    foreach (var innerEx in agrEx.InnerExceptions)
                        sb.AppendLine(CommonService.GetErrorMessage(innerEx, innerLevel, appendStackTrace, openAggregateExceptions));
                    return sb.ToString();
                }
            }

            var result = (ex.Message ?? "");
            if (appendStackTrace)
                result += Environment.NewLine + (ex.StackTrace ?? "");

            try
            {
                if (innerLevel > 0)
                {

                    if (ex is AggregateException)
                    {
                        (ex as AggregateException).InnerExceptions.ToList()
                            .ForEach(e => result += Environment.NewLine + Environment.NewLine + GetErrorMessage(e, innerLevel - 1, appendStackTrace));
                    }
                    else
                    {
                        if (ex.InnerException != null)
                            result += Environment.NewLine + Environment.NewLine + GetErrorMessage(ex.InnerException, innerLevel - 1, appendStackTrace);
                    }
                }
            }
            catch { }

            return result;
        }

        public static bool IsBase64String(this string s)
        {
            s = s.Trim();
            return (s.Length % 4 == 0) && Regex.IsMatch(s, @"^[a-zA-Z0-9\+/]*={0,3}$", RegexOptions.None);
        }

        public static double ToUnixTimestamp(this DateTime value)
        {
            var origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);

            return Math.Floor((value - origin).TotalSeconds);
        }

        static public string RemoveHtml(string text, string replaceHTML = "", string replaceBR = "\n")
        {
            var htmlPattern = @"<!DOCTYPE[^>]*>|<(style|script).*?<\/(\1)>|<\/?[\w][^>]*>|<!--.*?-->";

            var result = text;

            if (replaceBR != null)
                result = Regex.Replace(result, @"<br\s?\/?>", replaceBR, RegexOptions.Compiled | RegexOptions.IgnoreCase);

            result = Regex.Replace(result, htmlPattern, replaceHTML ?? "", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Singleline);

            result = HttpUtility.HtmlDecode(result);

            return result.Trim();
        }

        public static void RangeForEach<T>(List<T> collection, int maxItemsCountToIteration, Action<IEnumerable<T>> action, bool parallel = false)
        {
            var tasks = new List<Task>();

            for (int i = 0; i < (int)Math.Ceiling((decimal)collection.Count / maxItemsCountToIteration); i++)
            {
                var subCollection = collection.GetRange(i * maxItemsCountToIteration, Math.Min(collection.Count - i * maxItemsCountToIteration, maxItemsCountToIteration));

                if (parallel)
                    tasks.Add(Task.Run(() => action(subCollection)));
                else
                    action(subCollection);
            }

            Task.WaitAll(tasks.ToArray());
        }

        public static void RangeForEach<T>(List<T> collection, int maxItemsCountToIteration, Action<IEnumerable<T>, int> action, bool parallel = false)
        {
            var tasks = new List<Task>();

            for (int i = 0; i < (int)Math.Ceiling((decimal)collection.Count / maxItemsCountToIteration); i++)
            {
                var subCollection = collection.GetRange(i * maxItemsCountToIteration, Math.Min(collection.Count - i * maxItemsCountToIteration, maxItemsCountToIteration));

                if (parallel)
                    tasks.Add(Task.Run(() => action(subCollection, i * maxItemsCountToIteration)));
                else
                    action(subCollection, i * maxItemsCountToIteration);
            }

            Task.WaitAll(tasks.ToArray());
        }

        public static DateTime GetLastMonthDate(int month, int year)
        {
            month = month >= 1 ? month : 1;

            var day = DateTime.DaysInMonth(year, month);

            return new DateTime(year, month, day);
        }

        public static List<T> InList<T>(this T item)
        {
            return new List<T> { item };
        }

        public static string FormatAccountNumber(string number)
        {
            string result = number.Trim().Replace(" ", "");
            result = result.Insert(3, " ");
            result = result.Insert(6, " ");
            result = result.Insert(10, " ");
            result = result.Insert(12, " ");
            result = result.Insert(17, " ");
            return result;
        }

        //public static bool Change<T>(this ICollection<T> currentCollection, IEnumerable<T> newCollection)
        //    where T : IEntity
        //{
        //    if (newCollection == null || newCollection.Count() == currentCollection.Count() && newCollection.All(i => currentCollection.Contains(i)))
        //        return false;

        //    var itemsToAdd = newCollection
        //        .Where(i => !currentCollection.Contains(i))
        //        .ToList();

        //    foreach (var itemToAdd in itemsToAdd)
        //        currentCollection.Add(itemToAdd);

        //    for (int i = currentCollection.Count - 1; i >= 0; i--)
        //    {
        //        var responsible = currentCollection.ElementAt(i);
        //        if (!newCollection.Contains(responsible))
        //            currentCollection.Remove(responsible);
        //    }

        //    return true;
        //}

        //public static CertificateData ParseCertificate(X509Certificate2 certificate)
        //{
        //    if (certificate == null)
        //        return null;
        //    return new Certificate(certificate);
        //}

        public static void CalcExecutionTime(Action action, out TimeSpan result)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            action();
            watch.Stop();

            result = watch.Elapsed;
        }

        public static T CalcExecutionTime<T>(Func<T> action, out TimeSpan result)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            var actionResult = action();
            watch.Stop();

            result = watch.Elapsed;

            return actionResult;
        }

        public static string ChangeKeyLayout(string text)
        {
            string data = text;
            string result = "";

            for (int i = 0; i < data.Length; i++)
            {
                #region Translite
                if (data[i] == 'q') result += 'й';
                else if (data[i] == 'w') result += 'ц';
                else if (data[i] == 'e') result += 'у';
                else if (data[i] == 'r') result += 'к';
                else if (data[i] == 't') result += 'е';
                else if (data[i] == 'y') result += 'н';
                else if (data[i] == 'u') result += 'г';
                else if (data[i] == 'i') result += 'ш';
                else if (data[i] == 'o') result += 'щ';
                else if (data[i] == 'p') result += 'з';
                else if (data[i] == '[') result += 'х';
                else if (data[i] == ']') result += 'ъ';
                else if (data[i] == 'a') result += 'ф';
                else if (data[i] == 's') result += 'ы';
                else if (data[i] == 'd') result += 'в';
                else if (data[i] == 'f') result += 'а';
                else if (data[i] == 'g') result += 'п';
                else if (data[i] == 'h') result += 'р';
                else if (data[i] == 'j') result += 'о';
                else if (data[i] == 'k') result += 'л';
                else if (data[i] == 'l') result += 'д';
                else if (data[i] == ';') result += 'ж';
                else if (data[i] == '\'') result += 'э';
                else if (data[i] == 'z') result += 'я';
                else if (data[i] == 'x') result += 'ч';
                else if (data[i] == 'c') result += 'с';
                else if (data[i] == 'v') result += 'м';
                else if (data[i] == 'b') result += 'и';
                else if (data[i] == 'n') result += 'т';
                else if (data[i] == 'm') result += 'ь';
                else if (data[i] == ',') result += 'б';
                else if (data[i] == '.') result += 'ю';
                else if (data[i] == '/') result += '.';
                else if (data[i] == '`') result += 'ё';
                else if (data[i] == 'й') result += 'q';
                else if (data[i] == 'ц') result += 'w';
                else if (data[i] == 'у') result += 'e';
                else if (data[i] == 'к') result += 'r';
                else if (data[i] == 'е') result += 't';
                else if (data[i] == 'н') result += 'y';
                else if (data[i] == 'г') result += 'u';
                else if (data[i] == 'ш') result += 'i';
                else if (data[i] == 'щ') result += 'o';
                else if (data[i] == 'з') result += 'p';
                else if (data[i] == 'х') result += '[';
                else if (data[i] == 'ъ') result += ']';
                else if (data[i] == 'ф') result += 'a';
                else if (data[i] == 'ы') result += 's';
                else if (data[i] == 'в') result += 'd';
                else if (data[i] == 'а') result += 'f';
                else if (data[i] == 'п') result += 'g';
                else if (data[i] == 'р') result += 'h';
                else if (data[i] == 'о') result += 'j';
                else if (data[i] == 'л') result += 'k';
                else if (data[i] == 'д') result += 'l';
                else if (data[i] == 'ж') result += ';';
                else if (data[i] == 'э') result += '\'';
                else if (data[i] == 'я') result += 'z';
                else if (data[i] == 'ч') result += 'x';
                else if (data[i] == 'с') result += 'c';
                else if (data[i] == 'м') result += 'v';
                else if (data[i] == 'и') result += 'b';
                else if (data[i] == 'т') result += 'n';
                else if (data[i] == 'ь') result += 'm';
                else if (data[i] == 'б') result += ',';
                else if (data[i] == 'ю') result += '.';
                else if (data[i] == '.') result += '/';
                else if (data[i] == 'ё') result += '`';
                #endregion
            }

            return result;
        }

        public static string ConvertEncoding(string text, Encoding encoding)
        {
            var srcEncodingFormat = Encoding.UTF8;
            var dstEncodingFormat = encoding;

            var originalByteString = srcEncodingFormat.GetBytes(text);
            var convertedByteString = Encoding.Convert(srcEncodingFormat, dstEncodingFormat, originalByteString);

            return dstEncodingFormat.GetString(convertedByteString);
        }

        public static string ConvertToHexString(string text, Encoding encoding)
        {
            return BitConverter.ToString(encoding.GetBytes(text)).Replace("-", "").Trim();
        }

        //public static string GetTextFromPDFDocument(byte[] data)
        //{
        //    if (data == null)
        //        throw new CustomArgumentException("Данные для извлечения текста пусты или указаны неверно!");

        //    var result = new StringBuilder();

        //    using (PdfReader reader = new PdfReader(data))
        //    {
        //        for (int i = 1; i <= reader.NumberOfPages; i++)
        //            result.Append(PdfTextExtractor.GetTextFromPage(reader, i));
        //    }

        //    return result.ToString();
        //}

        public static int GetNumberFromString(string text)
        {
            try
            {
                if (String.IsNullOrWhiteSpace(text))
                    return 0;

                var digits = new String(text.Where(Char.IsDigit).ToArray());
                if (Int32.TryParse(digits, out int result))
                    return result;

                return 0;
            }
            catch { return 0; }
        }

        //public static void GrantAccessDirectory(string path, string logonName)
        //{
        //    bool exists = Directory.Exists(path);
        //    if (!exists)
        //        Directory.CreateDirectory(path);

        //    var ds = Directory.GetAccessControl(path);
        //    ds.AddAccessRule(new FileSystemAccessRule(logonName, FileSystemRights.Modify, AccessControlType.Allow));
        //    Directory.SetAccessControl(path, ds);
        //}

        public static string GetDataText(object data)
        {
            if (data == null)
                return "";

            try
            {
                if (data is string)
                    return data.ToString() ?? "";

                if (data is Exception)
                    return GetErrorMessage(data as Exception);

                return Serialize(data);
            }
            catch
            {
                return $"error writing object of type {data.GetType().Name}";
            }
        }

        public static string GetClearINN(string inn)
        {
            if (String.IsNullOrWhiteSpace(inn))
                return "";

            if (inn.StartsWith("00") && inn.Length > 2)
                return inn.Substring(2);

            return inn;
        }

        //public static string GetClientKey(string inn, Guid clientID)
        //{
        //    return string.IsNullOrWhiteSpace(inn) ? clientID.ToString() : inn;
        //}
        //public static string GetClientKey(IClientReference client)
        //{
        //    return string.IsNullOrWhiteSpace(client.INN) ? client.ID.ToString() : client.INN;
        //}


        public static CultureInfo GetCultureRU()
        {
            return CultureInfo.GetCultureInfo("ru");
        }

        public static string GetOrganizationBrief(string title)
        {
            if (String.IsNullOrWhiteSpace(title))
                return "";

            var result = title
                .Replace("Общество с ограниченной ответственностью", "ООО")
                .Replace("Открытое акционерное общество", "ОАО")
                .Replace("Закрытое акционерное общество", "ЗАО")
                .Replace("Публичное акционерное общество", "ПАО")
                .Replace("Общество с дополнительной ответственностью", "ОДО")
                .Replace("Индивидуальный предприниматель", "ИП");

            Match match = Regex.Match(result, @"(?<form>[\w]+)\s+\""(?<title>[\w\d\s\""\'\-]+)\""");
            if (match.Success)
            {
                result = match.Groups["title"]?.Value ?? "";
                //var grForm = match.Groups["form"];
                //if (!String.IsNullOrWhiteSpace(grForm?.Value))
                //    result += $", {grForm.Value}";
                return result.Trim();
            }
            else
            {
                return title
                    .Replace("ООО", "")
                    .Replace("ОАО", "")
                    .Replace("ЗАО", "")
                    .Replace("ПАО", "")
                    .Replace("ОДО", "")
                    .Replace("ИП", "")
                    .Replace("\"", "")
                    .Trim();
            }
        }

        public static XDocument ParseXML(FileData fileData)
        {
            XDocument xml = null;

            try
            {
                xml = XDocument.Parse(Regex.Replace(Encoding.UTF8.GetString(fileData.Data), "^([\\W]+)<", "<"));
                return xml;
            }
            catch { }

            try
            {
                xml = XDocument.Parse(Regex.Replace(Encoding.GetEncoding(1251).GetString(fileData.Data), "^([\\W]+)<", "<"));
                return xml;
            }
            catch { }

            return xml;
        }

        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            HashSet<TKey> seenKeys = new HashSet<TKey>();
            foreach (TSource element in source)
            {
                if (seenKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }




        public static string ToSQLString(this DateTime date) => $"'{date.ToString("yyyy-MM-dd")}'";

        public static string ToSQLString(this DateTime? date)
        {
            if (date.HasValue)
                return $"'{date.Value.ToString("yyyy-MM-dd")}'";

            return "NULL";
        }

        public static string ToSQLString(this Guid guid) => $"'{guid.ToString()}'";

        public static string ToSQLString(this Guid? guid)
        {
            if (guid.HasValue)
                return $"'{guid.ToString()}'";

            return "NULL";
        }

        public static string ToSQLString(this bool? flag)
        {
            if (flag.HasValue)
                if (flag.Value == true)
                    return "1";
                else return "0";

            return "NULL";

        }

        public static string ToSQLString(this bool flag) => flag ? "1" : "0";

        public static string ToSQLString(this string value)
        {
            if (value == null)
                return "NULL";

            return $"'{value.Replace("'", "''")}'";
        }



        /// <summary>
        /// Compute the distance between two strings.
        /// </summary>
        public static int LevenshteinDistance(string s, string t)
        {
            int n = s.Length;
            int m = t.Length;
            int[,] d = new int[n + 1, m + 1];

            // Step 1
            if (n == 0)
            {
                return m;
            }

            if (m == 0)
            {
                return n;
            }

            // Step 2
            for (int i = 0; i <= n; d[i, 0] = i++)
            {
            }

            for (int j = 0; j <= m; d[0, j] = j++)
            {
            }

            // Step 3
            for (int i = 1; i <= n; i++)
            {
                //Step 4
                for (int j = 1; j <= m; j++)
                {
                    // Step 5
                    int cost = (t[j - 1] == s[i - 1]) ? 0 : 1;

                    // Step 6
                    d[i, j] = Math.Min(
                        Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1),
                        d[i - 1, j - 1] + cost);
                }
            }
            // Step 7
            return d[n, m];
        }

        /// <summary>
        /// Compute the distance between two strings.
        /// </summary>
        public static int LevenshteinDamerauDistance(string s, string t)
        {
            var bounds = new { Height = s.Length + 1, Width = t.Length + 1 };

            int[,] matrix = new int[bounds.Height, bounds.Width];

            for (int height = 0; height < bounds.Height; height++) { matrix[height, 0] = height; };
            for (int width = 0; width < bounds.Width; width++) { matrix[0, width] = width; };

            for (int height = 1; height < bounds.Height; height++)
            {
                for (int width = 1; width < bounds.Width; width++)
                {
                    int cost = (s[height - 1] == t[width - 1]) ? 0 : 1;
                    int insertion = matrix[height, width - 1] + 1;
                    int deletion = matrix[height - 1, width] + 1;
                    int substitution = matrix[height - 1, width - 1] + cost;

                    int distance = Math.Min(insertion, Math.Min(deletion, substitution));

                    if (height > 1 && width > 1 && s[height - 1] == t[width - 2] && s[height - 2] == t[width - 1])
                    {
                        distance = Math.Min(distance, matrix[height - 2, width - 2] + cost);
                    }

                    matrix[height, width] = distance;
                }
            }

            return matrix[bounds.Height - 1, bounds.Width - 1];
        }

        public static bool IsGUID(string expression)
        {
            if (expression != null)
            {
                Regex guidRegEx = new Regex(@"^(\{{0,1}([0-9a-fA-F]){8}-([0-9a-fA-F]){4}-([0-9a-fA-F]){4}-([0-9a-fA-F]){4}-([0-9a-fA-F]){12}\}{0,1})$");

                return guidRegEx.IsMatch(expression);
            }
            return false;
        }

        public static string GetMimeMapping(string filename) => MimeMappingsService.GetMimeMapping(filename);
    }
}
