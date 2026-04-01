using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace Otori.Services
{
    public class WindowsSettingsService : ISettingsService
    {
        private readonly ApplicationDataContainer local;

        public WindowsSettingsService()
        {
            local = ApplicationData.Current.LocalSettings;
        }

        private bool SetLocalSetting<TValue>(string key, TValue value)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(key);

            try
            {
                local.Values[key] = value;
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool SetLocalSetting(string key, string value)
        {
            return SetLocalSetting<string>(key, value);
        }

        public bool SetLocalSetting(string key, int value)
        {
            return SetLocalSetting<int>(key, value);
        }

        public bool SetLocalSetting(string key, bool value)
        {
            return SetLocalSetting<bool>(key, value);
        }
    }
}
