using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BookShop.Domain.DomainModels
{
    public enum Genre
    {
        Fantasy,
        Action,
        Adventure,
        Mystery,
        Horror,
        Thriller,
        Historical,
        Romance,
        Biography,
        Fiction,
        Crime_novel
    }
    public class Book : BaseEntity
    {
        [Required]
        public string BookName { get; set; }
        [Required]
        public string BookImage { get; set; }
        [Required]
        public string BookAuthor { get; set; }
        [Required]
        public Genre Genre { get; set; }
        [Required]
        public DateTime Published { get; set; }
        [Required]
        public int BookPrice { get; set; }
        [Required]
        public int Rating { get; set; }

        public virtual ICollection<BookInShoppingCart> BookInShoppingCarts { get; set; }
        public IEnumerable<BookInOrder> BookInOrders { get; set; }
      
    }
}
