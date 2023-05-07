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
    /// Interaction logic for LedView.xaml
    /// </summary>
    public partial class LedView : UserControl
    {
        #region members
        private bool enabled;
        private Color color;
        #endregion

        #region constructor & destructor
        public LedView()
        {
            InitializeComponent();
        }
        #endregion

        #region properties
        public Color Color
        {
            get { return color; }
            set
            {
                color = value;
                UpdateUi();
            }
        }
        public bool Enabled
        {
            get { return enabled; }
            set
            {
                enabled = value;
                UpdateUi();
            }
        }
        #endregion

        #region methods
        private void UpdateUi()
        {
            if (Enabled)
            {
                colorLedCenter.Color = Color.FromArgb(128, (byte)(Color.R * 1), (byte)(Color.G * 1), (byte)(Color.B * 1));
                colorLedFace.Color = Color.FromRgb((byte)(Color.R * 0.75), (byte)(Color.G * 0.75), (byte)(Color.B * 0.75));
            }
            else
            {
                colorLedFace.Color = Color.FromRgb((byte)(Color.R * 0.3), (byte)(Color.G * 0.3), (byte)(Color.B * 0.3));
                colorLedCenter.Color = Color.FromRgb((byte)(Color.R * 0.6), (byte)(Color.G * 0.6), (byte)(Color.B * 0.6));
            }
        }
        #endregion
    }
}
