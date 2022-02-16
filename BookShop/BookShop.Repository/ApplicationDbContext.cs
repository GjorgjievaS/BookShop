using BookShop.Domain.DomainModels;
using BookShop.Domain.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookShop.Repository
{
    public class ApplicationDbContext : IdentityDbContext<BookShopUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Book> Book { get; set; }
        public virtual DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public virtual DbSet<BookInShoppingCart> BookInShoppingCarts { get; set; }
        public virtual DbSet<BookInOrder> BookInOrders { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<EmailMessage> EmailMessages { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Book>()
                .Property(z => z.Id)
                .ValueGeneratedOnAdd();

            builder.Entity<ShoppingCart>()
                .Property(z => z.Id)
                .ValueGeneratedOnAdd();

            //builder.Entity<ProductInShoppingCart>()
            //    .HasKey(z => new { z.ProductId, z.ShoppingCartId });

            builder.Entity<BookInShoppingCart>()
                .HasOne(z => z.Book)
                .WithMany(z => z.BookInShoppingCarts)
                .HasForeignKey(z => z.ShoppingCartId);

            builder.Entity<BookInShoppingCart>()
                .HasOne(z => z.ShoppingCart)
                .WithMany(z => z.BookInShoppingCarts)
                .HasForeignKey(z => z.BookId);


            builder.Entity<ShoppingCart>()
                .HasOne<BookShopUser>(z => z.Owner)
                .WithOne(z => z.UserCart)
                .HasForeignKey<ShoppingCart>(z => z.OwnerId);


            //builder.Entity<ProductInOrder>()
            //   .HasKey(z => new { z.ProductId, z.OrderId });

            builder.Entity<BookInOrder>()
                .HasOne(z => z.OrderedBook)
                .WithMany(z => z.BookInOrders)
                .HasForeignKey(z => z.OrderId);

            builder.Entity<BookInOrder>()
                .HasOne(z => z.UserOrder)
                .WithMany(z => z.BookInOrders)
                .HasForeignKey(z => z.BookId);
        }
    }
}
