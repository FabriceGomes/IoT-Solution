using IoT_Common;
using System;

namespace IoT_Service
{
    class Program
    {
        #region members
        private static readonly NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();
        #endregion

        static void Main(string[] args)
        {
            bool direct = (args.Length == 1 && args[0].ToLower() == "serial");
            Raspberry pi = new Raspberry(!direct);
            pi.Connect();
            log.Info("Service started.");
            Console.ReadKey();
            pi.Disconnect();
        }
    }
}

