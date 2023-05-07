using IoT_Common;
using System.Collections.Generic;
using System.Device.Gpio;
using uPLibrary.Networking.M2Mqtt;

namespace IoT_Service
{
    public class Raspberry
    {
        #region members
        private Dictionary<LedColor, Led> leds;
        #endregion

        #region constructor & destructor
        public Raspberry(bool direct)
        {
            //Client = new MqttClient("10.180.21.144");
            Client = new MqttClient("127.0.0.1");
            leds = new Dictionary<LedColor, Led>(4);

            // Raspberry Pi doesn't support I2C-MultiMaster
            // => Enabled/Disable I2C-Access from TinyK22
            GpioController = new GpioController();
            GpioController.OpenPin(17, PinMode.Output);

            if (direct)
            {
                GpioController.Write(17, false); // Disable I2C-Access from TinyK22
                leds.Add(LedColor.Green, new LedDirect(Client, Constants.TOPIC_LEDS, LedColor.Green, GpioController));
                leds.Add(LedColor.Yellow, new LedDirect(Client, Constants.TOPIC_LEDS, LedColor.Yellow, GpioController));
                leds.Add(LedColor.Red, new LedDirect(Client, Constants.TOPIC_LEDS, LedColor.Red, GpioController));
                Joystick = new JoystickDirect(Client, Constants.TOPIC_JOYSTICK, GpioController);
                Sht40 = new Sht40Direct(Client, Constants.TOPIC_SHT40);
            }
            else
            {
                GpioController.Write(17, true); // Enable I2C-Access from TinyK22
                Com com = new Com();
                leds.Add(LedColor.Green, new LedSerial(Client, Constants.TOPIC_LEDS, com, LedColor.Green));
                leds.Add(LedColor.Yellow, new LedSerial(Client, Constants.TOPIC_LEDS, com, LedColor.Yellow));
                leds.Add(LedColor.Red, new LedSerial(Client, Constants.TOPIC_LEDS, com, LedColor.Red));
                Joystick = new JoystickSerial(Client, Constants.TOPIC_JOYSTICK, com);
                Sht40 = new Sht40Serial(Client, Constants.TOPIC_SHT40, com);
            }
        }
        #endregion


        #region properties
        internal GpioController GpioController { get; }

        public ILed this[LedColor led]
        {
            get { return leds[led]; }
        }

        public IJoystick Joystick { get; }

        public MqttClient Client { get; }

        public ISht40 Sht40 { get; }
        #endregion


        #region methods
        public void Connect()
        {
            Client.Connect("");
        }

        public void Disconnect()
        {
            Client.Disconnect();
        }
        #endregion
    }
}