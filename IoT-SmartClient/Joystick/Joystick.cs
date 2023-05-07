using IoT_Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace IoT_SmartClient
{
    public class Joystick : MqttDevice, IJoystick
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
        public JoystickButtons State { get; private set; }
        #endregion

        #region methods
        protected override void StatusReceived(string message, MqttMsgPublishEventArgs e)
        {
            State = (JoystickButtons)Enum.Parse(typeof(JoystickButtons), message);
            JoystickChanged?.Invoke(this, new JoystickEventArgs(State));
        }
        #endregion
    }
}
