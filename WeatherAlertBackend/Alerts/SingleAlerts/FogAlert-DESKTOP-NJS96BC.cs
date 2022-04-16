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
        public FogAlert(DateTime time) : base(time)
        {
            Name = "Fog";
            Color = Color.LightGray;
        }

        public override bool IsActive(WeatherForecast forecast)
        {
            if (IsRain(forecast))
            {
                return false;
            }
            //TODO implement alert
            return true;
        }
    }
}
