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
public sealed partial class CategoriesViewPage : Page, IRecipient<LargeCategoryInvokedMessage>, IRecipient<SmallCategoryInvokedMessage>
{
    private CategoriesViewModel? viewModel;

    public CategoriesViewPage()
    {
        InitializeComponent();

        AddLargeCategoryCommand.ExecuteRequested += AddLargeCategoryCommand_ExecuteRequested;
        AddSmallCategoryCommand.ExecuteRequested += AddSmallCategoryCommand_ExecuteRequested;
    }

    private void AddSmallCategoryCommand_ExecuteRequested(XamlUICommand sender, ExecuteRequestedEventArgs args)
    {
        Frame.Navigate(typeof(SmallCategoryInvokedMessage));
    }

    private void AddLargeCategoryCommand_ExecuteRequested(XamlUICommand sender, ExecuteRequestedEventArgs args)
    {
        Frame.Navigate(typeof(LargeCategoryAdditionPage));
    }

    protected async override void OnNavigatedTo(NavigationEventArgs e)
    {
        base.OnNavigatedTo(e);

        viewModel = App.Current.Service.GetRequiredService<CategoriesViewModel>();

        WeakReferenceMessenger.Default.RegisterAll(this);

        await viewModel.LoadAsync();
    }

    protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
    {
        base.OnNavigatingFrom(e);

        WeakReferenceMessenger.Default.UnregisterAll(this);
    }

    public void Receive(LargeCategoryInvokedMessage message)
    {
        Frame.Navigate(typeof(LargeCategoryEditionPage), message.Value);
    }

    public void Receive(SmallCategoryInvokedMessage message)
    {
        throw new NotImplementedException();
    }
}

class TreeViewItemTemplateSelector : DataTemplateSelector
{
    public DataTemplate? LargeCategoryTemplate { get; set; }
    public DataTemplate? SmallCategoryTemplate { get; set; }

    protected override DataTemplate? SelectTemplateCore(object item)
    {
        return (item is LargeCategory) ? LargeCategoryTemplate : SmallCategoryTemplate;
    }
}
