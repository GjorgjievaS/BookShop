using BookShop.Domain.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookShop.Repository.Interface
{
    public interface IUserRepository
    {
        IEnumerable<BookShopUser> GetAll();
        BookShopUser Get(string id);
        void Insert(BookShopUser entity);
        void Update(BookShopUser entity);
        void Delete(BookShopUser entity);
    }
}
