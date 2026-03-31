using Hineno.Services;
using Sakaishi.Contexts;
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
    }
}
