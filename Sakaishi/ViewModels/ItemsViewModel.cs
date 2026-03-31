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
    public partial class ItemsViewModel : ObservableObject
    {
        private IDatabaseService<SakaishiContext> databaseService;
        private IVectorService vectorService;

        [ObservableProperty]
        private ObservableCollection<Item> items;

        public ItemsViewModel(IDatabaseService<SakaishiContext> databaseService, IVectorService vectorService)
        {
            this.databaseService = databaseService;
            this.vectorService = vectorService;

            Items = [];
        }

        [RelayCommand(AllowConcurrentExecutions = false)]
        public async Task LoadAsync()
        {
            Items.Clear();
            IList<Item> itemList = await databaseService.GetEntitiesAsync<Item>();
            List<Item> itemList2 =
                itemList
                .OrderBy(i => i.DateTime)
                .ThenBy(i => i.Id)
                .ToList();
            foreach (Item item in itemList2)
                Items.Add(item);
        }

        [RelayCommand(AllowConcurrentExecutions = false)]
        public async Task DeleteAsync(Item item)
        {
            await databaseService.DeleteAsync(item);
            await LoadAsync();
        }


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
    }
}
