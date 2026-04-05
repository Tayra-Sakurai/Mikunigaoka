using CommunityToolkit.Mvvm.Messaging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Otori.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Sakaishi.Messages;
using Otori.Messages;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Mikunigaoka;

/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class MainPage : Page, IRecipient<IsInitializedChangedMessage>
{
    private SettingsViewModel? settingsViewModel;

    public MainPage()
    {
        InitializeComponent();
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        base.OnNavigatedTo(e);

        settingsViewModel = App.Current.Service.GetRequiredService<SettingsViewModel>();

        if (settingsViewModel.IsInitialized is not true)
        {
            // Disable the navigation.

            foreach (NavigationViewItem navigationViewItem in MainNavigation.MenuItems.OfType<NavigationViewItem>())
                navigationViewItem.IsEnabled = false;

            ContentFrame.Navigate(typeof(LargeCategoryAdditionPage));

            // Register to reset

            WeakReferenceMessenger.Default.Register(this);
        }
    }

    public void Receive(IsInitializedChangedMessage message)
    {
        if (message.Value is true)
            foreach (NavigationViewItem navigationViewItem in MainNavigation.MenuItems.OfType<NavigationViewItem>())
                navigationViewItem.IsEnabled = true;
    }
}
