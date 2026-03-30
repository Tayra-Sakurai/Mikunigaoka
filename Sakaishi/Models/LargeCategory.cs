using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sakaishi.Models
{
    public class LargeCategory
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public float[] Vector { get; set; }
        public ICollection<SmallCategory> SmallCategories { get; } = new HashSet<SmallCategory>();
    }
}
