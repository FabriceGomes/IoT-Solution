using System;
using System.Collections.Generic;
using System.Text;
using System.Device.Gpio;
using uPLibrary.Networking.M2Mqtt;

namespace IoT_Service
{
    public class Sht40Serial : Sht40
    {


        #region members
        private static readonly NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();
        private float temperature, humidity;
        #endregion

        #region constructor & destructor
        public Sht40Serial(MqttClient client, string topic, Com com) : base(client, topic)
        {
            //// Enable I2C-Access from TinyK22
            //var gpio = new GpioController();
            //gpio.OpenPin(17, PinMode.Output);
            //gpio.Write(17, true);

            //var sensorControl = Pi.Gpio[17];
            //sensorControl.PinMode = GpioPinDriveMode.Output;
            //sensorControl.Write(true);

            Com = com;
            com.MessageReceived += ComMessageReceived;
        }
        #endregion

        #region properties
        public Com Com { get; }
        #endregion

        #region methods
        private void ComMessageReceived(object sender, ComEventArgs e)
        {
            bool hasChanges = false;
            if (e.Message.StartsWith("Temp "))
            {
                string data = e.Message.Replace("Temp ", "");
                temperature = int.Parse(data) / 10f;
                hasChanges = true;
            }

            if (e.Message.StartsWith("Humidity "))
            {
                string data = e.Message.Replace("Humidity ", "");
                humidity = int.Parse(data) / 10f;
                hasChanges = true;
            }

            if (hasChanges)
            {
                UpdateData(temperature, humidity);
                e.Handled = true;
            }
        }
        #endregion


    }
}
