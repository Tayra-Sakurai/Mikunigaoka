using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using Otori.ViewModels;
using Sakaishi.Messages;
using Sakaishi.Models;
using Sakaishi.ViewModels;

namespace Mikunigaoka
{
    public sealed partial class SmallCategoryEditionPage : Page, IRecipient<SmallCategoryDeletedMessage>
    {
        private readonly SmallCategoryViewModel viewModel;
        private readonly SettingsViewModel settingsViewModel;

        public SmallCategoryEditionPage()
        {
            InitializeComponent();

            viewModel = App.Current.Service.GetRequiredService<SmallCategoryViewModel>();
            settingsViewModel = App.Current.Service.GetRequiredService<SettingsViewModel>();
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            WeakReferenceMessenger.Default.Register(this);

            await viewModel.LoadAsync();

            if (e.Parameter is SmallCategory smallCategory)
                await viewModel.InitializeForExistingAsync(smallCategory);

            SuperDescriptions.Text = "小分類を編集します．";
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            WeakReferenceMessenger.Default.UnregisterAll(this);
            base.OnNavigatedFrom(e);
        }

        public void Receive(SmallCategoryDeletedMessage message)
        {
            if (Frame is null)
                return;

            if (Frame.CanGoBack)
            {
                Frame.GoBack();
                return;
            }

            if (settingsViewModel.IsInitialized)
                Frame.Navigate(typeof(MainPage));
        }
    }
}

