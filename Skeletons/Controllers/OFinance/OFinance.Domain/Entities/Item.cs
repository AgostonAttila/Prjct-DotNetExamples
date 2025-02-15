using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OFinance.Domain.Entities
{
    public class Item
    {
        public Guid Id { get; set; }
        public Guid Account { get; set; }
        public string Title { get; set; } = string.Empty;
        public double Amount { get; set; }
        public string Category { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
