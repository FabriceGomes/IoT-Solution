using System;
using System.Collections.Generic;
using System.Text;

namespace IoT_Common
{
    public static class Constants
    {
        public const string BASE_TOPIC = "86E57978-3401-4CA8-A41E-709A28DEEC56";

        public const string TOPIC_LEDS = BASE_TOPIC + "/Leds";
        public const string TOPIC_JOYSTICK = BASE_TOPIC + "/Joystick";
        public const string TOPIC_SHT40 = BASE_TOPIC + "/Sht40";

        public const string TOPIC_CMD = "/cmd";
        public const string TOPIC_STATUS = "/status";
    }
}
