using System;
using System.Collections.Generic;
using System.Text;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace IoT_Common
{
    public abstract class MqttDevice
    {
        #region members
        private static readonly NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();
        protected readonly string TOPIC_CMD;
        protected readonly string TOPIC_STATUS;
        #endregion


        #region constructor & destructor
        public MqttDevice(MqttClient client, string topic)
        {
            MqttClient = client;
            TOPIC_CMD = topic + Constants.TOPIC_CMD;          // example: "8E680785-D767-4813-BCFE-12E1AA0B5065/Leds/Green/cmd"
            TOPIC_STATUS = topic + Constants.TOPIC_STATUS;    // example: "8E680785-D767-4813-BCFE-12E1AA0B5065/Leds/Green/status"

            log.Debug("Subscribe to " + TOPIC_CMD);
            log.Debug("Subscribe to " + TOPIC_STATUS);

            MqttClient.Subscribe(
                new string[] { TOPIC_CMD, TOPIC_STATUS },
                new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE }
                );

            MqttClient.MqttMsgPublishReceived += MqttMsgPublishReceived;
        }
        #endregion


        #region properties
        public MqttClient MqttClient { get; }
        #endregion


        #region methods
        private void MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            string message = Encoding.UTF8.GetString(e.Message);

            if (e.Topic == TOPIC_CMD)
            {
                log.Trace($"Command message received: {e.Topic}>{message}");
                CommandReceived(message, e);
            }
            else if (e.Topic == TOPIC_STATUS)
            {
                log.Trace($"Status message received: {e.Topic}>{message}");
                StatusReceived(message, e);
            }
        }

        protected virtual void CommandReceived(string message, MqttMsgPublishEventArgs e) { }

        protected virtual void StatusReceived(string message, MqttMsgPublishEventArgs e) { }

        protected void SendStatusUpdate(string message)
        {
            log.Debug($"Sending Status: {TOPIC_STATUS}>{message}");
            MqttClient.Publish(TOPIC_STATUS, Encoding.UTF8.GetBytes(message), MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE, true);
        }

        protected void SendCommand(string message)
        {
            log.Debug($"Sending Command: {TOPIC_CMD}>{message}");
            MqttClient.Publish(TOPIC_CMD, Encoding.UTF8.GetBytes(message));
        }
        #endregion
    }
}
