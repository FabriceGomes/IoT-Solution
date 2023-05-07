using IoT_Common;
using System;
using System.Collections.Generic;
using System.Text;
using uPLibrary.Networking.M2Mqtt;

namespace IoT_Service
{
    public class JoystickSerial : Joystick
    {
        #region members
        private JoystickButtons state;
        #endregion

        #region constructor & destructor
        public JoystickSerial(MqttClient client, string topic, Com com) : base(client, topic)
        {
            Com = com;
            com.MessageReceived += MessageReceived;
        }
        #endregion

        #region properties
        public Com Com { get; }

        public override JoystickButtons State { get { return state; } }
        #endregion

        #region methods
        private void MessageReceived(object sender, ComEventArgs e)
        {
            if (e.Message.StartsWith("joystick "))
            {
                e.Handled = true;
                string data = e.Message.Replace("joystick ", "");
                state = (JoystickButtons)int.Parse(data);
                OnJoystickChanged(state);
            }
        }
        #endregion
    }
}
