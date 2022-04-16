using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherAlertBackend.Forecasts
{
    public class DailyForecast
    {
        #region Properties
        public List<HourlyForecast> Hours { get; set; }

        public double MoonPhase { get; set; }

        public DateTime Date { get; set; }

        public DateTime? Moonrise { get; set; }

        public DateTime? Moonset { get; set; }

        public DateTime? Sunrise { get; set; }

        public DateTime? Sunset { get; set; }

        public DateTime? CivDarknessStart { get; set; }

        public DateTime? CivDarknessEnd { get; set; }

        public DateTime? NautDarknessStart { get; set; }

        public DateTime? NautDarknessEnd { get; set; }

        public DateTime? AstroDarknessStart { get; set; }

        public DateTime? AstroDarknessEnd { get; set; }
        #endregion

        #region Constructor
        internal DailyForecast()
        {
            Hours = new List<HourlyForecast>();
        }
        #endregion
    }
}
