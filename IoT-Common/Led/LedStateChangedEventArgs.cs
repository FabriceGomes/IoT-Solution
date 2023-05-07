using System;
using System.Collections.Generic;
using System.Text;

namespace IoT_Common
{
    public class LedStateChangedEventArgs : EventArgs
    {
        #region constructor & destructor
        public LedStateChangedEventArgs(bool enabled)
        {
            Enabled = enabled;
        }
        #endregion

        #region properties
        public bool Enabled { get; }
        #endregion
    }
}
