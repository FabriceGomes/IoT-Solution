using System;
using System.Collections.Generic;
using System.Text;

namespace IoT_Common
{
    public interface ISht40
    {
        #region events
        event EventHandler<Sht40ChangedEventArgs> TemperatureChanged;
        event EventHandler<Sht40ChangedEventArgs> HumidityChanged;
        #endregion

        #region properties
        float Temperature { get; }
        float Humidity { get; }
        #endregion
    }
}
