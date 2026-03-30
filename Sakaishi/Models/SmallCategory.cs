using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sakaishi.Models
{
    public class SmallCategory
    {
        public int Id { get; set; }
        public int LargeCategoryId { get; set; }
        public string Name { get; set; }
        public ICollection<Item> Items { get; } = new HashSet<Item>();
        public LargeCategory LargeCategory { get; set; } = null!;
        public float[] Vector { get; set; }
    }
}
