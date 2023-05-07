using IoT_Common;
using System;
using System.Collections.Generic;
using System.Text;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace IoT_Service
{

    public abstract class Led : MqttDevice, ILed
    {
        #region members
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
            get { return EnableInternal; }
            set
            {
                EnableInternal = value;
                OnLedStateChanged(value);
            }
        }

        protected abstract bool EnableInternal { get; set; }
        #endregion

        #region methods
        protected void OnLedStateChanged(bool enabled)
        {
            SendStatusUpdate(enabled.ToString()); // true|false
            LedStateChanged?.Invoke(this, new LedStateChangedEventArgs(enabled));
        }

        protected override void CommandReceived(string message, MqttMsgPublishEventArgs e)
        {
            if (bool.TryParse(message, out bool result))
            {
                Enabled = result;
            }
        }
        #endregion
    }
}
