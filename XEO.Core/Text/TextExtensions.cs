using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XEO.Core.Text
{
    public static class TextExtensions
    {
        public static IEnumerable<char> Enumerate(this char character, int times)
        {
            for (int i = 0; i < times; i++)
            {
                yield return character;
            }
        }

        public static IEnumerable<string> Enumerate(this string text, int times)
        {
            for (int i = 0; i < times; i++)
            {
                yield return text;
            }
        }

        public static string GetString(this IEnumerable<char> chars)
        {
            return new string(chars.ToArray());
        }

        public static string GetString(this IEnumerable<string> strings)
        {
            return string.Concat(strings);
        }

        public static string Repeat(this string text, int times)
        {
            return text.Enumerate(times).GetString();
        }

        public static string Repeat(this char character, int times)
        {
            return character.Enumerate(times).GetString();
        }


        //public static implicit operator string(IEnumerable<char> chars)
        //{
        //    return chars.GetString();
        //}

        //public static implicit operator string(IEnumerable<string> strings)
        //{
        //    return strings.GetString();
        //}

        //public static string operator +(IEnumerable<char> charsFirst, IEnumerable<char> charsSecond)
        //{
        //    return charsFirst.GetString() + charsSecond;
        //}

        //public static string operator +(string text, IEnumerable<char> chars)
        //{
        //    return text + chars.GetString();
        //}

        //public static string operator +(IEnumerable<char> chars, string text)
        //{
        //    return chars.GetString() + text;
        //}

        //public static string operator +(IEnumerable<string> stringsFirst, IEnumerable<string> stringsSecond)
        //{
        //    return stringsFirst.GetString() + stringsSecond;
        //}

        //public static string operator +(string text, IEnumerable<string> strings)
        //{
        //    return text + string.Concat(strings);
        //}

        //public static string operator +(IEnumerable<string> strings, string text)
        //{
        //    return string.Concat(strings) + text;
        //}

    }
}
