using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WeatherAlertBackend.Forecasts
{
    public class WeatherForecast
    {
        #region Properties
        public List<DailyForecast> Days { get; set; }
        public double Latitude { get; private set; }
        #endregion

        #region Constructor
        public WeatherForecast(double longitude, double latitude)
        {
            Days = new List<DailyForecast>();
            Latitude = latitude;

            //Download data from web and split it day by day
            WebClient webClient = new WebClient();
            string html = webClient.DownloadString("https://clearoutside.com/forecast/" + longitude + "/" + latitude);

            List<string> days = new List<string>();
            for (int i = 0; i < 7; i++)
            {
                days.Add(GetDayHtml(html, i));
            }

            GetData(days);
        }

        #endregion

        #region Methods

        private string GetDayHtml(string fullHTML, int day)
        {
            int startIndex = fullHTML.IndexOf("<div class=\"fc_day\" id=\"day_" + day + "\"");
            int endIndex = day != 6 ? fullHTML.IndexOf("<div class=\"fc_day\" id=\"day_" + (day + 1) + "\"") : fullHTML.Length;
            return fullHTML.Substring(startIndex, endIndex - startIndex);
        }

        private void GetData(List<string> days)
        {
            for (int i = 0; i < days.Count; i++)
            {
                DailyForecast dailyForecast = new DailyForecast();
                //Makes it readable and make arr with all lines of day
                days[i] = days[i].Replace(" ", string.Empty);
                
                string[] allLines = days[i].Split(new[] { '\r', '\n' });

                //Hourlyforecast here
                int dataPosition = 4;
                int timeDataPosition = 8;
                for (int hour = 0; hour < 24; hour++)
                {
                    HourlyForecast hourlyForecast = new HourlyForecast();
                    //Going line by line
                    for (int j = 0; j < allLines.Length; j++)
                    {
                        //Searching for data
                        string[] hourDataName = allLines[j].Split('"');
                        for (int k = 0; k < hourDataName.Length; k++)
                        {
                            if (hourDataName[k] == "fc_hoursfc_hour_ratings")
                            {
                                string[] dataValue = allLines[j + 2].Split('>', '<');
                                DateTime time = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, Convert.ToInt32(dataValue[timeDataPosition]), 0, 0);
                                time = time.AddDays(i);
                                if (time.Hour < hour)
                                {
                                    time = time.AddDays(1);
                                }
                                hourlyForecast.Date = time;
                                timeDataPosition += 12;
                                hourlyForecast.Date = hourlyForecast.Date.AddMinutes((-1) * hourlyForecast.Date.Minute).AddSeconds((-1) * hourlyForecast.Date.Second);
                            }
                            else if (hourDataName[k] == "><span>TotalClouds(%SkyObscured)</span></span>")
                            {
                                string[] dataValue = allLines[j + 4].Split('>', '<');
                                hourlyForecast.TotalClouds = Convert.ToInt32(dataValue[dataPosition]);
                            }
                            else if (hourDataName[k] == "><span>LowClouds(%SkyObscured)</span></span>")
                            {
                                string[] dataValue = allLines[j + 4].Split('>', '<');
                                hourlyForecast.LowClouds = Convert.ToInt32(dataValue[dataPosition]);
                            }
                            else if (hourDataName[k] == "><span>MediumClouds(%SkyObscured)</span></span>")
                            {
                                string[] dataValue = allLines[j + 4].Split('>', '<');
                                hourlyForecast.MediumClouds = Convert.ToInt32(dataValue[dataPosition]);
                            }
                            else if (hourDataName[k] == "><span>HighClouds(%SkyObscured)</span></span>")
                            {
                                string[] dataValue = allLines[j + 4].Split('>', '<');
                                hourlyForecast.HighClouds = Convert.ToInt32(dataValue[dataPosition]);
                            }
                            else if (hourDataName[k] == "><span>ISSPassover</span></span>")
                            {
                                string[] dataValue = allLines[j + 4].Split('/', '"');
                                for (int a = 0; a < dataValue.Length; a++)
                                {
                                    if (dataValue[a] == "fc_none")
                                    {
                                        hourlyForecast.IssPassover = false;
                                    }
                                    else if (dataValue[a] == "fc_iss")
                                    {
                                        hourlyForecast.IssPassover = true;
                                    }
                                }
                            }
                            else if (hourDataName[k] == "><span>Visibility(miles)</span></span>")
                            {
                                string[] dataValue = allLines[j + 4].Split('>', '<');
                                hourlyForecast.Visibillity = Convert.ToInt32(dataValue[dataPosition]);
                            }
                            else if (hourDataName[k] == "><span>Fog(%)</span></span>")
                            {
                                string[] dataValue = allLines[j + 4].Split('>', '<');
                                hourlyForecast.Fog = Convert.ToInt32(dataValue[dataPosition]);
                            }
                            else if (hourDataName[k] == "><span>PrecipitationProbability(%)</span></span>")
                            {
                                string[] dataValue = allLines[j + 4].Split('>', '<');
                                hourlyForecast.PrecipitationProbability = Convert.ToInt32(dataValue[dataPosition]);
                            }
                            else if (hourDataName[k] == "><span>PrecipitationAmount(mm)</span></span>")
                            {
                                string[] dataValue = allLines[j + 4].Split('>', '<');
                                hourlyForecast.PrecipitationAmount = Convert.ToDouble(dataValue[dataPosition], Database.GlobalCultureInfo);
                            }
                            else if (hourDataName[k] == "><span>WindSpeed/Direction(mph)</span></span>")
                            {
                                string[] dataValue = allLines[j + 4].Split('>', '<');
                                hourlyForecast.WindSpeed = Convert.ToInt32(dataValue[6]);
                            }
                            else if (hourDataName[k] == "><span>ChanceofFrost</span></span>")
                            {
                                string[] dataValue = allLines[j + 4].Split('/', '"');
                                for (int a = 0; a < dataValue.Length; a++)
                                {
                                    if (dataValue[a] == "fc_none")
                                    {
                                        hourlyForecast.ChanceOfFrost = false;
                                    }
                                    else if (dataValue[a] == "climacon snowflake")
                                    {
                                        hourlyForecast.ChanceOfFrost = true;
                                    }
                                }
                            }
                            else if (hourDataName[k] == "><span>Temperature(&deg;C)</span></span>")
                            {
                                string[] dataValue = allLines[j + 4].Split('>', '<');
                                hourlyForecast.Temperature = Convert.ToInt32(dataValue[dataPosition]);
                            }
                            else if (hourDataName[k] == "><span>FeelsLike(&deg;C)</span></span>")
                            {
                                string[] dataValue = allLines[j + 4].Split('>', '<');
                                hourlyForecast.FeelsLike = Convert.ToInt32(dataValue[dataPosition]);
                            }
                            else if (hourDataName[k] == "><span>DewPoint(&deg;C)</span></span>")
                            {
                                string[] dataValue = allLines[j + 4].Split('>', '<');
                                hourlyForecast.DewPoint = Convert.ToInt32(dataValue[dataPosition]);
                            }
                            else if (hourDataName[k] == "><span>RelativeHumidity(%)</span></span>")
                            {
                                string[] dataValue = allLines[j + 4].Split('>', '<');
                                hourlyForecast.RelativeHumidity = Convert.ToInt32(dataValue[dataPosition]);
                            }
                            else if (hourDataName[k] == "><span>Pressure(mb)</span></span>")
                            {
                                string[] dataValue = allLines[j + 4].Split('>', '<');
                                hourlyForecast.Pressure = Convert.ToInt32(dataValue[dataPosition]);
                            }
                            else if (hourDataName[k] == "><span>Ozone(du)</span></span>")
                            {
                                string[] dataValue = allLines[j + 4].Split('>', '<');
                                hourlyForecast.Ozone = Convert.ToInt32(dataValue[dataPosition]);
                            }
                        }
                    }
                    dailyForecast.Hours.Add(hourlyForecast);
                    dataPosition += 4;
                }

                //Dailyforecast here
                for (int a = 0; a < allLines.Length; a++)
                {
                    string[] dayDataName = allLines[a].Split('"');
                    for (int b = 0; b < dayDataName.Length; b++)
                    {
                        if (dayDataName[b] == "fc_moon_percentage")
                        {
                            string[] dataValue = allLines[a].Split('>', '%');
                            dailyForecast.MoonPhase = Convert.ToDouble(dataValue[1], Database.GlobalCultureInfo);
                        }
                        else if (dayDataName[b] == "fc_moon_riseset")
                        {
                            //DataValuePosition is variable used here, because HTML code can have longer or shorter version
                            string[] dataValue = allLines[4].Split('>', '<', '"');
                            int dataValuePosition = 32;
                            if (dataValue.Length < 32)
                            {
                                dataValuePosition = 16;
                            }
                                if (dataValue[dataValuePosition] == "")
                                {
                                    dailyForecast.Moonrise = null;
                                }
                                else
                                {
                                    char[] chars = dataValue[dataValuePosition].ToCharArray();
                                    int counter = 0;
                                    string time = "";
                                    string date = "";
                                    foreach (char c in chars)
                                    {
                                        Convert.ToString(c);
                                        counter++;
                                        if (counter <= 5)
                                        {
                                            time += c;
                                        }
                                        else
                                        {
                                            date += c;
                                        }
                                    }
                                    dailyForecast.Moonrise = DateTime.ParseExact(date + " " + time, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
                                }
                                dataValuePosition = 38;
                                if (dataValue.Length < dataValuePosition)
                                {
                                    dataValuePosition = 22;
                                }
                                if (dataValue[38] == "")
                                {
                                    dailyForecast.Moonset = null;
                                }
                                else
                                {
                                    char[] chars = dataValue[dataValuePosition].ToCharArray();
                                    int counter = 0;
                                    string time = "";
                                    string date = "";
                                    foreach (char c in chars)
                                    {
                                        Convert.ToString(c);
                                        counter++;

                                        if (counter <= 5)
                                        {
                                            time += c;
                                        }
                                        else
                                        {
                                            date += c;
                                        }
                                    }
                                    dailyForecast.Moonset = DateTime.ParseExact(date + " " + time, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
                                }
                        }
                        else if (dayDataName[b] == "fc_daylight")
                        {
                            string[] dataValue = allLines[a].Split('>', '<', '&', '-');
                            if (dataValue[9] == "N/A")
                            {
                                dailyForecast.Sunrise = null;
                            }
                            else
                            {
                                dailyForecast.Sunrise = Convert.ToDateTime(dataValue[9]).AddDays(i + 1);
                            }
                            if (dataValue[17] == "N/A")
                            {
                                dailyForecast.Sunset = null;
                            }
                            else
                            {
                                dailyForecast.Sunset = Convert.ToDateTime(dataValue[17]).AddDays(i);
                            }
                            if (dataValue[29] == "N/A")
                            {
                                dailyForecast.CivDarknessStart = null;
                            }
                            else
                            {
                                dailyForecast.CivDarknessStart = Convert.ToDateTime(dataValue[29]).AddDays(i);
                            }
                            if (dataValue[30] == "N/A")
                            {
                                dailyForecast.CivDarknessEnd = null;
                            }
                            else
                            {
                                dailyForecast.CivDarknessEnd = Convert.ToDateTime(dataValue[30]).AddDays(i + 1);
                            }
                            if (dataValue[36] == "N/A")
                            {
                                dailyForecast.NautDarknessStart = null;
                            }
                            else
                            {
                                dailyForecast.NautDarknessStart = Convert.ToDateTime(dataValue[36]).AddDays(i);
                            }
                            if (dataValue[37] == "N/A")
                            {
                                dailyForecast.NautDarknessEnd = null;
                            }
                            else
                            {
                                dailyForecast.NautDarknessEnd = Convert.ToDateTime(dataValue[37]).AddDays(i + 1);
                            }
                            if (dataValue[43] == "N/A")
                            {
                                dailyForecast.AstroDarknessStart = null;
                            }
                            else
                            {
                                dailyForecast.AstroDarknessStart = Convert.ToDateTime(dataValue[43]).AddDays(i);
                            }
                            string[] astroDarknessdata = dataValue[44].Split('"');
                            if (astroDarknessdata[0] == "N/A")
                            {
                                dailyForecast.AstroDarknessEnd = null;
                            }
                            else
                            {
                                dailyForecast.AstroDarknessEnd = Convert.ToDateTime(astroDarknessdata[0]).AddDays(i + 1);
                            }
                            dailyForecast.Date = DateTime.Today.AddDays(i);
                        }
                    }
                }
                Days.Add(dailyForecast);
            }
        }
        #endregion
    }
}
