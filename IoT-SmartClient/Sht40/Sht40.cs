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
    public class Sht40 : MqttDevice, ISht40
    {

        #region constructor & destructor
        public Sht40(MqttClient client, string topic) : base(client, topic)
        {
        }
        #endregion

        #region properties
        public float Temperature { get; protected set; }
        public float Humidity { get; protected set; }

        public event EventHandler<Sht40ChangedEventArgs> TemperatureChanged;
        public event EventHandler<Sht40ChangedEventArgs> HumidityChanged;
        #endregion

        #region methods
        protected override void StatusReceived(string message, MqttMsgPublishEventArgs e)
        {
            // T=38.1;H=13.7
            Console.WriteLine("Message: " + message);
            string[] values = message.Split(';');
            if (values.Length != 2) return;

            string[] keyValuePair = values[0].Split('=');
            if (keyValuePair.Length != 2 && keyValuePair[0] != "T") return;
            float temperature = float.Parse(keyValuePair[1]);

            keyValuePair = values[1].Split('=');
            if (keyValuePair.Length != 2 && keyValuePair[0] != "H") return;
            float humidity = float.Parse(keyValuePair[1]);

            if (temperature != Temperature)
            {
                Temperature = temperature;
                OnTemperatureChanged(Temperature);
            }
            if (humidity != Humidity)
            {
                Humidity = humidity;
                OnHumidityChanged(Humidity);
            }
        }

        protected void OnTemperatureChanged(float temperature)
        {
            TemperatureChanged?.Invoke(this, new Sht40ChangedEventArgs(Temperature, Humidity));
        }

        protected void OnHumidityChanged(float humidity)
        {
            HumidityChanged?.Invoke(this, new Sht40ChangedEventArgs(Temperature, Humidity));
        }
        #endregion

    }


}
