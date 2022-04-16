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
        #region Properties
        public string Name { get; protected set; }
        public Color Color { get; protected set; }
        public DateTime Time { get; protected set; }
        #endregion

        #region Constructor
        public Alert(DateTime time)
        {
            Time = time;
        }
        #endregion

        #region Methods
        public abstract bool IsActive(WeatherForecast forecast);

        protected bool IsRain(WeatherForecast forecast)
        {
            for (int i = 0; i < forecast.Days.Count; i++)
            {
                for (int j = 0; j < forecast.Days[i].Hours.Count; j++)
                {
                    if(forecast.Days[i].Hours[j].Date.Day == Time.Day && forecast.Days[i].Hours[j].Date.Hour == Time.Hour)
                    {
                        if (forecast.Days[i].Hours[j].PrecipitationProbability >= 30 && forecast.Days[i].Hours[j].Temperature > 0)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
            }
            throw new Exception ("Time " + Time + " is not in database!");
        }
        #endregion
    }
}
