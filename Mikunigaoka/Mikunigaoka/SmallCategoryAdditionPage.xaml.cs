using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Sakaishi.Messages;
using Sakaishi.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Mikunigaoka
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SmallCategoryAdditionPage : Page, IRecipient<SmallCategoryAddedMessage>
    {
        private readonly SmallCategoryViewModel viewModel;

        public SmallCategoryAdditionPage()
        {
            InitializeComponent();

            viewModel = App.Current.Service.GetRequiredService<SmallCategoryViewModel>();
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            WeakReferenceMessenger.Default.RegisterAll(this);

            await viewModel.LoadAsync();
        }

        public void Receive(SmallCategoryAddedMessage message)
        {
            WeakReferenceMessenger.Default.UnregisterAll(this);
        }
    }
}
