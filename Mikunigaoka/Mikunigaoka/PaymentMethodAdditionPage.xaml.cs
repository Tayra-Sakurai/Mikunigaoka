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
using Sakaishi.ViewModels;
using Otori.ViewModels;
using CommunityToolkit.Mvvm.Messaging;
using Sakaishi.Messages;
using Microsoft.Extensions.DependencyInjection;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Mikunigaoka;

/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class PaymentMethodAdditionPage : Page, IRecipient<PaymentMethodAddedMessage>
{
    private PaymentMethodViewModel? viewModel;
    private SettingsViewModel? settingsViewModel;

    public PaymentMethodAdditionPage()
    {
        InitializeComponent();
    }

    protected async override void OnNavigatedTo(NavigationEventArgs e)
    {
        base.OnNavigatedTo(e);

        viewModel = App.Current.Service.GetRequiredService<PaymentMethodViewModel>();
        settingsViewModel = App.Current.Service.GetRequiredService<SettingsViewModel>();

        DataContext = viewModel;

        WeakReferenceMessenger.Default.Register(this);
    }

    public void Receive(PaymentMethodAddedMessage message)
    {
        if (settingsViewModel is not null &&
            settingsViewModel.IsInitialized is not true)
        {
            settingsViewModel.IsInitialized = true;
            Frame.Navigate(typeof(ItemsViewPage));
        }
    }
}
