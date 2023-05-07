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

    public class RaspberryClient
    {
        #region members
        private MqttClient client;
        private Dictionary<LedColor, Led> leds;
        #endregion

        #region properties
        public Sht40 Sht40 { get; private set; }

        public Joystick Joystick { get; private set; }

        public Led this[LedColor led]
        {
            get { return leds[led]; }
        }
        #endregion

        #region methods
        public void Connect(string host)
        {
            if (client == null)
            {
                client = new MqttClient(host);
                string id = Guid.NewGuid().ToString();

                client.MqttMsgPublishReceived += MqttMsgPublishReceived;
                client.Subscribe(new string[] { Constants.BASE_TOPIC + "/#" }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
                client.Connect(id);

                leds = new Dictionary<LedColor, Led>(4);
                leds.Add(LedColor.Green, new Led(client, Constants.TOPIC_LEDS, LedColor.Green));
                leds.Add(LedColor.Red, new Led(client, Constants.TOPIC_LEDS, LedColor.Red));
                leds.Add(LedColor.Yellow, new Led(client, Constants.TOPIC_LEDS, LedColor.Yellow));
                leds.Add(LedColor.Blue, new Led(client, Constants.TOPIC_LEDS, LedColor.Blue));

                Joystick = new Joystick(client, Constants.TOPIC_JOYSTICK);
                Sht40 = new Sht40(client, Constants.TOPIC_SHT40);
            }
        }

        public void Disconnect()
        {
            if (client != null)
            {
                try
                {
                    client.MqttMsgPublishReceived -= MqttMsgPublishReceived;
                    if (client.IsConnected)
                    {
                        client.Disconnect();
                    }
                }
                finally
                {
                    client = null;
                }
            }
        }
        private void MqttMsgPublishReceived(object sender, uPLibrary.Networking.M2Mqtt.Messages.MqttMsgPublishEventArgs e)
        {
            Console.WriteLine("Message received: " + e.Topic + " > " + Encoding.UTF8.GetString(e.Message));
        }
        #endregion

    }
}
