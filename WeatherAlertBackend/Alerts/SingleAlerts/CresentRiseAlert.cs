using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherAlertBackend.Forecasts;
using System.Drawing;

namespace WeatherAlertBackend.Alerts.SingleAlerts
{
    public class CresentRiseAlert : Alert
    {
        public CresentRiseAlert(WeatherForecast forecast, DateTime time) : base(forecast, time)
        {
            Name = "Cresent Rise";
            Color = Color.LightPink;
        }

        public override bool IsActive()
        {
            DailyForecast df = GetDailyForecast();
            HourlyForecast hf = GetHourlyForecast();
            return !IsRain() && df.MoonPhase >= 10 && df.MoonPhase <= 30 && hf.TotalClouds < 20 && (df.Moonrise.Value.Hour == Time.Hour || df.Moonrise.Value.AddHours(1).Hour == Time.Hour || df.Moonrise.Value.AddHours(2).Hour == Time.Hour);
        }
    }
}
