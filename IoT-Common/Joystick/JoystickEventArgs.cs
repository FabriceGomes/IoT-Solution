using System;
using System.Collections.Generic;
using System.Text;

namespace IoT_Common
{
    public class JoystickEventArgs : EventArgs
    {

        #region constructor & destructor
        public JoystickEventArgs(JoystickButtons button)
        {
            Button = button;
        }
        #endregion


        #region properties
        public JoystickButtons Button { get; }
        #endregion

    }
}
