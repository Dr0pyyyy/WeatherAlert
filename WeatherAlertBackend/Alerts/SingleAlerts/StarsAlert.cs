using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherAlertBackend.Forecasts;
using System.Drawing;

namespace WeatherAlertBackend.Alerts.SingleAlerts
{
    public class StarsAlert : Alert
    {
        public StarsAlert(WeatherForecast forecast, DateTime time) : base(forecast, time)
        {
            Name = "Stars";
            Color = Color.Yellow;
        }

        public override bool IsActive()
        {
            HourlyForecast hourlyForecast = GetHourlyForecast();
            return !IsRain() && hourlyForecast.TotalClouds <= 10 && IsAstroDark() && !IsMoonUp();
        }
    }
}
