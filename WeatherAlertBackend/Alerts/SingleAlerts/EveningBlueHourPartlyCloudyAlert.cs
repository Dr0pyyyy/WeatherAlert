using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherAlertBackend.Forecasts;
using System.Drawing;

namespace WeatherAlertBackend.Alerts.SingleAlerts
{
    public class EveningBlueHourPartlyCloudyAlert : Alert
    {
        public EveningBlueHourPartlyCloudyAlert(WeatherForecast forecast, DateTime time) : base(forecast, time)
        {
            Name = "Evening Blue Hour Party Cloudy";
            Color = Color.DarkBlue;
        }

        public override bool IsActive()
        {
            HourlyForecast hourlyForecast = GetHourlyForecast();
            return IsEveningBlueHour() && !IsRain() && hourlyForecast.TotalClouds >= 20 && hourlyForecast.TotalClouds <= 80;
        }
    }
}
