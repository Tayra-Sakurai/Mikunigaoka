using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Hineno.Services;
using Sakaishi.Contexts;
using Sakaishi.Messages;
using Sakaishi.Models;
using Sakaishi.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sakaishi.ViewModels
{
    public partial class ItemsViewModel : ObservableObject
    {
        private IDatabaseService<SakaishiContext> databaseService;
        private IVectorService vectorService;

        [ObservableProperty]
        private ObservableCollection<Item> items;
        [ObservableProperty]
        private ObservableCollection<IGrouping<LargeCategory, SmallCategory>> categories;
        [ObservableProperty]
        private ObservableCollection<PaymentMethod> paymentMethods;
        [ObservableProperty]
        private SmallCategory filteringCategory;
        [ObservableProperty]
        private PaymentMethod paymentMethod;

        public ItemsViewModel(IDatabaseService<SakaishiContext> databaseService, IVectorService vectorService)
        {
            this.databaseService = databaseService;
            this.vectorService = vectorService;

            Items = [];
            Categories = [];
            PaymentMethods = [];
        }

        [RelayCommand(AllowConcurrentExecutions = false)]
        public async Task LoadAsync()
        {
            Items.Clear();
            Categories.Clear();
            PaymentMethods.Clear();

            IList<Item> itemList = await databaseService.GetEntitiesAsync<Item>();
            List<Item> itemList2 =
                itemList
                .OrderBy(i => i.DateTime)
                .ThenBy(i => i.Id)
                .ToList();
            foreach (Item item in itemList2)
                Items.Add(item);

            IList<SmallCategory> smallCategories = await databaseService.GetEntitiesAsync(context => context.SmallCategories);
            foreach (IGrouping<LargeCategory, SmallCategory> categoryGroup in smallCategories.GroupBy(c => c.LargeCategory))
                Categories.Add(categoryGroup);

            foreach (PaymentMethod method in await databaseService.GetEntitiesAsync(context => context.PaymentMethods))
                PaymentMethods.Add(method);
        }

        [RelayCommand(AllowConcurrentExecutions = false, CanExecute = nameof(CanDelete))]
        public async Task DeleteAsync(Item item)
        {
            await databaseService.DeleteAsync(item);
            await LoadAsync();

            WeakReferenceMessenger.Default.Send(new ItemDeletedMessage(item));
        }

        [RelayCommand(AllowConcurrentExecutions = false, CanExecute = nameof(IsSearchStringValid))]
        public async Task SearchAsync(string query)
        {
            List<float> scores = [];
            await foreach (float score in vectorService.GetMatchRatesAsync(query, Items.Select(i => i.Vector)))
                scores.Add(score);
            List<Item> values =
                Items.Zip(scores)
                .OrderByDescending(i => i.Second)
                .ThenBy(i => i.First.Id)
                .Select(i => i.First)
                .ToList();

            Items.Clear();

            foreach (Item item in values)
                Items.Add(item);
        }

        [RelayCommand(AllowConcurrentExecutions = false, CanExecute = nameof(CanApplyFilter))]
        public async Task ApplyFilterAsync()
        {
            Items.Clear();

            IList<Item> items = await databaseService.FilterAndGetEntitiesAsync(context => context.Items,
                i => i.CategoryId == FilteringCategory.Id && i.PaymentMethodId == PaymentMethod.Id);

            foreach (Item item in items)
                Items.Add(item);
        }

        private bool CanApplyFilter()
        {
            return FilteringCategory is not null &&
                PaymentMethod is not null;
        }

        private static bool IsSearchStringValid(string query)
        {
            return !string.IsNullOrWhiteSpace(query);
        }

        private bool CanDelete(Item item)
        {
            return item is not null && databaseService.Exists(item);
        }
    }
}
