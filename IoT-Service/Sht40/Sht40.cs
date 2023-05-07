using IoT_Common;
using System;
using System.Collections.Generic;
using System.Text;
using uPLibrary.Networking.M2Mqtt;

namespace IoT_Service
{
    public abstract class Sht40 : MqttDevice, ISht40
    {

        #region members
        private static readonly NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();
        public event EventHandler<Sht40ChangedEventArgs> TemperatureChanged;
        public event EventHandler<Sht40ChangedEventArgs> HumidityChanged;
        #endregion

        #region constructor & destructor
        public Sht40(MqttClient client, string topic ) : base(client, topic)
        {
        }
        #endregion

        #region properties
        public float Temperature { get; protected set; }
        public float Humidity { get; protected set; }
        #endregion

        #region methods
        protected void OnTemperatureChanged(float temperature)
        {
            TemperatureChanged?.Invoke(this, new Sht40ChangedEventArgs(Temperature, Humidity));
        }

        protected void OnHumidityChanged(float humidity)
        {
            HumidityChanged?.Invoke(this, new Sht40ChangedEventArgs(Temperature, Humidity));
        }

        protected void UpdateData(float temperature, float humidity)
        {
            bool sendUpdate = false;
            if (temperature != Temperature)
            {
                Temperature = temperature;
                OnTemperatureChanged(Temperature);
                sendUpdate = true;
            }
            if (humidity != Humidity)
            {
                Humidity = humidity;
                OnHumidityChanged(Humidity);
                sendUpdate = true;
            }
            if (sendUpdate)
            {
                SendStatusUpdate($"T={Temperature};H={Humidity}");
                log.Trace($"Sending SHT40 T={Temperature};H={Humidity}");
            }
        }
        #endregion
    }
}
