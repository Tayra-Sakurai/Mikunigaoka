using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using Otori.Messages;
using Otori.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Otori.ViewModels
{
    public class SettingsViewModel : ObservableObject
    {
        private readonly ISettingsService settingsService;

        public SettingsViewModel(ISettingsService settingsService)
        {
            this.settingsService = settingsService;
        }

        public bool? IsInitialized
        {
            get => settingsService.GetLocalSetting("IsInitialized") as bool?;
            set
            {
                settingsService.SetLocalSetting("IsInitialized", value);
                OnPropertyChanged();

                WeakReferenceMessenger.Default.Send(new IsInitializedChangedMessage(value));
            }
        }
    }
}
