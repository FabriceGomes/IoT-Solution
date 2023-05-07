using System;
using System.Collections.Generic;
using System.Text;

namespace IoT_Service
{
    public class ComEventArgs : EventArgs
    {
        #region constructor & destructor
        public ComEventArgs(string message)
        {
            Message = message;
            Handled = false;
        }
        #endregion

        #region properties
        public string Message { get; }

        public bool Handled { get; set; }
        #endregion
    }
}
