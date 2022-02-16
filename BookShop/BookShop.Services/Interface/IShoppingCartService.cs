using BookShop.Domain.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookShop.Services.Interface
{
    public interface IShoppingCartService
    {
        ShoppingCartDto getShoppingCartInfo(string userId);
        bool deleteBookFromShoppingCart(string userId, Guid id);
        bool orderNow(string userId);
    }
}
