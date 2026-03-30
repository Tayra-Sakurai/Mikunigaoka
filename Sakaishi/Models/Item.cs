using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sakaishi.Models
{
    public class Item
    {
        public int Id { get; set; }
        public DateTime DateTime { get; set; } = DateTime.Now;
        public int CategoryId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int PaymentMethodId { get; set; }
        public double Expense { get; set; } = 0;
        public double Income { get; set; } = 0;
        public float[] Vector { get; set; }
        public SmallCategory Category { get; set; } = null!;
        public PaymentMethod PaymentMethod { get; set; } = null!;
    }
}
