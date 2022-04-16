using System;
using System.Drawing;
using WeatherAlertBackend.Forecasts;

namespace WeatherAlertBackend.Alerts.SingleAlerts
{
    public class CloudInversionAlert : Alert
    {
        public CloudInversionAlert(WeatherForecast forecast, DateTime time) : base(forecast, time)
        {
            Name = "Cloud Inversion";
            Color = Color.White;
        }

        public override bool IsActive()
        {
            HourlyForecast hf = GetHourlyForecast();

            for (int i = 0; i < Forecast.Days.Count; i++)
            {
                //First if statement is here for first day, first day needs different evaluation
                if (i == 0)
                {
                    foreach (HourlyForecast hour in Forecast.Days[i].Hours)
                    {
                        if (hour.Date.Hour >= Forecast.Days[i].Date.Hour && hour.Date.Hour >= GetMorningBlueHour().Value.Hour && hour.TotalClouds > 20)
                        {
                            return false;
                        }
                    }
                }
                //Evaluation for every other day
                else
                {
                    //Checking hours (civDarknessStart - lasthour) from day before the day, that is currently being checked
                    foreach (HourlyForecast hour in Forecast.Days[i - 1].Hours)
                    {
                        if (hour.Date >= Forecast.Days[i - 1].CivDarknessStart && hour.TotalClouds > 20)
                        {
                            return false;
                        }
                    }
                    //Checking every hour (first hour - morning blue hour) from day that is currently checked
                    foreach (HourlyForecast hour in Forecast.Days[i].Hours)
                    {
                        if (hour.Date >= Forecast.Days[i].Date && hour.Date.Hour <= GetMorningBlueHour().Value.Hour && hour.TotalClouds > 20)
                        {
                            return false;
                        }
                    }
                }
            }

            return !IsRain() && hf.LowClouds >= 70 && hf.WindSpeed <= 10 && hf.Temperature <= hf.DewPoint && (IsMorningBlueHour() || IsMorningGoldenHour());
        }
        private DateTime? GetMorningBlueHour()
        {
            DateTime dt = new DateTime();
            foreach (DailyForecast day in Forecast.Days)
            {
                if (day.Sunrise != null)
                {
                    //Sunset time check
                    DateTime sunrise = day.Sunrise.Value;
                    if (Time.Date == sunrise.Date && Time.Hour == sunrise.Hour)
                    {
                        return Time.Date;
                    }

                    //Check 30 minutes before sunrise
                    sunrise = sunrise.AddMinutes(-30);
                    if (Time.Date == sunrise.Date && Time.Hour == sunrise.Hour)
                    {
                        return Time.Date;
                    }
                }
            }
            //Value cant be nullable so it returs datetime, that is not possible to ever return true
            return dt;
        }
    }
}