using IoT_Common;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace IoT_SmartClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        #region members
        private RaspberryClient client;
        #endregion

        #region constructor & destructor
        public MainWindow()
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            InitializeComponent();

            client = new RaspberryClient();
            textBoxHost.Text = RegistryHelper.RegistryGetString("host", "");
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Console.WriteLine("Crash:\r\n" + e.ToString());
        }
        #endregion

        #region properties
        public bool Connected { get; private set; }
        #endregion

        #region temperature & humidity events
        private void TemperatureChanged(object sender, IoT_Common.Sht40ChangedEventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                labelTemperature.Content = e.Temperature.ToString() + "°C";
            });
        }

        private void HumidityChanged(object sender, IoT_Common.Sht40ChangedEventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
               labelHumidity.Content = e.Humidity.ToString() + "%";
            });
        }
        #endregion

        #region joystick events
        private void JoystickChanged(object sender, IoT_Common.JoystickEventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                imageUp.Opacity = imageDown.Opacity = imageLeft.Opacity = imageRight.Opacity = imageCenter.Opacity = 0.3;
                if ((e.Button & JoystickButtons.Up) != 0) imageUp.Opacity = 1;
                if ((e.Button & JoystickButtons.Down) != 0) imageDown.Opacity = 1;
                if ((e.Button & JoystickButtons.Left) != 0) imageLeft.Opacity = 1;
                if ((e.Button & JoystickButtons.Right) != 0) imageRight.Opacity = 1;
                if ((e.Button & JoystickButtons.Center) != 0) imageCenter.Opacity = 1;
            });
        }
        #endregion

        #region connect/disconnect event
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!Connected)
            {
                RegistryHelper.RegistrySetString("host", textBoxHost.Text);
                client.Connect(textBoxHost.Text);

                client.Sht40.HumidityChanged += HumidityChanged;
                client.Sht40.TemperatureChanged += TemperatureChanged;
                client.Joystick.JoystickChanged += JoystickChanged;

                ledControlGreen.Led = client[LedColor.Green];
                ledControlRed.Led = client[LedColor.Red];
                ledControlYellow.Led = client[LedColor.Yellow];

                labelStatus.Content = "Connected to " + textBoxHost.Text;
                buttonConnect.Content = "Disconnect";
                Connected = true;
            }
            else
            {
                labelStatus.Content = "Disconnected";
                buttonConnect.Content = "Connect";
                client.Disconnect();
                Connected = false;
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            client.Disconnect();
        }

        private void MenuItemExitClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
        #endregion

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (menuItemShowConsole.IsChecked)
            {
                ConsoleManager.Show();
            }
            else
            {
                ConsoleManager.Hide();
            }
        }
    }
}
