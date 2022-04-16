using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherAlertBackend;
using WeatherAlertBackend.Alerts;

namespace Testing
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Database db = new Database();
                db.Places.Add(new Place("Prostejov", 17.11, 49.47, AlertList.Fog));
                db.Places.Add(new Place("Mikuluvka", 17.93, 49.41, AlertList.Fog));

                bool isAlert = false;
                foreach (Place p in db.Places)
                {
                    foreach(Alert a in p.ActiveAlerts)
                    {
                        isAlert = true;
                        Console.ForegroundColor = ColorToConsoleColor(a.Color);
                        Console.WriteLine(p.Name + " - " + a.Time.ToString("d. M. yyyy H:mm") + " " + a.Name);
                    }
                }

                if(!isAlert)
                {
                    Console.WriteLine("No alert is active.");
                }
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.Message);
            }
            Console.ReadKey();
        }

        private static ConsoleColor ColorToConsoleColor(Color c)
        {
            int index = (c.R > 128 | c.G > 128 | c.B > 128) ? 8 : 0;
            index |= (c.R > 64) ? 4 : 0;
            index |= (c.G > 64) ? 2 : 0;
            index |= (c.B > 64) ? 1 : 0;
            return (System.ConsoleColor)index;
        }
    }
}
