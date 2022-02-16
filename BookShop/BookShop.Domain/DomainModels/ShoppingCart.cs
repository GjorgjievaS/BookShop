using BookShop.Domain.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookShop.Domain.DomainModels
{
   public class ShoppingCart : BaseEntity
    {
        public string OwnerId { get; set; }
        public BookShopUser Owner { get; set; }
        public virtual ICollection<BookInShoppingCart> BookInShoppingCarts { get; set; }
    }
}
