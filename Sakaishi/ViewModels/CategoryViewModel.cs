using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Hineno.Services;
using Sakaishi.Contexts;
using Sakaishi.Models;
using Sakaishi.Services;
using Sakaishi.Validations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sakaishi.ViewModels
{
    public partial class CategoryViewModel<TCategory> : ObservableValidator
        where TCategory : Category, new()
    {
        protected TCategory category;
        protected readonly IDatabaseService<SakaishiContext> databaseService;
        protected readonly IVectorService vectorService;

        public CategoryViewModel(IDatabaseService<SakaishiContext> databaseService, IVectorService vectorService)
        {
            this.databaseService = databaseService;
            this.vectorService = vectorService;

            category = new();
        }

        public virtual async Task InitializeForExistingAsync(TCategory category)
        {
            this.category = category;

            OnPropertyChanged(nameof(Name));
        }

        [Required]
        [StringNoWhiteSpaceValidation]
        public string Name
        {
            get => category.Name;
            set
            {
                SetProperty(category.Name, value, category, (m, v) => m.Name = v, true);
                SaveCommand.NotifyCanExecuteChanged();
            }
        }

        [RelayCommand(AllowConcurrentExecutions = false, CanExecute = nameof(CanSave))]
        public async Task SaveAsync()
        {
            category.Vector = await vectorService.GenerateVectorAsync(category.Name);

            await databaseService.UpdateAsync(category);
        }

        protected virtual bool CanSave()
        {
            return !string.IsNullOrWhiteSpace(Name);
        }
    }
}
