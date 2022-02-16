using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookShop.Services.Interface;
using BookShop.Domain.Dto;
using System.Security.Claims;
using BookShop.Domain.DomainModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace BookShop.Web.Controllers
{
    public class BooksController : Controller
    {
        private readonly IBookService _bookService;

        public BooksController(IBookService bookService)
        {
            _bookService = bookService;
        }

        // GET: Books
        public IActionResult Index()
        {
            var allBooks = this._bookService.GetAllBooks();
            return View(allBooks);
        }

        // GET: Books
        public IActionResult FilterByGenre(String genre)
        {
            Genre genreEnum = (Genre)Enum.Parse(typeof(Genre), genre);

            var allBooks = this._bookService.FilterByGenre(genreEnum);
            return View("Index" ,allBooks);
        }

        public IActionResult AddBookToCart(Guid? id)
        {
            var model = this._bookService.GetShoppingCartInfo(id);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddBookToCart([Bind("BookId", "Quantity")] AddToShoppingCartDto item)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = this._bookService.AddToShoppingCart(item, userId);

            if (result)
            {
                return RedirectToAction("Index", "Books");
            }

            return View(item);
        }

        // GET: Books/Details/5
        public IActionResult Details(Guid? b)
        {
            if (b == null)
            {
                return NotFound();
            }

            var book = this._bookService.GetDetailsForBook(b);

            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // GET: Books/Create
        [Authorize(Roles = "Administrator")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Books/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,BookName,BookImage,BookAuthor,Genre,Published,BookPrice,Rating")] Book book)
        {
            if (ModelState.IsValid)
            {
                this._bookService.CreateNewBook(book);
                return RedirectToAction(nameof(Index));
            }
            return View(book);
        }

        // GET: Books/Edit/5
        [Authorize(Roles = "Administrator")]
        public IActionResult Edit(Guid? b)
        {
            if (b == null)
            {
                return NotFound();
            }

            var book = this._bookService.GetDetailsForBook(b);

            if (book == null)
            {
                return NotFound();
            }
            return View(book);
        }

        // POST: Books/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Guid id, [Bind("Id,BookName,BookImage,BookAuthor,Genre,Published,BookPrice,Rating")] Book book)
        {
            if (id != book.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    this._bookService.UpdateExistingBook(book);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookExists(book.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(book);
        }

        // GET: Books/Delete/5
        [Authorize(Roles = "Administrator")]
        public IActionResult Delete(Guid? b)
        {
            if (b == null)
            {
                return NotFound();
            }

            var book = this._bookService.GetDetailsForBook(b);

            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // POST: Books/Delete/5
        [Authorize(Roles = "Administrator")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(Guid Id)
        {
            this._bookService.DeleteBook(Id);
            return RedirectToAction(nameof(Index));
        }

        private bool BookExists(Guid id)
        {
            return this._bookService.GetDetailsForBook(id) != null;
        }
    }
}
