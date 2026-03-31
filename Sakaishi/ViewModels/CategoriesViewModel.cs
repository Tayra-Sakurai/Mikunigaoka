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
    public partial class CategoriesViewModel : ObservableObject
    {
        private IDatabaseService<SakaishiContext> databaseService;
        private IVectorService vectorService;

        [ObservableProperty]
        private ObservableCollection<LargeCategory> categories;

        public CategoriesViewModel(IDatabaseService<SakaishiContext> databaseService, IVectorService vectorService)
        {
            this.databaseService = databaseService;
            this.vectorService = vectorService;

            Categories = [];
        }

        [RelayCommand(AllowConcurrentExecutions = false)]
        public async Task LoadAsync()
        {
            Categories.Clear();

            foreach (var category in await databaseService.GetEntitiesAsync(context => context.LargeCategories))
                Categories.Add(category);
        }

        public async Task SearchAsync(string query)
        {
            List<float> scores = [];

            await foreach (var score in vectorService.GetMatchRatesAsync(query, Categories.Select(c => c.Vector).ToList()))
                scores.Add(score);

            List<LargeCategory> largeCategories =
                Categories.Zip(scores)
                .OrderByDescending(c => c.Second)
                .ThenBy(c => c.First.Id)
                .Select(c => c.First)
                .ToList();

            Categories.Clear();

            foreach (LargeCategory category in largeCategories)
                Categories.Add(category);
        }
    }
}
