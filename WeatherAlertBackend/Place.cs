using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WeatherAlertBackend.Alerts;
using WeatherAlertBackend.Forecasts;

namespace WeatherAlertBackend
{
    public class Place
    {
        #region Properties
        public string Name { get; set; }

        public double Longitude { get; set; }

        public double Latitude { get; set; }

        private AlertManager alertMng;

        public AlertList WatchedAlerts
        {
            get
            {
                return alertMng != null ? alertMng.WatchedAlerts : AlertList.None;
            }
        }

        private List<Alert> _activeAlerts;

        public List<Alert> ActiveAlerts
        {
            get
            {
                if(_activeAlerts == null)
                {
                    DateTime start = Forecast.Days[0].Hours[0].Date;
                    _activeAlerts = alertMng.EvaluateAlerts(Forecast, start, start.AddDays(7));
                }
                return _activeAlerts;
            }
        }

        private WeatherForecast _forecast;

        public WeatherForecast Forecast 
        {
            get
            {
                if (_forecast == null)
                {
                    _forecast = new WeatherForecast(Longitude, Latitude);
                }
                return _forecast;
            }
        }
        #endregion

        #region Constructor
        public Place(string name, double longitude, double latitude, AlertList alerts)
        {
            Name = name;
            Longitude = longitude;
            Latitude = latitude;
            alertMng = new AlertManager(alerts);
        }
        #endregion

        #region Methods

        #endregion
    }
}
