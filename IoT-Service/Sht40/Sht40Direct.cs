using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Device.Gpio;
using System.Device.I2c;
using uPLibrary.Networking.M2Mqtt;

namespace IoT_Service
{
    public class Sht40Direct : Sht40
    {

        #region members
        private static readonly NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();
        private const byte SHT40_CRC8_POLYNOMIAL = 0x31;    // Seed for CRC polynomial
        private const byte SHT40_CRC8_INIT = 0xFF;          // Init value for CRC
        private I2cDevice dev;
        #endregion

        #region constructor & destructor
        public Sht40Direct(MqttClient client, string topic) : base(client, topic)
        {
            //// Disable I2C-Access from TinyK22
            //var gpio = new GpioController();
            //gpio.OpenPin(17, PinMode.Output);
            //gpio.Write(17, false); // Pin 17 = LOW => Disable TinyK22 Access

            I2cBus bus = I2cBus.Create(1);
            dev = bus.CreateDevice(0x44);

            Thread t = new Thread(Run);
            t.IsBackground = true;
            t.Start();
        }
        #endregion

        #region methods
        public void Run()
        {
            float temperature = float.MinValue;
            float humidity = float.MinValue;
            while (true)
            {
                if (ReadTempHumidity(out temperature, out humidity))
                {
                    UpdateData(temperature, humidity);
                }
                Thread.Sleep(1000);
            }
        }

        private byte GenerateCRC(byte[] data, byte index, byte length)
        {
            // calculates 8-Bit checksum with given polynomial
            byte crc = SHT40_CRC8_INIT;
            byte b;
            byte i;

            for (i = index; i < (index + length); i++)
            {
                crc ^= data[i];
                for (b = 0; b < 8; b++)
                {
                    if ((crc & 0x80) != 0)
                    {
                        crc = (byte)((crc << 1) ^ SHT40_CRC8_POLYNOMIAL);
                    }
                    else
                    {
                        crc <<= 1;
                    }
                }
            }
            return crc;
        }

        public bool ReadTempHumidity(out float temperature, out float humidity)
        {
            temperature = 0.0f;
            humidity = 0.0f;
            ///return true;

            temperature = humidity = float.MinValue;


            dev.WriteByte(0xFD);
            Thread.Sleep(100);

            byte[] data = new byte[6];
            dev.Read(data);


            UInt16 t = (UInt16)((data[0] << 8) | data[1]);
            if (data[2] != GenerateCRC(data, 0, 2))
            {
                Console.WriteLine("CRC Error reading temperature from SHT40");
                return false;
            }

            UInt16 h = (UInt16)((data[3] << 8) | data[4]);
            if (data[5] != GenerateCRC(data, 3, 2))
            {
                Console.WriteLine("CRC Error reading humidity from SHT40");
                return false;
            }

            temperature = (175f * t) / 0xffff - 45;
            humidity = (125f * h) / 0xffff - 6;

            temperature = (float)Math.Round(temperature, 1);
            humidity = (float)Math.Round(humidity, 1);

            return true;
        }
        /*
        public bool ReadTempHumiditySHT30Old(out float temperature, out float humidity)
        {
            temperature = humidity = float.MinValue;

            byte[] command = new byte[2] { 0x2C, 0x06 };
            int result = tempSensor.Write(command, command.Length);
            if (result != 2)
            {
                log.Error("Error write command to SHT30, ErrorCode:" + result);
                return false;
            }
            Thread.Sleep(100);

            byte[] data = new byte[6];
            result = tempSensor.Read(data, 6);
            if (result != 6)
            {
                log.Error("Error reading data from SHT30, ErrorCode:" + result);
                return false;
            }

            UInt16 t = (UInt16)((data[0] << 8) | data[1]);
            if (data[2] != GenerateCRC(data, 0, 2))
            {
                log.Error("CRC Error reading temperature from SHT30");
                return false;
            }

            UInt16 h = (UInt16)((data[3] << 8) | data[4]);
            if (data[5] != GenerateCRC(data, 3, 2))
            {
                log.Error("CRC Error reading humidity from SHT30");
                return false;
            }

            temperature = (175f * t) / 0xffff - 45;
            humidity = (100f * h) / 0xffff;

            temperature = (float)Math.Round(temperature, 1);
            humidity = (float)Math.Round(humidity, 1);

            return true;
        }
        */
        #endregion

    }
}
