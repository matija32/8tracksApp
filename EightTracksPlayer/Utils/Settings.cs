/************************************************************************/
/*                     Tech42 - Project (c) 2010                        */
/************************************************************************/

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Data;
using Microsoft.Win32;

namespace EightTracksPlayer.Utils
{
    /// <summary>
    /// This class saves user settings into the windows registry.
    /// 
    /// If accessible by a property, you can easily load/save settings from WPF as:
    ///		Top="{Binding Settings[Top], Mode=TwoWay, TargetNullValue=200}"
    ///   WindowState="{Binding Settings[WindowState], Mode=TwoWay, TargetNullValue=Normal}"
    ///   
    ///	Remember to define this window data context as:
    ///		DataContext="{Binding RelativeSource={RelativeSource Self}}"
    /// </summary>
    public class Settings : INotifyPropertyChanged
    {
        /// <summary>
        /// Entry path for keys
        /// </summary>
        private string mSubkeyPath;

        public Settings(string registryPath, string extension)
        {
            this.mSubkeyPath = registryPath + @"\" + extension;
        }

        /// <summary>
        /// Constructs the object and extend the registry path to save settings.
        /// </summary>
        /// <param name="extension">extension to the registry path</param>
        public Settings(string extension)
            : this(Application.ResourceAssembly.GetName().Name, extension)
        {
        }

        public Settings()
            : this(String.Empty)
        {
        }

        /// <summary>
        /// Accesses the key values.
        /// </summary>
        /// <param name="key">key to be accessed</param>
        /// <returns>the key object</returns>
        /// <remarks>the returned value can be null</remarks>
        public virtual object this[string key]
        {
            get
            {
                RegistryKey regkey = Registry.CurrentUser.OpenSubKey(mSubkeyPath);
                if (regkey == null)
                {
                    return null;
                }
                object value = regkey.GetValue(key);

                return value;
            }

            set
            {
                try
                {
                    // Get the registry key if any or create a new one.
                    RegistryKey regkey = Registry.CurrentUser.OpenSubKey(mSubkeyPath, true);
                    if (regkey == null)
                    {
                        regkey = Registry.CurrentUser.CreateSubKey(mSubkeyPath);
                    }

                    // If the value if false, delete the key, so the default value is used next time.
                    if (value == null)
                    {
                        regkey.DeleteValue(key);
                    }
                    else
                    {
                        regkey.SetValue(key, value);

                        if (PropertyChanged != null)
                        {
                            PropertyChanged(this, new PropertyChangedEventArgs("Settings[" + key + "]"));
                        }
                    }
                }
                catch (System.Exception ex)
                {
                    Debug.Print("Save setting [{0}] failed {1}", key, ex.Message);
                }
            }
        }

        #region INotifyPropertyChanged Members

        /// <summary>
        /// Used to trigger property changes when a setting changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }

    public class SettingConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            // Try some basic type casting first.
            if (value as string != null)
            {
                if (targetType == typeof (double))
                {
                    double d;
                    if (double.TryParse((string) value, out d))
                    {
                        return d;
                    }
                }
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter,
                                  System.Globalization.CultureInfo culture)
        {
            return value;
        }

        #endregion
    }
}