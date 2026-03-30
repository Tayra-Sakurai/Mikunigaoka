using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sakaishi.Models
{
    public class PaymentMethod
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Item> Items { get; set; } = new HashSet<Item>();
    }
}
