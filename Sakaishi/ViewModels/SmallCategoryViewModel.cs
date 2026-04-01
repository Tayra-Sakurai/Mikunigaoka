using CommunityToolkit.Mvvm.ComponentModel;
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
    public partial class SmallCategoryViewModel : CategoryViewModel<SmallCategory>
    {
        public SmallCategoryViewModel(IDatabaseService<SakaishiContext> databaseService, IVectorService vectorService)
            : base(databaseService, vectorService)
        {
            LargeCategories = [];
        }

        [ObservableProperty]
        private ObservableCollection<LargeCategory> largeCategories;

        public LargeCategory LargeCategory
        {
            get => category.LargeCategory;
            set => SetProperty(category.LargeCategory, value, category, (m, v) => m.LargeCategory = v, true);
        }

        public override async Task InitializeForExistingAsync(SmallCategory category)
        {
            await base.InitializeForExistingAsync(category);

            LargeCategories.Clear();

            foreach (var largeCategory in await databaseService.GetEntitiesAsync<LargeCategory>())
                LargeCategories.Add(largeCategory);

            OnPropertyChanged(nameof(LargeCategories));
            OnPropertyChanged(nameof(LargeCategory));
        }
    }
}
