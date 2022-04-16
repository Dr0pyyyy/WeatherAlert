using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherAlertBackend.Forecasts;
using System.Drawing;

namespace WeatherAlertBackend.Alerts.SingleAlerts
{
    public class MorningBlueHourPartlyCloudyAlert : Alert
    {
        public MorningBlueHourPartlyCloudyAlert(WeatherForecast forecast, DateTime time) : base(forecast, time)
        {
            Name = "Morning Blue Hour Party Cloudy";
            Color = Color.LightBlue;
        }

        public override bool IsActive()
        {
            HourlyForecast hourlyForecast = GetHourlyForecast();
            return IsMorningBlueHour() && !IsRain() && hourlyForecast.TotalClouds >= 20 && hourlyForecast.TotalClouds <= 80;
        }
    }
}
