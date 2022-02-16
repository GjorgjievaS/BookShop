using BookShop.Domain.DomainModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookShop.Repository.Interface
{
    public interface IOrderRepository
    {
        List<Order> getAllOrders();
        Order getOrderDetails(BaseEntity model);
    }
}
