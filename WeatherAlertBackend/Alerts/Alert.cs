using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherAlertBackend.Forecasts;

namespace WeatherAlertBackend.Alerts
{
    public abstract class Alert
    {
        private enum TimeStampType
        {
            Start,
            End,
            StartAfter,
            EndAfter,
            None
        }

        private enum Season
        {
            Summer,
            Winter
        }

        #region Properties
        public string Name { get; protected set; }
        public Color Color { get; protected set; }
        public DateTime Time { get; protected set; }
        public WeatherForecast Forecast { get; protected set; }
        #endregion

        #region Constructor
        public Alert(WeatherForecast forecast, DateTime time)
        {
            Forecast = forecast;
            Time = time;
        }
        #endregion

        #region Methods
        public abstract bool IsActive();

        protected bool IsRain()
        {
            HourlyForecast hourlyForecast = GetHourlyForecast();
            return hourlyForecast.PrecipitationProbability >= 30 && hourlyForecast.Temperature > 0;
        }

        protected bool IsMorningBlueHour()
        {
            foreach (DailyForecast day in Forecast.Days)
            {
                if (day.Sunrise != null)
                {
                    //Sunset time check
                    DateTime sunrise = day.Sunrise.Value;
                    if (Time.Date == sunrise.Date && Time.Hour == sunrise.Hour)
                    {
                        return true;
                    }

                    //Check 30 minutes before sunrise
                    sunrise = sunrise.AddMinutes(-30);
                    if (Time.Date == sunrise.Date && Time.Hour == sunrise.Hour)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        protected bool IsMorningGoldenHour()
        {
            foreach (DailyForecast day in Forecast.Days)
            {
                if (day.Sunrise != null)
                {
                    //Sunset time check
                    DateTime sunrise = day.Sunrise.Value;
                    if (Time.Date == sunrise.Date && Time.Hour == sunrise.Hour)
                    {
                        return true;
                    }

                    //Check 30 minutes after sunrise
                    sunrise = sunrise.AddMinutes(30);
                    if (Time.Date == sunrise.Date && Time.Hour == sunrise.Hour)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        protected bool IsEveningBlueHour()
        {
            foreach (DailyForecast day in Forecast.Days)
            {
                if (day.Sunset != null)
                {
                    //Sunset time check
                    DateTime sunset = day.Sunset.Value;
                    if (Time.Date == sunset.Date && Time.Hour == sunset.Hour)
                    {
                        return true;
                    }

                    //Check 30 minutes after sunset
                    sunset = sunset.AddMinutes(30);
                    if (Time.Date == sunset.Date && Time.Hour == sunset.Hour)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        protected bool IsEveningGoldenHour()
        {
            foreach (DailyForecast day in Forecast.Days)
            {
                if (day.Sunset != null)
                {
                    //Sunset time check
                    DateTime sunset = day.Sunset.Value;
                    if (Time.Date == sunset.Date && Time.Hour == sunset.Hour)
                    {
                        return true;
                    }

                    //Check 30 minutes before sunset
                    sunset = sunset.AddMinutes(-30);
                    if (Time.Date == sunset.Date && Time.Hour == sunset.Hour)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private Tuple<DateTime?, TimeStampType> GetCivDarknessTimeStamp()
        {
            //Find a civDark timestamp which is occuring before desired Time
            for (int i = Forecast.Days.Count - 1; i >= 0; i--)
            {
                if (Forecast.Days[i].CivDarknessEnd.HasValue && ResetMinutes(Forecast.Days[i].CivDarknessEnd.Value) <= Time)
                {
                    return new Tuple<DateTime?, TimeStampType>(ResetMinutes(Forecast.Days[i].CivDarknessEnd.Value), TimeStampType.End);
                }
                if (Forecast.Days[i].CivDarknessStart.HasValue && Forecast.Days[i].CivDarknessStart <= Time)
                {
                    return new Tuple<DateTime?, TimeStampType>(Forecast.Days[i].CivDarknessStart, TimeStampType.Start);
                }
            }

            //Not found any timestamp before
            //Try to find first timestamp after
            for (int i = 0; i < Forecast.Days.Count; i++)
            {
                if (Forecast.Days[i].CivDarknessStart.HasValue && Forecast.Days[i].CivDarknessStart >= Time)
                {
                    return new Tuple<DateTime?, TimeStampType>(Forecast.Days[i].CivDarknessStart, TimeStampType.StartAfter);
                }
                if (Forecast.Days[i].CivDarknessEnd.HasValue && Forecast.Days[i].CivDarknessEnd >= Time)
                {
                    return new Tuple<DateTime?, TimeStampType>(Forecast.Days[i].CivDarknessEnd, TimeStampType.EndAfter);
                }
            }

            //Not found any timestamp at all
            return new Tuple<DateTime?, TimeStampType>(null, TimeStampType.None);
        }

        private Tuple<DateTime?, TimeStampType> GetAstroDarknessTimeStamp()
        {
            //Find an astroDark timestamp which is occuring before desired Time
            for (int i = Forecast.Days.Count - 1; i >= 0; i--)
            {
                if (Forecast.Days[i].AstroDarknessEnd.HasValue && Forecast.Days[i].AstroDarknessEnd.Value <= Time)
                {
                    return new Tuple<DateTime?, TimeStampType>(Forecast.Days[i].AstroDarknessEnd.Value, TimeStampType.End);
                }
                if (Forecast.Days[i].AstroDarknessStart.HasValue && ResetMinutes(Forecast.Days[i].AstroDarknessStart.Value) <= Time)
                {
                    return new Tuple<DateTime?, TimeStampType>(ResetMinutes(Forecast.Days[i].AstroDarknessStart.Value), TimeStampType.Start);
                }
            }

            //Not found any timestamp before
            //Try to find first timestamp after
            for (int i = 0; i < Forecast.Days.Count; i++)
            {
                if (Forecast.Days[i].AstroDarknessStart.HasValue && Forecast.Days[i].AstroDarknessStart >= Time)
                {
                    return new Tuple<DateTime?, TimeStampType>(Forecast.Days[i].AstroDarknessStart, TimeStampType.StartAfter);
                }
                if (Forecast.Days[i].AstroDarknessEnd.HasValue && Forecast.Days[i].AstroDarknessEnd >= Time)
                {
                    return new Tuple<DateTime?, TimeStampType>(Forecast.Days[i].AstroDarknessEnd, TimeStampType.EndAfter);
                }
            }

            //Not found any timestamp at all
            return new Tuple<DateTime?, TimeStampType>(null, TimeStampType.None);
        }

        private Tuple<DateTime?, TimeStampType> GetMoonUpTimeStamp()
        {
            //Find an moonset or moonrise timestamp which is occuring before desired Time
            for (int i = Forecast.Days.Count - 1; i >= 0; i--)
            {
                if (Forecast.Days[i].Moonset.HasValue && Forecast.Days[i].Moonset.Value <= Time)
                {
                    return new Tuple<DateTime?, TimeStampType>(Forecast.Days[i].Moonset.Value, TimeStampType.End);
                }
                if (Forecast.Days[i].Moonrise.HasValue && ResetMinutes(Forecast.Days[i].Moonrise.Value) <= Time)
                {
                    return new Tuple<DateTime?, TimeStampType>(ResetMinutes(Forecast.Days[i].Moonrise.Value), TimeStampType.Start);
                }
            }

            //Not found any timestamp before
            //Try to find first timestamp after
            for (int i = 0; i < Forecast.Days.Count; i++)
            {
                if (Forecast.Days[i].Moonrise.HasValue && Forecast.Days[i].Moonrise >= Time)
                {
                    return new Tuple<DateTime?, TimeStampType>(Forecast.Days[i].Moonrise, TimeStampType.StartAfter);
                }
                if (Forecast.Days[i].Moonset.HasValue && Forecast.Days[i].Moonset >= Time)
                {
                    return new Tuple<DateTime?, TimeStampType>(Forecast.Days[i].Moonset, TimeStampType.EndAfter);
                }
            }

            //Not found any timestamp at all
            return new Tuple<DateTime?, TimeStampType>(null, TimeStampType.None);
        }

        private DateTime ResetMinutes(DateTime time)
        {
            return new DateTime(time.Year, time.Month, time.Day, time.Hour, 0, 0);
        }

        private Season GetSeason()
        {
            //Find season for northern hemisphere
            Season northernHemisphere;
            switch (Time.Month)
            {
                case 4:
                case 5:
                case 6:
                case 7:
                case 8:
                    northernHemisphere = Season.Summer;
                    break;
                case 10:
                case 11:
                case 12:
                case 1:
                case 2:
                    northernHemisphere = Season.Winter;
                    break;
                case 3:
                    northernHemisphere = Time.Day < 21 ? Season.Winter : Season.Summer;
                    break;
                case 9:
                    northernHemisphere = Time.Day < 23 ? Season.Summer : Season.Winter;
                    break;
                default:
                    throw new Exception($"Unknown month {Time.Month}.");
            }
            //correct for southern hemisphere
            if (Forecast.Latitude < 0)
            {
                return (northernHemisphere == Season.Summer) ? Season.Winter : Season.Summer;
            }
            //Else
            return northernHemisphere;
        }

        protected bool IsCivDark()
        {
            //find the last predecessing civil darknes marker
            Tuple<DateTime?, TimeStampType> tsMark = GetCivDarknessTimeStamp();

            switch (tsMark.Item2)
            {
                //IF no marker is before selected time but start marker is after selected time
                case TimeStampType.StartAfter:
                    //If first day has value - might be evaluating in the night - check for approximate end of night
                    if (Forecast.Days[0].CivDarknessEnd.HasValue)
                    {
                        //Create new approximate DateTime for first end of civil darknes, based on the next day data
                        DateTime end = Forecast.Days[0].CivDarknessEnd.Value.AddDays(-1);
                        //Reset minutes and seconds for better comparison
                        return Time < ResetMinutes(end);
                    }
                    //If the first day does not have value - polar night
                    else
                    {
                        return true;
                    }
                //IF no marker is before selected time but end marker is after the selected time, it is dark
                case TimeStampType.EndAfter:
                    //Reset minutes and seconds for better comparison
                    return Time < tsMark.Item1.Value;
                //IF no marker is ever found - polar night or day the whole week - evaluate based on time of year and latitude
                case TimeStampType.None:
                    Season season = GetSeason();
                    return season == Season.Winter;
                //IF previous mark exists - check its type to decide upon darkness
                case TimeStampType.Start:
                    return true;
                case TimeStampType.End:
                    return false;
                default:
                    throw new Exception($"Unknown TimeStampType: {tsMark.Item2}.");
            }
        }

        protected HourlyForecast GetHourlyForecast()
        {
            for (int i = 0; i < Forecast.Days.Count; i++)
            {
                for (int j = 0; j < Forecast.Days[i].Hours.Count; j++)
                {
                    if (Forecast.Days[i].Hours[j].Date.Day == Time.Day && Forecast.Days[i].Hours[j].Date.Hour == Time.Hour)
                    {
                        return Forecast.Days[i].Hours[j];
                    }
                }
            }
            throw new Exception($"GetHourlyForecast: Time {Time} not found.");
        }

        protected DailyForecast GetDailyForecast()
        {
            for (int i = 0; i < Forecast.Days.Count; i++)
            {
                for (int j = 0; j < Forecast.Days[i].Hours.Count; j++)
                {
                    if (Forecast.Days[i].Hours[j].Date.Day == Time.Day && Forecast.Days[i].Hours[j].Date.Hour == Time.Hour)
                    {
                        return Forecast.Days[i];
                    }
                }
            }
            throw new Exception($"GetDailyForecast: Day {Time} not found.");
        }

        protected bool IsAstroDark()
        {
            //find the last predecessing astronomical darknes marker
            Tuple<DateTime?, TimeStampType> tsMark = GetAstroDarknessTimeStamp();

            switch (tsMark.Item2)
            {
                //IF no marker is before selected time but start marker is after selected time
                case TimeStampType.StartAfter:
                    //If first day has value - might be evaluating in the night - check for approximate end of night
                    if (Forecast.Days[0].AstroDarknessEnd.HasValue)
                    {
                        //Create new approximate DateTime for first end of astronomical darknes, based on the next day data
                        DateTime end = Forecast.Days[0].AstroDarknessEnd.Value.AddDays(-1);
                        //Reset minutes and seconds for better comparison
                        return Time <= ResetMinutes(end);
                    }
                    //If the first day does not have value - polar night
                    else
                    {
                        return true;
                    }
                //IF no marker is before selected time but end marker is after the selected time, it is dark
                case TimeStampType.EndAfter:
                    //Reset minutes and seconds for better comparison
                    return Time <= tsMark.Item1.Value;
                //IF no marker is ever found - polar night or day the whole week - evaluate based on time of year and latitude
                case TimeStampType.None:
                    Season season = GetSeason();
                    return season == Season.Winter;
                //IF previous mark exists - check its type to decide upon darkness
                case TimeStampType.Start:
                    return true;
                case TimeStampType.End:
                    return false;
                default:
                    throw new Exception($"Unknown TimeStampType: {tsMark.Item2}.");
            }
        }

        protected bool IsMoonUp()
        {
            //find the last predecessing moonrise/moonset marker
            Tuple<DateTime?, TimeStampType> tsMark = GetMoonUpTimeStamp();

            switch (tsMark.Item2)
            {
                //IF no marker is before selected time but start marker is after selected time
                case TimeStampType.StartAfter:
                    //If first day has value - might be evaluating in the night - check for approximate end of night
                    if (Forecast.Days[0].Moonset.HasValue)
                    {
                        //Create new approximate DateTime for first moonset, based on the next day data
                        DateTime end = Forecast.Days[0].Moonset.Value.AddDays(-1);
                        //Reset minutes and seconds for better comparison
                        return Time <= ResetMinutes(end);
                    }
                    //If the first day does not have value - polar night
                    else
                    {
                        return true;
                    }
                //IF no marker is before selected time but end marker is after the selected time, it is down
                case TimeStampType.EndAfter:
                    //Reset minutes and seconds for better comparison
                    return Time <= tsMark.Item1.Value;
                //IF no marker is ever found - polar night or day the whole week - don't know how to evaluate
                case TimeStampType.None:
                    //I don't know how to properly evalutate this
                    return false;
                //IF previous mark exists - check its type to decide
                case TimeStampType.Start:
                    return true;
                case TimeStampType.End:
                    return false;
                default:
                    throw new Exception($"Unknown TimeStampType: {tsMark.Item2}.");
            }
        }
        #endregion
    }
}
