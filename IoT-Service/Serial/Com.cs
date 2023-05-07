//using RJCP.IO.Ports; // Alternative
using System.IO.Ports;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Device.Gpio;

namespace IoT_Service
{
    public class Com
    {

        #region members
        private static readonly NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();
        public event EventHandler<ComEventArgs> MessageReceived;
        //private SerialPortStream com;
        private SerialPort com;
        #endregion

        #region constructor & destructor
        public Com()
        {
            //com = new SerialPortStream("/dev/ttyS0", 115200);
            com = new SerialPort("/dev/ttyAMA2", 115200);
            com.Open();
            if (!com.IsOpen)
            {
                log.Error("Error opening serial port" + com.PortName);
                return;
            }
            log.Info($"Serial Port {com.PortName} open");

            //com.Handshake = Handshake.None;
            //com.ReadTimeout = 10000;
            com.NewLine = "\n";

            Thread t = new Thread(Run);
            t.IsBackground = true;
            t.Start();
        }
        #endregion

        #region methods
        private void OnMessageReceived(string message)
        {
            ComEventArgs e = new ComEventArgs(message);
            MessageReceived?.Invoke(this, e);
            if (!e.Handled)
            {
                log.Warn("Message not handled: " + message);
            }
        }

        public void WriteMessage(string s)
        {
            com.Write(s + "\n");
        }

        private void Run()
        {
            log.Debug("starting run");
            while (true)
            {
                string s = com.ReadLine();
                log.Debug("Message received: " + s);
                OnMessageReceived(s);
            }
        }
        #endregion

    }
}
