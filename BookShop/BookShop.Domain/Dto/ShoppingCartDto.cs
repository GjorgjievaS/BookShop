using BookShop.Domain.DomainModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookShop.Domain.Dto
{
   public class ShoppingCartDto
    {
        public List<BookInShoppingCart> Books { get; set; }
        public double TotalPrice { get; set; }
    }
}
