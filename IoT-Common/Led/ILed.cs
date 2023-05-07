using System;
using System.Collections.Generic;
using System.Text;

namespace IoT_Common
{
    public interface ILed
    {
        #region events
        event EventHandler<LedStateChangedEventArgs> LedStateChanged;
        #endregion

        #region properties
        bool Enabled { get; set; }

        LedColor Color { get; }
        #endregion
    }
}
