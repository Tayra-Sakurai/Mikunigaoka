using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
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
    public partial class PaymentMethodsViewModel : ObservableObject
    {
        private readonly IDatabaseService<SakaishiContext> databaseService;

        [ObservableProperty]
        private ObservableCollection<PaymentMethod> paymentMethods;

        public PaymentMethodsViewModel(IDatabaseService<SakaishiContext> databaseService)
        {
            this.databaseService = databaseService;

            PaymentMethods = [];
        }

        [RelayCommand(AllowConcurrentExecutions = false)]
        public async Task LoadAsync()
        {
            PaymentMethods.Clear();

            foreach (PaymentMethod paymentMethod in await databaseService.GetEntitiesAsync(context => context.PaymentMethods, method => method.Items))
                PaymentMethods.Add(paymentMethod);
        }

        public static double GetBalance(ICollection<Item> items)
        {
            return items.Sum(item => item.Income - item.Expense);
        }
    }
}
