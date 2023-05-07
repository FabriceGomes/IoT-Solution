using IoT_Common;
using System;
using System.Collections.Generic;
using System.Text;
using uPLibrary.Networking.M2Mqtt;

namespace IoT_Service
{
    public class LedSerial : Led
    {

        #region constructor & destructor
        public LedSerial(MqttClient client, string topic, Com com, LedColor color) : base(client, topic, color)
        {
            Com = com;
        }
        #endregion

        #region properties
        public Com Com { get; }

        protected override bool EnableInternal
        {
            get { return false; }

            set
            {
                string col;
                switch (Color)
                {
                    case LedColor.Red: col = "red"; break;
                    case LedColor.Yellow: col = "yellow"; break;
                    case LedColor.Green: col = "green"; break;
                    case LedColor.Blue: col = "blue"; break;
                    default: throw new InvalidOperationException("Invalid color: " + Color);
                }
                Com.WriteMessage($"led {col} {(value ? "on" : "off")}"); // led red on
            }
        }
        #endregion
    }
}
