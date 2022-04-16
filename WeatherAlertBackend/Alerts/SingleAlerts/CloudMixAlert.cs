using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherAlertBackend.Forecasts;
using System.Drawing;

namespace WeatherAlertBackend.Alerts.SingleAlerts
{
    public class CloudMixAlert : Alert
    {
        public CloudMixAlert(WeatherForecast forecast, DateTime time) : base(forecast, time)
        {
            Name = "Cloud Mix";
            Color = Color.DarkGray;
        }

        public override bool IsActive()
        {
            HourlyForecast hourlyForecast = GetHourlyForecast();
            return !IsRain() && hourlyForecast.LowClouds <= 70 && hourlyForecast.TotalClouds >= 90 && !IsCivDark();
        }
    }
}