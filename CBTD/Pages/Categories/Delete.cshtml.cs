using DataAccess.Data;
using DataAccess.Models;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CBTD.Pages.Categories
{
    public class DeleteModel : PageModel
    {

        private readonly UnitOfWork _unitOfWork;
        [BindProperty]  //synchonizes form fields with values in code behind
        public Category objCategory { get; set; }


        public DeleteModel(UnitOfWork unitOfWork)  //dependency injection
        {
            _unitOfWork = unitOfWork;
            objCategory = new Category();
        }

        public IActionResult OnGet(int? id)
        {
           
            
            if (id != 0)
            {
                objCategory = _unitOfWork.Category.GetById(id);
            }

            if (objCategory == null)
            {
                return NotFound();
            }

            
            return Page();
        }

        public IActionResult OnPost()
        {
         
                _unitOfWork.Category.Delete(objCategory);
                TempData["success"] = "Category deleted Successfully";
            
                _unitOfWork.Commit();

            return RedirectToPage("./Index");
        }
    }
}
