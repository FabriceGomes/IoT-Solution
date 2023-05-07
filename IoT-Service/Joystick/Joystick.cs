using IoT_Common;
using System;
using System.Collections.Generic;
using System.Text;
using uPLibrary.Networking.M2Mqtt;

namespace IoT_Service
{
    public abstract class Joystick : MqttDevice, IJoystick
    {
        #region members
        public event EventHandler<JoystickEventArgs> JoystickChanged;
        #endregion

        #region constructor & destructor
        public Joystick(MqttClient client, string topic) : base(client, topic)
        {
        }
        #endregion

        #region properties
        public abstract JoystickButtons State { get; }
        #endregion

        #region methods
        protected void OnJoystickChanged(JoystickButtons state)
        {
            SendStatusUpdate(state.ToString());
            JoystickChanged?.Invoke(this, new JoystickEventArgs(state));
        }
        #endregion
    }
}
