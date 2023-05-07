using System;
using System.Collections.Generic;
using System.Text;

namespace IoT_Common
{
    public interface IJoystick
    {
        #region events
        event EventHandler<JoystickEventArgs> JoystickChanged;
        #endregion

        #region properties
        JoystickButtons State { get; }
        #endregion
    }
}
