using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherAlertBackend.Forecasts;
using System.Drawing;

namespace WeatherAlertBackend.Alerts.SingleAlerts
{
    public class MorningGoldenHourPartlyCloudyAlert : Alert
    {
        public MorningGoldenHourPartlyCloudyAlert(WeatherForecast forecast, DateTime time) : base(forecast, time)
        {
            Name = "Morning Golden Hour Party Cloudy";
            Color = Color.Orange;
        }

        public override bool IsActive()
        {
            HourlyForecast hourlyForecast = GetHourlyForecast();
            return IsMorningGoldenHour() && !IsRain() && hourlyForecast.TotalClouds >= 20 && hourlyForecast.TotalClouds <= 80;
        }
    }
}
