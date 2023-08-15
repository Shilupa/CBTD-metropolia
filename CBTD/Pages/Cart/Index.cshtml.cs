using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CBTD.ViewModel;
using Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CBTD.Pages.Cart
{
    [Authorize]

    public class IndexModel : PageModel
    {

        private readonly UnitOfWork _unitOfWork;
        [BindProperty]
        public ShoppingCartVM ShoppingCartVM { get; set; }
        public IndexModel(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult OnGet()
        {
            var claimsIdentity = User.Identity as ClaimsIdentity;

            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            ShoppingCartVM = new ShoppingCartVM()
            {
                cartItems = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == claim.Value, u => u.ProductId, "Product"),

            };

            foreach (var item in ShoppingCartVM.cartItems)
            {
                item.CartPrice = GetPriceBasedOnQuantity(item.Count, item.Product.UnitPrice,
                    item.Product.HalfDozenPrice, item.Product.DozenPrice);
                ShoppingCartVM.CartTotal += (item.CartPrice * item.Count);

            }


            return Page();
        }

        private double GetPriceBasedOnQuantity(double quantity, double unitPrice, double priceHalfDozen, double priceDozen)
        {
            if (quantity <= 5)
            {
                return unitPrice;
            }
            else
            {
                if (quantity <= 11)
                {
                    return priceHalfDozen;
                }
                return priceDozen;
            }
        }

        public IActionResult OnPostMinus(int cartId)
        {
            var cart = _unitOfWork.ShoppingCart.Get(c => c.Id == cartId);
            if (cart.Count == 1)
            {
                _unitOfWork.ShoppingCart.Delete(cart);
            }


            else
            {
                cart.Count -= 1;


                _unitOfWork.ShoppingCart.Update(cart);
            }
            _unitOfWork.Commit();


            var cnt = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == cart.ApplicationUserId).Count();
            //HttpContext.Session.SetInt32(SD.ShoppingCart, cnt);
            return RedirectToPage();




        }


        public IActionResult OnPostPlus(int cartId)
        {
            var cart = _unitOfWork.ShoppingCart.Get(c => c.Id == cartId);
            cart.Count += 1;


            _unitOfWork.ShoppingCart.Update(cart);




            _unitOfWork.Commit();




            return RedirectToPage();




        }


        public IActionResult OnPostRemove(int cartId)
        {
            var cart = _unitOfWork.ShoppingCart.Get(c => c.Id == cartId);
            _unitOfWork.ShoppingCart.Delete(cart);
            _unitOfWork.Commit();
            var cnt = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == cart.ApplicationUserId).Count();
            //HttpContext.Session.SetInt32(SD.ShoppingCart, cnt);
            return RedirectToPage();


        }



    }
}
