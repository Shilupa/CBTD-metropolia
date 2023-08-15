using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using DataAccess.Models;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CBTD.Pages
{
    public class ProductDetailsModel : PageModel
    {
        private readonly UnitOfWork _unitOfWork;
        [BindProperty]
        public int txtCount { get; set; }

        public Product objProduct { get; set; }

        public ShoppingCart objCart { get; set; }

        public ProductDetailsModel(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult OnGet(int? productId)
        {
            //check to see if user logged on
            var claimsIdentity = User.Identity as ClaimsIdentity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            TempData["UserLoggedIn"] = claim;
            objProduct = _unitOfWork.Product.Get(p => p.Id == productId, includes: "Category,Manufacturer");
            return Page();
        }

        public IActionResult OnPost(Product objProduct)
        {

            //check to see if we have a shopping cart and this item already for the user

            var claimsIdentity = User.Identity as ClaimsIdentity;

            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            //does this user have shopping cart with that product already
            ShoppingCart cartFromDb = _unitOfWork.ShoppingCart.Get(
            u => u.ApplicationUserId == claim.Value && u.ProductId == objProduct.Id);


            if (cartFromDb == null)
            {
                objCart = new ShoppingCart();
                objCart.ApplicationUserId = claim.Value;
                objCart.ProductId = objProduct.Id;
                objCart.Count = txtCount;
                _unitOfWork.ShoppingCart.Add(objCart);
                _unitOfWork.Commit();

            }

            else
            {

                _unitOfWork.ShoppingCart.IncrementCount(cartFromDb, txtCount);
                _unitOfWork.ShoppingCart.Update(cartFromDb);
                _unitOfWork.Commit();
            }
            return RedirectToPage("Index");
        }


    }

}

