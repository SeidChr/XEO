using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XEO.Core.Text;

namespace XEO.Console
{
    public class Selector
    {
        public Selector()
        {
            CountSeperator = ": ";
            SelectoOptionText = "Plese select a line by Number: ";
            SelectOptionMarginLines = 1;
        }

        public int SelectOptionMarginLines
        {
            get;
            set;
        }

        public string CountSeperator
        {
            get;
            set;
        }

        public string SelectoOptionText
        {
            get;
            set;
        }

        public string InvalidSelectionText
        {
            get;
            set;
        }



        /// <summary>
        /// Prints n option-list to console, queries the desired option and retrurns the selected value.
        /// </summary>
        /// <param name="options">List of options.</param>
        /// <returns>Value of the selected option.</returns>
        public TValue Select<TValue>(IEnumerable<TValue> options, Func<TValue, string> idSelector)
        {
            var indent = System.Console.CursorLeft;
            var result = default(TValue);

            var elementCount = options.Count();
            var countWidth = elementCount.ToString().Length;

            int i = 1;
            foreach (var element in options)
            {
                var currentCountAsString = i.ToString();
                System.Console.Write(' '.Repeat(countWidth - currentCountAsString.Length) + currentCountAsString);
                System.Console.Write(CountSeperator);
                System.Console.WriteLine(idSelector(element));

                System.Console.CursorLeft = indent;
                i++;
            }

            for (i = 0; i < SelectOptionMarginLines; i++)
            {
                System.Console.WriteLine();
            }

            System.Console.CursorLeft = indent;
            System.Console.Write(SelectoOptionText);

            var inputIndent = System.Console.CursorLeft;
            var oldIsCursorVisible = System.Console.CursorVisible;
            System.Console.CursorVisible = true;
            string optionText = System.Console.ReadLine();

            int optionInt;

            if (int.TryParse(optionText, out optionInt))
            {
                result = options.ElementAtOrDefault(optionInt - 1);
            }

            System.Console.CursorVisible = oldIsCursorVisible;

            return result;
        }

        private bool IsValidOption(string optionText)
        {
            throw new NotImplementedException();
        }
    }
}
