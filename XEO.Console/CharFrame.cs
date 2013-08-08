using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XEO.Core.Text;

namespace XEO.Console
{
    public class CharFrame
    {
        public CharFrame()
        {
            Char = '#';
            
            MarginLines = 0;
            MarginColumns = 1;
            
            FullLines = 1;
            WidthColumns = 3;

            ForegroundColor = default(ConsoleColor);
            BackgroundColor = default(ConsoleColor);
        }

        public char Char
        {
            get;
            set;
        }

        public int MarginLines
        {
            get;
            set;
        }

        public int MarginColumns
        {
            get;
            set;
        }

        public int FullLines 
        {
            get; 
            set; 
        }

        public int WidthColumns
        {
            get;
            set;
        }


        public ConsoleColor ForegroundColor
        {
            get;
            set;
        }

        public ConsoleColor BackgroundColor
        {
            get;
            set;
        }

        /// <summary>
        /// Prints the given text to the console inside of a character frame.
        /// </summary>
        /// <param name="text">The text to put inside of the frame.</param>
        public void Print(string text)
        {
            var indent = System.Console.CursorLeft;

            var useForegroundColor = ForegroundColor != default(ConsoleColor);
            var useBackgroundColor = BackgroundColor != default(ConsoleColor);

            var oldForegroundColor = System.Console.ForegroundColor;
            var oldBackgroundColor = System.Console.BackgroundColor;

            var boderLine = Char.Repeat((WidthColumns * 2) + (MarginColumns * 2) + text.Length);
            var sideBorder = Char.Repeat(WidthColumns);
            var sideMargin = ' '.Repeat(MarginColumns);
            var marginLineCenter = ' '.Repeat((MarginColumns * 2) + text.Length);

            var marginLine = sideBorder
                + sideMargin
                + ' '.Repeat(text.Length)
                + sideMargin
                + sideBorder;

            if (useForegroundColor)
            {
                System.Console.ForegroundColor = ForegroundColor;
            }

            if (useBackgroundColor)
            {
                System.Console.BackgroundColor = BackgroundColor;
            }

            for (int i = 0; i < FullLines; i++)
            {
                System.Console.WriteLine(boderLine);
                System.Console.CursorLeft = indent;
            }

            for (int i = 0; i < MarginLines; i++)
            {
                System.Console.Write(sideBorder);
                
                if (useForegroundColor)
                {
                    System.Console.ForegroundColor = oldForegroundColor;
                }

                if (useBackgroundColor)
                {
                    System.Console.BackgroundColor = oldBackgroundColor;
                }

                System.Console.Write(marginLineCenter);
                
                if (useForegroundColor)
                {
                    System.Console.ForegroundColor = ForegroundColor;
                }

                if (useBackgroundColor)
                {
                    System.Console.BackgroundColor = BackgroundColor;
                }

                System.Console.WriteLine(sideBorder);
                System.Console.CursorLeft = indent;
            }

            System.Console.Write(sideBorder);

            if (useForegroundColor)
            {
                System.Console.ForegroundColor = oldForegroundColor;
            }

            if (useBackgroundColor)
            {
                System.Console.BackgroundColor = oldBackgroundColor;
            }

            System.Console.Write(sideMargin + text + sideMargin);

            if (useForegroundColor)
            {
                System.Console.ForegroundColor = ForegroundColor;
            }

            if (useBackgroundColor)
            {
                System.Console.BackgroundColor = BackgroundColor;
            }

            System.Console.WriteLine(sideBorder);
            System.Console.CursorLeft = indent;

            for (int i = 0; i < MarginLines; i++)
            {
                System.Console.Write(sideBorder);

                if (useForegroundColor)
                {
                    System.Console.ForegroundColor = oldForegroundColor;
                }

                if (useBackgroundColor)
                {
                    System.Console.BackgroundColor = oldBackgroundColor;
                }

                System.Console.Write(marginLineCenter);

                if (useForegroundColor)
                {
                    System.Console.ForegroundColor = ForegroundColor;
                }

                if (useBackgroundColor)
                {
                    System.Console.BackgroundColor = BackgroundColor;
                }

                System.Console.WriteLine(sideBorder);
                System.Console.CursorLeft = indent;
            }

            for (int i = 0; i < FullLines; i++)
            {
                System.Console.WriteLine(boderLine);
                System.Console.CursorLeft = indent;
            }

            if (useForegroundColor)
            {
                System.Console.ForegroundColor = oldForegroundColor;
            }

            if (useBackgroundColor)
            {
                System.Console.BackgroundColor = oldBackgroundColor;
            }
        }

        /// <summary>
        /// Rendering the given text inside of a character frame. Color is not used.
        /// </summary>
        /// <param name="text">The text to put inside of the frame.</param>
        public void Render(string text)
        {
            StringBuilder sb = new StringBuilder();
            var boderLine = Char.Repeat((WidthColumns * 2) + (MarginColumns * 2) + text.Length);
            var sideBorder = Char.Repeat(WidthColumns);
            var sideMargin = ' '.Repeat(MarginColumns);

            var marginLine = sideBorder
                + sideMargin 
                + ' '.Repeat(text.Length)
                + sideMargin
                + sideBorder;

            for (int i = 0; i < FullLines; i++)
            {
                sb.AppendLine(boderLine);
            }

            for (int i = 0; i < MarginLines; i++)
            {
                sb.AppendLine(marginLine);
            }

            sb.AppendLine(sideBorder + sideMargin + text + sideMargin + sideBorder);

            for (int i = 0; i < MarginLines; i++)
            {
                sb.AppendLine(marginLine);
            }

            for (int i = 0; i < FullLines; i++)
            {
                sb.AppendLine(boderLine);
            }
        }
    }
}
