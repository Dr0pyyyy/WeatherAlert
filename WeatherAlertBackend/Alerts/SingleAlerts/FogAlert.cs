using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherAlertBackend.Forecasts;
using System.Drawing;

namespace WeatherAlertBackend.Alerts.SingleAlerts
{
    public class FogAlert : Alert
    {
        public FogAlert(WeatherForecast forecast, DateTime time) : base(forecast, time)
        {
            Name = "Fog";
            Color = Color.LightGray;
        }

        public override bool IsActive()
        {
            return !IsRain() && GetHourlyForecast().Fog >= 30 && !IsCivDark();
        }
    }
}
