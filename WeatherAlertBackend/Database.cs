using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Globalization;
using WeatherAlertBackend.Alerts;

namespace WeatherAlertBackend
{
    public class Database
    {
        #region Properties
        public List<Place> Places { get; set; }

        public static IFormatProvider GlobalCultureInfo = new CultureInfo("en-US");
        #endregion

        #region Constructor
        public Database()
        {
            Places = new List<Place>();
        }

        public Database(string path) : this()
        {
            string data = File.ReadAllText(path);

            string[] dataLines = data.Split('\n');
            for (int i = 0; i < dataLines.Length; i++)
            {
                if(String.IsNullOrWhiteSpace(dataLines[i]))
                {
                    continue;
                }

                string[] dataLoad = dataLines[i].Split(';');

                Places.Add(new Place(dataLoad[0],
                    Convert.ToDouble(dataLoad[1], Database.GlobalCultureInfo),
                    Convert.ToDouble(dataLoad[2], Database.GlobalCultureInfo),
                    (AlertList)Convert.ToInt32(dataLoad[3])));
            }
        }

        public void SaveToFile(string path)
        {
                string data = null;
                for (int i = 0; i < Places.Count; i++)
                {
                    data += Places[i].Name + ";" + 
                        Convert.ToString(Places[i].Longitude, Database.GlobalCultureInfo) + ";" + 
                        Convert.ToString(Places[i].Latitude, Database.GlobalCultureInfo) +  ";" +
                        (int)Places[i].WatchedAlerts + "\n";
                }

                if (!String.IsNullOrEmpty(Path.GetDirectoryName(path)) && !Directory.Exists(Path.GetDirectoryName(path)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(path));
                }

                File.WriteAllText(path, data);
        }
        #endregion
    }
}
