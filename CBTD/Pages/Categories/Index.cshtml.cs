using DataAccess.Models;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CBTD.Pages.Categories
{
    public class IndexModel : PageModel
    {
        private readonly UnitOfWork _unitOfWork;
        public IEnumerable<Category> objCategoryList;

        public IndexModel(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            objCategoryList = new List<Category>();
        }

        public IActionResult OnGet()
        {
            //There are five major sets of IActionResult Types the can be returned
            //1. Server Status Code Results
            //2. Server Status Code and Object Results
            //3. Redirect (to another webpage) Results
            //4. File Results
            //5. Content Results (like another Razor Web Page)
            objCategoryList = _unitOfWork.Category.GetAll();
            return Page();    //render Page

        }
    }
}
