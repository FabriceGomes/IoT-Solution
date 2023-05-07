using IoT_Common;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace IoT_SmartClient
{
    /// <summary>
    /// Interaction logic for LedControl.xaml
    /// </summary>
    public partial class LedControl : UserControl
    {
        #region members
        private ILed led;
        #endregion

        #region constructor & destructor
        public LedControl()
        {
            InitializeComponent();
        }
        #endregion

        #region properties
        public Color Color
        {
            get { return ledView.Color; }
            set { ledView.Color = value; }
        }

        public ILed Led
        {
            set
            {
                led = value;
                if (led != null)
                {
                    led.LedStateChanged += LedStateChanged;
                }
            }
            get { return led; }
        }
        #endregion

        #region methods

        private void UpdateLed(bool enable)
        {
            ledView.Enabled = enable;
        }

        private void LedStateChanged(object sender, LedStateChangedEventArgs e)
        {
            //// Normale Methode
            //Action<bool> myAction1 = UpdateLed;
            //Dispatcher.Invoke(myAction1, e.Enabled);

            //// Anonyme Methode
            //Action myAction2 = delegate ()
            //{
            //    ledView.Enabled = e.Enabled;
            //};
            //Dispatcher.Invoke(myAction2);

            //// Kürzere Variante
            //Dispatcher.Invoke(delegate ()
            //{
            //    ledView.Enabled = e.Enabled;
            //});

            // Mit Lambda Ausdruck
            //Dispatcher.Invoke(() =>
            //{
            //   ledView.Enabled = e.Enabled;
            //});


            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(
                    new EventHandler<LedStateChangedEventArgs>(LedStateChanged), sender, e);
            }
            else
            {
                ledView.Enabled = e.Enabled;
            }
        }

        private void ButtonLedOnClick(object sender, RoutedEventArgs e)
        {
            led.Enabled = true;
        }

        private void ButtonLedOffClick(object sender, RoutedEventArgs e)
        {
            led.Enabled = false;
        }
        #endregion
    }
}
