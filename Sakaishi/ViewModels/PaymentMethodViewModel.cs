using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Sakaishi.Contexts;
using Sakaishi.Messages;
using Sakaishi.Models;
using Sakaishi.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sakaishi.ViewModels
{
    public partial class PaymentMethodViewModel : ObservableValidator
    {
        private readonly IDatabaseService<SakaishiContext> databaseService;
        private PaymentMethod paymentMethod;

        public PaymentMethodViewModel(IDatabaseService<SakaishiContext> databaseService)
        {
            this.databaseService = databaseService;

            paymentMethod = new();
        }

        public async Task InitializeForExistingValue(PaymentMethod paymentMethod)
        {
            this.paymentMethod = paymentMethod;

            OnPropertyChanged(nameof(Name));
            OnPropertyChanged(nameof(Balance));
        }

        [RelayCommand(AllowConcurrentExecutions = false, CanExecute = nameof(CanSave))]
        public async Task UpdateAsync()
        {
            await databaseService.UpdateAsync(paymentMethod);
        }

        private bool CanSave()
        {
            return !string.IsNullOrEmpty(Name);
        }

        [Required]
        public string Name
        {
            get => paymentMethod.Name;
            set
            {
                SetProperty(paymentMethod.Name, value, paymentMethod, (m, v) => m.Name = v, true);
                UpdateCommand.NotifyCanExecuteChanged();
            }
        }

        public double? Balance => paymentMethod?.Items.Sum(i => i.Income - i.Expense);

        private bool CanDelete()
        {
            return databaseService.Exists(paymentMethod);
        }

        [RelayCommand(AllowConcurrentExecutions = false, CanExecute = nameof(CanDelete))]
        public async Task DeleteAsync()
        {
            await databaseService.DeleteAsync(paymentMethod);

            WeakReferenceMessenger.Default.Send(new PaymentMethodDeletedMessage(paymentMethod));
        }

        [RelayCommand(AllowConcurrentExecutions = false, CanExecute = nameof(CanSave))]
        public async Task AddAsync()
        {
            await databaseService.AddAsync(paymentMethod);

            WeakReferenceMessenger.Default.Send(new PaymentMethodAddedMessage(paymentMethod));
        }
    }
}
