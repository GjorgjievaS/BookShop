using BookShop.Domain.DomainModels;
using BookShop.Domain.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookShop.Services.Interface
{
    public interface IBookService
    {
        List<Book> GetAllBooks();

        List<Book> FilterByGenre(Genre genre);
        Book GetDetailsForBook(Guid? id);
        void CreateNewBook(Book b);
        void UpdateExistingBook(Book b);
        AddToShoppingCartDto GetShoppingCartInfo(Guid? id);
        void DeleteBook(Guid id);
        bool AddToShoppingCart(AddToShoppingCartDto item, string userID);
    }
}
