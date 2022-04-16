using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherAlertBackend.Alerts
{
    [Flags]
    public enum AlertList
    {
        None                            = 0b0,
        Fog                             = 0b1,
        DewPoint                        = 0b10,
        CloudInversion                  = 0b100,
        MorningBlueHourPartlyCloudy     = 0b1000,
        MorningGoldenHourPartlyCloudy   = 0b10000,
        EveningBlueHourPartlyCloudy     = 0b100000,
        EveningGoldenHourPartlyCloudy   = 0b1000000,
        CloudMix                        = 0b10000000,
        FullmoonRise                    = 0b100000000,
        FullmoonSet                     = 0b1000000000,
        CresentRise                     = 0b10000000000,
        CresentSet                      = 0b100000000000,
        Stars                           = 0b1000000000000,
        Morning                         = Fog | DewPoint | CloudInversion | MorningBlueHourPartlyCloudy | MorningGoldenHourPartlyCloudy,
        Evening                         = EveningBlueHourPartlyCloudy | EveningGoldenHourPartlyCloudy,
        Day                             = Morning | Evening | CloudMix,
        Moon                            = FullmoonRise | FullmoonSet | CresentRise | CresentSet,
        Night                           = Moon | Stars,
        All                             = Day | Night
    }
}
