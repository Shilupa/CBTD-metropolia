using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccess.Models;
using Infrastructure;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Utility;

namespace CBTD.Pages.Cart
{
    public class OrderConfirmationModel : PageModel
    {
        [BindProperty]
        public int OrderId { get; set; }
        private readonly UnitOfWork _unitOfWork;
        private readonly IEmailSender _emailSender;
        public OrderConfirmationModel(UnitOfWork unitOfWork, IEmailSender emailSender)
        {
            _unitOfWork = unitOfWork;
            _emailSender = emailSender;
        }
        public void OnGet(int orderId)
        {
            OrderHeader objOrderHeader = _unitOfWork.OrderHeader.Get(u => u.Id == orderId, includes: "ApplicationUser");
            OrderId = objOrderHeader.Id;
            //var service = new SessionService();
            //Session session = service.Get(objOrderHeader.SessionId);
            ////check the stripe status
            //if (session.PaymentStatus.ToLower() == "paid")
            //{
            //    _unitOfWork.OrderHeader.UpdateStatus(orderId, SD.StatusApproved, SD.PaymentStatusApproved);
            //    _unitOfWork.Commit();
            //}
            //Send an e-mail

            _emailSender.SendEmailAsync(objOrderHeader.ApplicationUser.Email, "New Order - CBTD", "<p>New Order Created.</p>Your order number is " + OrderId.ToString());

            //remove shopping cart items
            List<ShoppingCart> shoppingCartItems = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId ==
            objOrderHeader.ApplicationUserId).ToList();
            _unitOfWork.ShoppingCart.Delete(shoppingCartItems);
            _unitOfWork.Commit();
        }
    }
}