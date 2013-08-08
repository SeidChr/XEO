using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XEO.Console;
using XEO.Core;
using XEO.Facebook;

namespace TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Factory<IEnvironment>.Instance.WriteToConsole = true;
            var toolbox = Factory<IFacebookToolbox>.Instance;

            var frame = new CharFrame();
            frame.Char = '#';
            frame.WidthColumns = 3;
            frame.MarginColumns = 1;
            frame.MarginLines = 1;
            frame.FullLines = 1;
            frame.ForegroundColor = ConsoleColor.DarkGreen;

            Console.CursorVisible = false;
            Console.CursorLeft = 3;
            Console.CursorTop = 1;

            frame.Print("XEO-Test-Console");
            Console.WriteLine();

            Console.CursorLeft = 3;

            //var session = toolbox.Login("hamburgneulinge@kurzepost.de", "efJOgiFTr0mEBmGmLNpZ");

            //TestGetPastEvents(session);
            //TestGetBigEventById(session);

            GetBrowserCookies();

            Console.ReadKey(true);
        }

        static void GetBrowserCookies()
        {
            var chromeDefaultCookiesPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "Google",
                "Chrome",
                "User Data",
                "Default", 
                "Cookies");

            var conn = SQLiteFactory.Instance.CreateConnection();
            conn.ConnectionString =  "Data Source=" + chromeDefaultCookiesPath;
            conn.Open();
            var command = conn.CreateCommand();
            command.CommandText = "SELECT * FROM Cookies";
            var reader = command.ExecuteReader();
            
            var values = new object[reader.FieldCount];
            while (reader.NextResult())
            {
                reader.GetValues(values);
                foreach (var value in values)
                {
                    Console.WriteLine(value.ToString());
                }
            }

        }

        static void TestGetPastEvents(IFacebookSession session)
        {
            var selector = new Selector();
            
            var lastEvents = session.GetBasicPastEventsData();

            var selectedEvent = selector.Select(lastEvents, e => e.Title);
            if (selectedEvent != null)
            {
                PrintEventParticipants(selectedEvent);
            }
            else 
            {
                Console.WriteLine("Kein gültiges Event gewählt.");
            }
        }

        static void TestGetBigEventById(IFacebookSession session)
        {
            var harleyDays = session.GetEvent("377496955695973");
            PrintEventParticipants(harleyDays);
        }

        static void PrintEventParticipants(IFacebookEvent e)
        {
            Console.WriteLine(e.Title + " (" + e.Id + ")");
            var users = e.GetGoingUsers();
            foreach (var user in users)
            {
                Console.WriteLine("   + " + user.SinglePartName + " (" + user.ProfileUrl + ")");
            }
        }
    }
}
