using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherAlertBackend.Alerts.SingleAlerts;
using WeatherAlertBackend.Forecasts;

namespace WeatherAlertBackend.Alerts
{
    public class AlertManager
    {
        #region Properties
        public AlertList WatchedAlerts { get; set; }
        #endregion

        #region Constructor
        public AlertManager(AlertList watchedAlerts)
        {
            //Save user defined alerts
            WatchedAlerts = watchedAlerts;
        }
        #endregion

        #region Methods
        public List<Alert> EvaluateAlerts(WeatherForecast forecast, DateTime start, DateTime end)
        {
            List<Alert> res = new List<Alert>();

            for (int alertType = 1; alertType < (int)AlertList.All; alertType <<= 1)
            {
                if(WatchedAlerts.HasFlag((AlertList)alertType))
                {
                    DateTime time = new DateTime(start.Year, start.Month, start.Day, start.Hour, 0, 0);
                    while(time <= end)
                    {
                        Alert checkedAlert = CreateAlert((AlertList)alertType, time);
                        if(checkedAlert.IsActive(forecast))
                        {
                            res.Add(checkedAlert);
                        }
                        time = time.AddHours(1);
                    }
                }
            }
            return res;
        }

        private Alert CreateAlert(AlertList type, DateTime time)
        {
            switch (type)
            {
                case AlertList.Fog:
                    return new FogAlert(time);
                case AlertList.DewPoint:
                    return new DewPointAlert(time);
                case AlertList.CloudInversion:
                    return new CloudInversionAlert(time);
                case AlertList.MorningBlueHourPartlyCloudy:
                    return new MorningBlueHourPartlyCloudyAlert(time);
                case AlertList.MorningGoldenHourPartlyCloudy:
                    return new MorningGoldenHourPartlyCloudyAlert(time);
                case AlertList.EveningBlueHourPartlyCloudy:
                    return new EveningBlueHourPartlyCloudyAlert(time);
                case AlertList.EveningGoldenHourPartlyCloudy:
                    return new EveningGoldenHourPartlyCloudyAlert(time);
                case AlertList.CloudMix:
                    return new CloudMixAlert(time);
                case AlertList.FullmoonRise:
                    return new FullmoonRiseAlert(time);
                case AlertList.FullmoonSet:
                    return new FullmoonSetAlert(time);
                case AlertList.CresentRise:
                    return new CresentRiseAlert(time);
                case AlertList.CresentSet:
                    return new CresentSetAlert(time);
                case AlertList.Stars:
                    return new StarsAlert(time);
                default:
                    throw new Exception("Unknown alert type: " + type);
            }
        }
        #endregion
    }
}
