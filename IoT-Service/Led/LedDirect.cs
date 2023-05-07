using IoT_Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Device.Gpio;
using uPLibrary.Networking.M2Mqtt;

namespace IoT_Service
{
    public class LedDirect : Led
    {

        #region members
        private readonly int ledPin;
        #endregion

        #region constructor & destructor
        public LedDirect(MqttClient client, string topic, LedColor color, GpioController gpioController) : base (client, topic, color)
        {
            switch (color)
            {
                case LedColor.Green: ledPin = 21; break;
                case LedColor.Yellow: ledPin = 20; break;
                case LedColor.Red: ledPin = 16; break;
                default:
                    throw new NotImplementedException("Invalid Led Color " + color);
            }
            //ledPin = (int)color;
            GpioController = gpioController;
            GpioController.OpenPin(ledPin, PinMode.Output);

            Enabled = false;
        }
        #endregion

        #region properties
        private GpioController GpioController { get; }

        protected override bool EnableInternal
        {
            get { return (bool)GpioController.Read(ledPin); }
            set { GpioController.Write(ledPin, value); }
        }
        #endregion
    }
}
