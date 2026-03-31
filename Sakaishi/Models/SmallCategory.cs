using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sakaishi.Models
{
    public class SmallCategory : Category
    {
        public int LargeCategoryId { get; set; }
        public ICollection<Item> Items { get; } = new HashSet<Item>();
        public LargeCategory LargeCategory { get; set; } = null!;
    }
}
