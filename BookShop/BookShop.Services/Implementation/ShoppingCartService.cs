using BookShop.Domain.DomainModels;
using BookShop.Domain.Dto;
using BookShop.Repository.Interface;
using BookShop.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BookShop.Services.Implementation
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly IRepository<ShoppingCart> _shoppingCartRepositorty;
        private readonly IRepository<Order> _orderRepositorty;
        private readonly IRepository<BookInOrder> _bookInOrderRepositorty;
        private readonly IUserRepository _userRepository;
        private readonly IRepository<EmailMessage> _mailRepository;

        public ShoppingCartService(IRepository<EmailMessage> mailRepository, IRepository<ShoppingCart> shoppingCartRepository, IRepository<BookInOrder> bookInOrderRepositorty, IRepository<Order> orderRepositorty, IUserRepository userRepository)
        {
            _shoppingCartRepositorty = shoppingCartRepository;
            _userRepository = userRepository;
            _orderRepositorty = orderRepositorty;
            _bookInOrderRepositorty = bookInOrderRepositorty;
            _mailRepository = mailRepository;
        }

        public bool deleteBookFromShoppingCart(string userId, Guid id)
        {
            if (!string.IsNullOrEmpty(userId) && id != null)
            {
                //Select * from Users Where Id LIKE userId

                var loggedInUser = this._userRepository.Get(userId);

                var userShoppingCart = loggedInUser.UserCart;

                var itemToDelete = userShoppingCart.BookInShoppingCarts.Where(z => z.BookId.Equals(id)).FirstOrDefault();

                userShoppingCart.BookInShoppingCarts.Remove(itemToDelete);

                this._shoppingCartRepositorty.Update(userShoppingCart);

                return true;
            }

            return false;
        }

        public ShoppingCartDto getShoppingCartInfo(string userId)
        {
            var loggedInUser = this._userRepository.Get(userId);

            var userShoppingCart = loggedInUser.UserCart;

            var AllBooks = userShoppingCart.BookInShoppingCarts.ToList();

            var allBookPrice = AllBooks.Select(z => new
            {
                BookPrice = z.Book.BookPrice,
                Quanitity = z.Quantity
            }).ToList();

            var totalPrice = 0;


            foreach (var item in allBookPrice)
            {
                totalPrice += item.Quanitity * item.BookPrice;
            }


            ShoppingCartDto scDto = new ShoppingCartDto
            {
                Books = AllBooks,
                TotalPrice = totalPrice
            };


            return scDto;

        }

        public bool orderNow(string userId)
        {
            if (!string.IsNullOrEmpty(userId))
            {
                //Select * from Users Where Id LIKE userId

                var loggedInUser = this._userRepository.Get(userId);

                var userShoppingCart = loggedInUser.UserCart;

                EmailMessage mail = new EmailMessage();
                mail.MailTo = loggedInUser.Email;
                mail.Subject = "Successfully created order";
                mail.Status = false;

                Order order = new Order
                {
                    Id = Guid.NewGuid(),
                    User = loggedInUser,
                    UserId = userId
                };

                this._orderRepositorty.Insert(order);

                List<BookInOrder> bookInOrders = new List<BookInOrder>();

                var result = userShoppingCart.BookInShoppingCarts.Select(z => new BookInOrder
                {
                    Id = Guid.NewGuid(),
                    BookId = z.Book.Id,
                    OrderedBook = z.Book,
                    OrderId = order.Id,
                    Quantity = z.Quantity,
                    UserOrder = order
                }).ToList();

                StringBuilder sb = new StringBuilder();

                var totalPrice = 0;

                sb.AppendLine("Your order is completed. The order contains: ");

                for (int i = 1; i <= result.Count(); i++)
                {
                    var item = result[i - 1];

                    totalPrice += item.Quantity * item.OrderedBook.BookPrice;

                    sb.AppendLine(i.ToString() + ". " + item.OrderedBook.BookName + " with price of: " + item.OrderedBook.BookPrice + " and quantity of: " + item.Quantity);
                }

                sb.AppendLine("Total price: " + totalPrice.ToString());


                mail.Content = sb.ToString();


                bookInOrders.AddRange(result);

                foreach (var item in bookInOrders)
                {
                    this._bookInOrderRepositorty.Insert(item);
                }

                loggedInUser.UserCart.BookInShoppingCarts.Clear();

                this._userRepository.Update(loggedInUser);
                this._mailRepository.Insert(mail);

                return true;
            }
            return false;
        }
    }
}
