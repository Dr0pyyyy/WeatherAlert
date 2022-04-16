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
                    for(int d = 0; d < forecast.Days.Count; d++)
                    {
                        for (int h = 0; h < forecast.Days[d].Hours.Count; h++)
                        {
                            Alert checkedAlert = CreateAlert((AlertList)alertType, forecast.Days[d].Hours[h].Date, forecast);
                            if (checkedAlert.IsActive())
                            {
                                res.Add(checkedAlert);
                            }
                        }
                    }
                }
            }

            return res;
        }

        private Alert CreateAlert(AlertList type, DateTime time, WeatherForecast forecast)
        {
            switch (type)
            {
                case AlertList.Fog:
                    return new FogAlert(forecast, time);
                case AlertList.DewPoint:
                    return new DewPointAlert(forecast, time);
                case AlertList.CloudInversion:
                    return new CloudInversionAlert(forecast, time);
                case AlertList.MorningBlueHourPartlyCloudy:
                    return new MorningBlueHourPartlyCloudyAlert(forecast, time);
                case AlertList.MorningGoldenHourPartlyCloudy:
                    return new MorningGoldenHourPartlyCloudyAlert(forecast, time);
                case AlertList.EveningBlueHourPartlyCloudy:
                    return new EveningBlueHourPartlyCloudyAlert(forecast, time);
                case AlertList.EveningGoldenHourPartlyCloudy:
                    return new EveningGoldenHourPartlyCloudyAlert(forecast, time);
                case AlertList.CloudMix:
                    return new CloudMixAlert(forecast, time);
                case AlertList.FullmoonRise:
                    return new FullmoonRiseAlert(forecast, time);
                case AlertList.FullmoonSet:
                    return new FullmoonSetAlert(forecast, time);
                case AlertList.CresentRise:
                    return new CresentRiseAlert(forecast, time);
                case AlertList.CresentSet:
                    return new CresentSetAlert(forecast, time);
                case AlertList.Stars:
                    return new StarsAlert(forecast, time);
                default:
                    throw new Exception("Unknown alert type: " + type);
            }
        }
        #endregion
    }
}
