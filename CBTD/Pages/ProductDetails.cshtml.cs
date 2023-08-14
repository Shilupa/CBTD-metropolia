using System;
using System.Collections.Generic;
using System.Linq;
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

        public ProductDetailsModel(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult OnGet(int? productId)
        {
            objProduct = _unitOfWork.Product.Get(p => p.Id == productId, includes: "Category,Manufacturer");
            return Page();
        }
    }

}
