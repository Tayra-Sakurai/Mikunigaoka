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
using Microsoft.Extensions.DependencyInjection;
using Sakaishi.Models;
using CommunityToolkit.Mvvm.Messaging;
using Sakaishi.Messages;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Mikunigaoka;

/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class PaymentMethodEditionPage : Page, IRecipient<PaymentMethodDeletedMessage>
{
    private PaymentMethodViewModel? viewModel;

    public PaymentMethodEditionPage()
    {
        InitializeComponent();
    }

    protected async override void OnNavigatedTo(NavigationEventArgs e)
    {
        base.OnNavigatedTo(e);

        viewModel = App.Current.Service.GetRequiredService<PaymentMethodViewModel>();

        WeakReferenceMessenger.Default.Register(this);

        if (e.Parameter is PaymentMethod method)
            await viewModel.InitializeForExistingValue(method);
    }

    protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
    {
        WeakReferenceMessenger.Default.UnregisterAll(this);

        base.OnNavigatingFrom(e);
    }

    public void Receive(PaymentMethodDeletedMessage message)
    {
        if (Frame is not null)
        {
            if (Frame.CanGoBack)
            {
                Frame.GoBack();
                return;
            }

            Frame.Navigate(typeof(ItemsViewPage));
        }
    }
}
