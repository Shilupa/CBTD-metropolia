using System;
using DataAccess.Data;
using DataAccess.Models;
using Infrastructure.Interfaces;

namespace Infrastructure
{
    public class OrderHeaderRepository : GenericRepository<OrderHeader>, IOrderHeaderRepository<OrderHeader>
    {
        private readonly ApplicationDbContext _db;

        public OrderHeaderRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void UpdateStatus(int id, string orderStatus, string? paymentStatus = null)
        {
            var orderFromDb = _db.OrderHeaders.FirstOrDefault(u => u.Id == id);
            if (orderFromDb != null)
            {
                orderFromDb.OrderStatus = orderStatus;
                if (paymentStatus != null)
                {
                    orderFromDb.PaymentStatus = paymentStatus;
                }
            }
        }

    }
}


