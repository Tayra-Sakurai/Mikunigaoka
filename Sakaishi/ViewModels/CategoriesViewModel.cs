using CommunityToolkit.Mvvm.ComponentModel;
using Hineno.Services;
using Sakaishi.Contexts;
using Sakaishi.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sakaishi.ViewModels
{
    public partial class CategoriesViewModel : ObservableObject
    {
        private IDatabaseService<SakaishiContext> databaseService;
        private IVectorService vectorService;

        public CategoriesViewModel(IDatabaseService<SakaishiContext> databaseService, IVectorService vectorService)
        {
            this.databaseService = databaseService;
            this.vectorService = vectorService;
        }
    }
}
