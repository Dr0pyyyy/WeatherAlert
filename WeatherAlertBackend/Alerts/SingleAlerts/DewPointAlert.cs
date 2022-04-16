using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherAlertBackend.Forecasts;
using System.Drawing;

namespace WeatherAlertBackend.Alerts.SingleAlerts
{
    public class DewPointAlert : Alert
    {
        public DewPointAlert(WeatherForecast forecast, DateTime time) : base(forecast, time)
        {
            Name = "Dew Point";
            Color = Color.LightGray;
        }

        public override bool IsActive()
        {
            HourlyForecast hourlyForecast = GetHourlyForecast();
            return !IsRain() && hourlyForecast.DewPoint >= hourlyForecast.Temperature && !IsCivDark();
        }
    }
}
