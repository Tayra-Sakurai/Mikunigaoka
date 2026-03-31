using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Otori.Services
{
    public interface ISettingsService
    {
        /// <summary>
        /// Sets the value for a local application setting identified by the specified key.
        /// </summary>
        /// <param name="key">The name of the setting to update. Cannot be null or empty.</param>
        /// <param name="value">The value to assign to the setting. If null, the setting may be removed or cleared depending on
        /// implementation.</param>
        /// <returns>true if the setting was successfully updated; otherwise, false.</returns>
        /// <exception cref="ArgumentException">One or more arguments are empty or only contain white spaces.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="key"/> is null.</exception>
        bool SetLocalSetting(string key, string value);

        /// <exception cref="ArgumentException"><paramref name="key"/> contains only white spaces.</exception>
        /// <inheritdoc cref="SetLocalSetting(string, string)"/>
        bool SetLocalSetting(string key, bool value);

        /// <inheritdoc cref="SetLocalSetting(string, bool)"/>
        bool SetLocalSetting(string key, int value);
    }
}
