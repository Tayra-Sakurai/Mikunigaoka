using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Hineno.Services;
using Sakaishi.Contexts;
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
    public partial class ItemViewModel : ObservableObject
    {
        private readonly IDatabaseService<SakaishiContext> databaseService;
        private readonly IVectorService vectorService;
        private Item model;

        public ItemViewModel(IDatabaseService<SakaishiContext> databaseService, IVectorService vectorService)
        {
            this.databaseService = databaseService;
            this.vectorService = vectorService;

            model = new();

            LargeCategories = [];
            SmallCategories = [];
            PaymentMethods = [];
        }

        public async Task InitializeForExistingValue(Item item)
        {
            model = item;

            OnPropertyChanged();

            LargeCategories.Clear();
            SmallCategories.Clear();
            PaymentMethods.Clear();

            foreach (var largeCategory in await databaseService.GetEntitiesAsync(context => context.LargeCategories))
                LargeCategories.Add(largeCategory);

            foreach (var smallCategory in model.Category.LargeCategory.SmallCategories)
                SmallCategories.Add(smallCategory);

            foreach (var paymentMethod in await databaseService.GetEntitiesAsync(context => context.PaymentMethods))
                PaymentMethods.Add(paymentMethod);
        }

        [RelayCommand(AllowConcurrentExecutions = false)]
        public async Task SaveAsync()
        {
            // Calculate the vector.

            float[] vector = await vectorService.GenerateVectorAsync(Title, Description);

            model.Vector = vector;

            // Save the model to database.

            await databaseService.UpdateAsync(model);
        }

        [ObservableProperty]
        private ObservableCollection<LargeCategory> largeCategories;

        [ObservableProperty]
        private ObservableCollection<SmallCategory> smallCategories;

        [ObservableProperty]
        private ObservableCollection<PaymentMethod> paymentMethods;

        public DateTimeOffset Date
        {
            get => model.DateTime.Date;
            set => SetProperty(model.DateTime.Date, value, model, SetDate);
        }

        public TimeSpan Time
        {
            get => model.DateTime.TimeOfDay;
            set => SetProperty(model.DateTime.TimeOfDay, value, model, SetTime);
        }

        public string Title
        {
            get => model.Title;
            set => SetProperty(model.Title, value, model, (m, v) => m.Title = v);
        }

        public string Description
        {
            get => model.Description;
            set => SetProperty(model.Description, value, model, (m, v) => m.Description = v);
        }

        public LargeCategory LargeCategory
        {
            get => model.Category.LargeCategory;
            set => SetProperty(model.Category.LargeCategory, value, model, SetLargeCategory);
        }

        public SmallCategory SmallCategory
        {
            get => model.Category;
            set => SetProperty(model.Category, value, model, (m, v) => m.Category = v);
        }

        public double Expense
        {
            get => model.Expense;
            set => SetProperty(model.Expense, value, model, (m, v) => m.Expense = v);
        }

        public double Income
        {
            get => model.Income;
            set => SetProperty(model.Income, value, model, (m, v) => m.Income = v);
        }

        private static void SetDate(Item model, DateTimeOffset value)
        {
            TimeSpan timeOfDay = model.DateTime.TimeOfDay;
            model.DateTime = value.Date.Add(timeOfDay);
        }

        private static void SetTime(Item model, TimeSpan value)
        {
            model.DateTime = model.DateTime.Date.Add(value);
        }

        private void SetLargeCategory(Item model, LargeCategory value)
        {
            model.Category = value.SmallCategories.First();
            OnPropertyChanged(nameof(SmallCategory));

            SmallCategories.Clear();
            foreach (SmallCategory small in value.SmallCategories)
                SmallCategories.Add(small);
        }
    }
}
