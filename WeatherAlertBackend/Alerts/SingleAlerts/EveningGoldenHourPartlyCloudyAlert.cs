using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherAlertBackend.Forecasts;
using System.Drawing;

namespace WeatherAlertBackend.Alerts.SingleAlerts
{
    public class EveningGoldenHourPartlyCloudyAlert : Alert
    {
        public EveningGoldenHourPartlyCloudyAlert(WeatherForecast forecast, DateTime time) : base(forecast, time)
        {
            Name = "Evening Golden Hour Party Cloudy";
            Color = Color.DarkOrange;
        }

        public override bool IsActive()
        {
            HourlyForecast hourlyForecast = GetHourlyForecast();
            return IsEveningGoldenHour() && !IsRain() && hourlyForecast.TotalClouds >= 20 && hourlyForecast.TotalClouds <= 80 && IsEveningGoldenHour();
        }
    }
}
