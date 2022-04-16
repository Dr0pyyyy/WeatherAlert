using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherAlertBackend.Forecasts;
using System.Drawing;

namespace WeatherAlertBackend.Alerts.SingleAlerts
{
    public class CresentSetAlert : Alert
    {
        public CresentSetAlert(WeatherForecast forecast, DateTime time) : base(forecast, time)
        {
            Name = "Cresent Set";
            Color = Color.Pink;
        }

        public override bool IsActive()
        {
            DailyForecast df = GetDailyForecast();
            HourlyForecast hf = GetHourlyForecast();
            return !IsRain() && df.MoonPhase >= 10 && df.MoonPhase <= 30 && hf.TotalClouds < 20 && (Time.Hour == df.Moonset.Value.Hour || Time.Hour == df.Moonset.Value.AddHours(-1).Hour || Time.Hour == df.Moonset.Value.AddHours(-2).Hour);
        }
    }
}
