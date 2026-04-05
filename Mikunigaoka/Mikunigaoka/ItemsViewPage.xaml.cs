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

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Mikunigaoka;

/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class ItemsViewPage : Page
{
    private ItemsViewModel viewModel;

    public ItemsViewPage()
    {
        InitializeComponent();

        viewModel = App.Current.Service.GetRequiredService<ItemsViewModel>();

        SuperAdditionCommand.ExecuteRequested += SuperAdditionCommand_ExecuteRequested;
        SuperEditCommand.CanExecuteRequested += SuperEditCommand_CanExecuteRequested;
        SuperEditCommand.ExecuteRequested += SuperEditCommand_ExecuteRequested;
    }

    private void SuperEditCommand_ExecuteRequested(XamlUICommand sender, ExecuteRequestedEventArgs args)
    {
        if (args.Parameter is Item item)
            Frame.Navigate(typeof(ItemEditAddPage), item);
    }

    private void SuperEditCommand_CanExecuteRequested(XamlUICommand sender, CanExecuteRequestedEventArgs args)
    {
        if (args.Parameter is Item item)
        {
            args.CanExecute = true;
            return;
        }

        args.CanExecute = false;
    }

    private void SuperAdditionCommand_ExecuteRequested(XamlUICommand sender, ExecuteRequestedEventArgs args)
    {
        Frame.Navigate(typeof(ItemEditAddPage));
    }
}
