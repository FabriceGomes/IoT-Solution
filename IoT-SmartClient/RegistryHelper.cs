using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IoT_SmartClient
{
    public class RegistryHelper
    {
        #region members
        private const string RegKey = "Software\\hslu\\IoT-SmartClient";
        #endregion


        #region registry helper
        public static string RegistryGetString(string name, string defaultValue)
        {
            RegistryKey key = Registry.CurrentUser.OpenSubKey(RegKey, false);
            if (key == null) key = Registry.CurrentUser.CreateSubKey(RegKey);
            string value = (string)key.GetValue(name, defaultValue);
            key.Close();
            return value;
        }

        public static void RegistrySetString(string name, string value)
        {
            RegistryKey key = Registry.CurrentUser.OpenSubKey(RegKey, true);
            if (key == null) key = Registry.CurrentUser.CreateSubKey(RegKey);
            key.SetValue(name, value);
            key.Close();
        }
        #endregion
    }
}
