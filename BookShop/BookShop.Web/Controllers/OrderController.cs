﻿using BookShop.Domain.DomainModels;
using BookShop.Domain.Identity;
using BookShop.Services.Interface;
using ClosedXML.Excel;
using GemBox.Document;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.Web.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly UserManager<BookShopUser> userManager;

        public OrderController(IOrderService orderService, UserManager<BookShopUser> userManager)
        {
            this._orderService = orderService;
            this.userManager = userManager;
            ComponentInfo.SetLicense("FREE-LIMITED-KEY");
        }
      

        public IActionResult Index()
        {
            HttpClient client = new HttpClient();


            string URI = "https://localhost:44377/api/Admin/GetOrders";

            HttpResponseMessage responseMessage = client.GetAsync(URI).Result;

            var result = responseMessage.Content.ReadAsAsync<List<Order>>().Result;

            return View(result);
        }

        public IActionResult Details(Guid id)
        {
            //HttpClient client = new HttpClient();


            //string URI = "https://localhost:44377/api/Admin/GetDetailsForOrder";

            //var model = new
            //{
            //    Id = id
            //};

            //HttpContent content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");

            //HttpResponseMessage responseMessage = client.PostAsync(URI, content).Result;


            //var result = responseMessage.Content.ReadAsAsync<Order>().Result;

            BaseEntity baseEntity = new BaseEntity();
            baseEntity.Id = id;
            Order result = this._orderService.getOrderDetails(baseEntity);

            return View(result);
        }

        [Authorize(Roles = "Administrator")]
        [HttpGet]
        public FileContentResult ExportAllOrders()
        {
            string fileName = "Orders.xlsx";
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

            using (var workbook = new XLWorkbook())
            {
                IXLWorksheet worksheet = workbook.Worksheets.Add("All Orders");

                worksheet.Cell(1, 1).Value = "Order Id";
                worksheet.Cell(1, 2).Value = "Costumer Email";


                HttpClient client = new HttpClient();


                string URI = "https://localhost:44377/api/Admin/GetOrders";

                HttpResponseMessage responseMessage = client.GetAsync(URI).Result;

                var result = responseMessage.Content.ReadAsAsync<List<Order>>().Result;

                for (int i = 1; i <= result.Count(); i++)
                {
                    var item = result[i - 1];

                    worksheet.Cell(i + 1, 1).Value = item.Id.ToString();
                    worksheet.Cell(i + 1, 2).Value = item.User.Email;

                    for (int p = 0; p < item.BookInOrders.Count(); p++)
                    {
                        worksheet.Cell(1, p + 3).Value = "Book-" + (p + 1);
                        worksheet.Cell(i + 1, p + 3).Value = item.BookInOrders.ElementAt(p).OrderedBook.BookName;
                    }
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();

                    return File(content, contentType, fileName);
                }

            }
        }

        public FileContentResult CreateInvoice(Guid id)
        {
            HttpClient client = new HttpClient();


            string URI = "https://localhost:44377/api/Admin/GetDetailsForOrder";

            var model = new
            {
                Id = id
            };

            HttpContent content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");

            HttpResponseMessage responseMessage = client.PostAsync(URI, content).Result;


            var result = responseMessage.Content.ReadAsAsync<Order>().Result;

            var templatePath = Path.Combine(Directory.GetCurrentDirectory(), "Invoice.docx");
            var document = DocumentModel.Load(templatePath);


            document.Content.Replace("{{OrderNumber}}", result.Id.ToString());
            document.Content.Replace("{{UserName}}", result.User.UserName);

            StringBuilder sb = new StringBuilder();

            var totalPrice = 0.0;

            foreach (var item in result.BookInOrders)
            {
                totalPrice += item.Quantity * item.OrderedBook.BookPrice;
                sb.AppendLine(item.OrderedBook.BookName + " with quantity of: " + item.Quantity + " and price of: " + item.OrderedBook.BookPrice + "$");
            }


            document.Content.Replace("{{BookList}}", sb.ToString());
            document.Content.Replace("{{TotalPrice}}", totalPrice.ToString() + "$");


            var stream = new MemoryStream();

            document.Save(stream, new PdfSaveOptions());

            return File(stream.ToArray(), new PdfSaveOptions().ContentType, "ExportInvoice.pdf");
        }

        [HttpGet]
        public async Task<IActionResult> GetUserOrders()
        {
            BookShopUser user = await this.userManager.GetUserAsync(User);

            List<Order> orders = this._orderService.getAllOrders();

            List<Order> userOrders = new List<Order>();

            foreach (var item in orders)
            {
                if (item.UserId.Equals(user.Id))
                {
                    userOrders.Add(item);
                }
            }

            return View(userOrders);
        }
    }
}
