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
    public class Led : MqttDevice, ILed
    {
        #region members
        private bool state;
        public event EventHandler<LedStateChangedEventArgs> LedStateChanged;
        #endregion

        #region constructor & destructor
        public Led(MqttClient client, string topic, LedColor color) : base(client, topic + "/" + color.ToString())
        {
            Color = color;
        }
        #endregion

        #region properties
        public LedColor Color { get; }

        public bool Enabled
        {
            get { return state; }
            set { SendCommand(value.ToString()); }
        }
        #endregion

        #region methods
        protected override void StatusReceived(string message, MqttMsgPublishEventArgs e)
        {
            if (bool.TryParse(message, out bool result))
            {
                if (result != state)
                {
                    state = result;
                    LedStateChanged?.Invoke(this, new LedStateChangedEventArgs(state));
                }
            }
        }
        #endregion
    }
}
