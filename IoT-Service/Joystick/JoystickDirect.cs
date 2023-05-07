using IoT_Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Device.Gpio;
using uPLibrary.Networking.M2Mqtt;

namespace IoT_Service
{
    public class JoystickDirect : Joystick
    {

        #region members
        private const int buttonLeft = 6;
        private const int buttonRight = 5;
        private const int buttonUp = 19;
        private const int buttonDown = 13;
        private const int buttonCenter = 26;
        #endregion

        #region constructor & destructor
        public JoystickDirect(MqttClient client, string topic, GpioController gpioController) : base(client, topic)
        {
            GpioController = gpioController;
            // 5 GPIO Pins vom Joystick konfigurieren (Input, Pull-Up)
            GpioController.OpenPin(buttonLeft, PinMode.InputPullUp);
            GpioController.OpenPin(buttonRight, PinMode.InputPullUp);
            GpioController.OpenPin(buttonUp, PinMode.InputPullUp);
            GpioController.OpenPin(buttonDown, PinMode.InputPullUp);
            GpioController.OpenPin(buttonCenter, PinMode.InputPullUp);

            Thread t = new Thread(Run);
            t.IsBackground = true;
            t.Start();
        }
        #endregion

        #region properties
        internal GpioController GpioController { get; set; }

        /// <summary>
        /// Liefert den Joystick-Zustand (welche Taste(n) gedrückt ist/sind)
        /// </summary>
        public override JoystickButtons State
        {
            get
            {
                JoystickButtons state = JoystickButtons.None;
                if (!(bool)GpioController.Read(buttonCenter)) state |= JoystickButtons.Center;
                if (!(bool)GpioController.Read(buttonLeft)) state |= JoystickButtons.Left;
                if (!(bool)GpioController.Read(buttonRight)) state |= JoystickButtons.Right;
                if (!(bool)GpioController.Read(buttonUp)) state |= JoystickButtons.Up;
                if (!(bool)GpioController.Read(buttonDown)) state |= JoystickButtons.Down;
                return state;
            }
        }
        #endregion

        #region methods
        public void Run()
        {
            JoystickButtons oldState = State;
            while(true)
            {
                JoystickButtons newState = State;
                if (oldState != newState)
                {
                    oldState = newState;
                    OnJoystickChanged(newState);
                }
                Thread.Sleep(50);
            }
        }
        #endregion
    }
}
