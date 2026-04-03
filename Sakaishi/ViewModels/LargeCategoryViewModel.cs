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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sakaishi.ViewModels
{
    public partial class LargeCategoryViewModel : CategoryViewModel<LargeCategory>
    {
        public LargeCategoryViewModel(IDatabaseService<SakaishiContext> databaseService, IVectorService vectorService)
            : base(databaseService, vectorService) { }

        [RelayCommand(AllowConcurrentExecutions = false, CanExecute = nameof(CanAdd))]
        public async Task AddAsync()
        {
            category.Vector = await vectorService.GenerateVectorAsync(category.Name);

            await databaseService.AddAsync(category);

            WeakReferenceMessenger.Default.Send(new LargeCategoryAddedMessage(category));
        }

        public bool CanAdd()
        {
            return !databaseService.Exists(category) && !string.IsNullOrWhiteSpace(Name);
        }

        public override async Task DeleteAsync()
        {
            await base.DeleteAsync();

            WeakReferenceMessenger.Default.Send(new LargeCategoryDeletedMessage(category));
        }

        public new string Name
        {
            get => category.Name;
            set
            {
                SetProperty(category.Name, value, category, (m, v) => m.Name = v, true);

                SaveCommand.NotifyCanExecuteChanged();
                AddCommand.NotifyCanExecuteChanged();
            }
        }
    }
}
