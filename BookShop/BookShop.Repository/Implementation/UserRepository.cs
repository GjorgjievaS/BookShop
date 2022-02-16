using BookShop.Domain.Identity;
using BookShop.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BookShop.Repository.Implementation
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext context;
        private DbSet<BookShopUser> entities;
        string errorMessage = string.Empty;

        public UserRepository(ApplicationDbContext context)
        {
            this.context = context;
            entities = context.Set<BookShopUser>();
        }
        public IEnumerable<BookShopUser> GetAll()
        {
            return entities.AsEnumerable();
        }

        public BookShopUser Get(string id)
        {
            return entities
               .Include(z => z.UserCart)
               .Include("UserCart.BookInShoppingCarts")
               .Include("UserCart.BookInShoppingCarts.Book")
               .SingleOrDefault(s => s.Id == id);
        }
        public void Insert(BookShopUser entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.Add(entity);
            context.SaveChanges();
        }

        public void Update(BookShopUser entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.Update(entity);
            context.SaveChanges();
        }

        public void Delete(BookShopUser entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.Remove(entity);
            context.SaveChanges();
        }
    }
}
