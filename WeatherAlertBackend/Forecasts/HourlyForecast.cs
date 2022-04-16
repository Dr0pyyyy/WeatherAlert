using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherAlertBackend.Forecasts
{
    public class HourlyForecast
    {
        #region Properties
        public DateTime Date { get; set; }

        public int TotalClouds { get; set; }

        public int LowClouds { get; set; }

        public int MediumClouds { get; set; }

        public int HighClouds { get; set; }

        public bool IssPassover { get; set; }

        public int Visibillity { get; set; }

        public int Fog { get; set; }

        public int PrecipitationProbability { get; set; }

        public double PrecipitationAmount { get; set; }

        public int WindSpeed { get; set; }

        public bool ChanceOfFrost { get; set; }

        public int Temperature { get; set; }

        public int FeelsLike { get; set; }

        public int DewPoint { get; set; }

        public int RelativeHumidity { get; set; }

        public int Pressure { get; set; }

        public int Ozone { get; set; }
        #endregion

        #region Constructor
        internal HourlyForecast()
        {
        }
        #endregion
    }
}
