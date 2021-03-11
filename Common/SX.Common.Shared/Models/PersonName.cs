using SX.Common.Shared.Enums;
using SX.Common.Shared.Exceptions;
using SX.Common.Shared.Interfaces;
using SX.Common.Shared.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace SX.Common.Shared.Models
{
    public class PersonName
    {
        public const string PREFIX_REFERENCE_MALE = "Уважаемый";
        public const string PREFIX_REFERENCE_FEMALE = "Уважаемая";
        public const string PREFIX_REFERENCE_NONE = "Уважаемый(ая)";


        public string First { get; private set; }
        public string Last { get; private set; }

        private PersonName()
        {
            this.First = "";
            this.Last = "";
        }

        public PersonName(string first, string last)
        {
            this.First = String.IsNullOrEmpty(first) ? "" : first;
            this.Last = String.IsNullOrEmpty(last) ? "" : last;
        }

        public override string ToString()
        {
            return String.Format("{0} {1}", this.First, this.Last);
        }

        public override int GetHashCode()
        {
            return this.ToString().GetHashCode();
        }

        public override bool Equals(object name)
        {
            return name != null && name is PersonName && this == (PersonName)name;
        }

        public static bool operator ==(PersonName name1, PersonName name2)
        {
            if (ReferenceEquals(name1, null) || ReferenceEquals(name2, null))
                return false;

            if (ReferenceEquals(name1, name2))
                return true;

            return name1.Last.Equals(name2.Last, CommonService.StringComparison) && name1.First.Equals(name2.First, CommonService.StringComparison);
        }

        public static bool operator !=(PersonName name1, PersonName name2)
        {
            return !(name1 == name2);
        }

        public static implicit operator PersonName(PersonFullName name)
        {
            return new PersonName(name.First, name.Last);
        }

        public PersonName Copy()
        {
            return new PersonName()
            {
                First = this.First,
                Last = this.Last
            };
        }

        public string GetReference(Gender gender = Gender.Unknown)
        {
            var prefix = PersonName.GetReferencePrefix(gender);

            return $"{prefix} {this.First} {this.Last}".Trim();
        }

        static public string GetReferencePrefix(Gender gender)
        {
            switch (gender)
            {
                case Gender.Male:
                    return PREFIX_REFERENCE_MALE;
                case Gender.Female:
                    return PREFIX_REFERENCE_FEMALE;
                default:
                    return PREFIX_REFERENCE_NONE;
            }
        }
    }

    public class PersonFullName : IParamValueContainer
    {
        public string First { get; private set; } = "";
        public string Last { get; private set; } = "";
        public string Second { get; private set; } = "";

        private PersonFullName() { }

        public PersonFullName(string first, string last, string second = "")
        {
            this.First = String.IsNullOrEmpty(first) ? "" : first;
            this.Last = String.IsNullOrEmpty(last) ? "" : last;
            this.Second = String.IsNullOrEmpty(second) ? "" : second;
        }

        public string InitialisOnly() => $"{GetInitial(this.First)} {GetInitial(this.Second)}".Trim();

        public string InitialsWithLastName() => $"{this.Last ?? ""} {this.InitialisOnly()}".Trim();

        public string FullName() => String.Format("{0} {1} {2}", this.Last ?? "", this.First ?? "", this.Second ?? "").Trim();
        public string ShortName() => String.Format("{0} {1}", this.First ?? "", this.Last ?? "").Trim();

        /// <summary>
        /// ФИО полностью
        /// </summary>
        /// <returns></returns>
        public override string ToString() => this.FullName();

        public override bool Equals(object name)
        {
            return name != null && name is PersonFullName && this == (PersonFullName)name;
        }

        public static bool operator ==(PersonFullName name1, PersonFullName name2)
        {
            if (ReferenceEquals(name1, null) || ReferenceEquals(name2, null))
                return false;

            if (ReferenceEquals(name1, name2))
                return true;

            return name1.Last.Equals(name2.Last) && name1.First.Equals(name2.First) && name1.Second.Equals(name2.Second);
        }

        public static bool operator !=(PersonFullName name1, PersonFullName name2)
        {
            return !(name1 == name2);
        }

        public override int GetHashCode()
        {
            return this.ToString().GetHashCode();
        }

        public PersonFullName Copy()
        {
            return new PersonFullName()
            {
                First = this.First,
                Last = this.Last,
                Second = this.Second
            };
        }

        public string GetReference(Gender gender = Gender.Unknown)
        {
            var prefix = PersonName.GetReferencePrefix(gender);

            if (String.IsNullOrWhiteSpace(this.Second))
                return $"{prefix} {this.First} {this.Last}".Trim();
            else
                return $"{prefix} {this.First} {this.Second}".Trim();
        }

        public ParamValueCollection GetParamValues(string prefix = "")
        {
            var collection = new ParamValueCollection(prefix);

            collection.Add("First", this.First);
            collection.Add("Last", this.Last);
            collection.Add("Second", this.Second);

            return collection;
        }

        static public string GetFirstLetter(string value)
        {
            if (String.IsNullOrWhiteSpace(value))
                return "";

            if (value.Length > 1)
                return value[0].ToString();
            else if (value.Length == 1)
                return value;
            return "";
        }

        static public string GetInitial(string value)
        {
            var firstLetter = GetFirstLetter(value);
            if (!String.IsNullOrWhiteSpace(firstLetter))
                firstLetter += ".";

            return firstLetter;
        }

        static public PersonFullName Parse(string text)
        {
            if (String.IsNullOrWhiteSpace(text))
                throw new CustomArgumentException("Не задано имя!");

            var split = text.Split(' ');
            switch (split.Length)
            {
                case 0:
                    throw new CustomArgumentException("Не задано имя!");
                case 1:
                    return new PersonFullName(split[0].Trim(), "");
                case 2:
                    return new PersonFullName(split[1].Trim(), split[0].Trim());
                case 3:
                    return new PersonFullName(split[1].Trim(), split[0].Trim(), split[2].Trim());
                default:
                    return new PersonFullName(split[1].Trim(), split[0].Trim(), split[2].Trim());
            }
        }
    }

}
