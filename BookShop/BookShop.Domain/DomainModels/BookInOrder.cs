using System;
using System.Collections.Generic;
using System.Text;

namespace BookShop.Domain.DomainModels
{
   public class BookInOrder : BaseEntity
    {
        public Guid BookId { get; set; }
        public Book OrderedBook { get; set; }

        public Guid OrderId { get; set; }
        public Order UserOrder { get; set; }
        public int Quantity { get; set; }
    }
}
