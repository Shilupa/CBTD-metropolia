using DataAccess.Models;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CBTD.Pages.Products
{
	public class IndexModel : PageModel
    {
        private readonly UnitOfWork _unitOfWork;
        public IEnumerable<Product> objProductList;

        public IndexModel(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            objProductList = new List<Product>();
        }

        public IActionResult OnGet()
        {
            //There are five major sets of IActionResult Types the can be returned
            //1. Server Status Code Results
            //2. Server Status Code and Object Results
            //3. Redirect (to another webpage) Results
            //4. File Results
            //5. Content Results (like another Razor Web Page)
            objProductList = _unitOfWork.Product.GetAll();
            return Page();    //render Page

        }
    }
}
