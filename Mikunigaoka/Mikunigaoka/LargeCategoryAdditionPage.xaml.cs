using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.Windows.ApplicationModel.Resources;
using Otori.ViewModels;
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
    public sealed partial class LargeCategoryAdditionPage : Page, IRecipient<LargeCategoryAddedMessage>
    {
        private SettingsViewModel viewModel;

        public LargeCategoryAdditionPage()
        {
            InitializeComponent();

            DataContext = App.Current.Service.GetRequiredService<LargeCategoryViewModel>();
            viewModel = App.Current.Service.GetRequiredService<SettingsViewModel>();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            ResourceLoader resourceLoader = new();

            if (viewModel.IsInitialized is not true)
            {
                SuperDescriptionBlock.Text = resourceLoader.GetString("LargeCategoryAdditionDescription");
                return;
            }

            SuperDescriptionBlock.Text = resourceLoader.GetString("LargecategoryAdditionNormalDescription");
        }

        public void Receive(LargeCategoryAddedMessage message)
        {
            if (viewModel.IsInitialized is true)
                Frame.Navigate(typeof(SmallCategoryAdditionPage));
        }
    }
}
