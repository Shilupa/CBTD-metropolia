using DataAccess.Data;
using DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CBTD.Pages.Manufacturers
{
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        [BindProperty]  //synchonizes form fields with values in code behind
        public Manufacturer objManufacturer { get; set; }


        public DeleteModel(ApplicationDbContext db)  //dependency injection
        {
            _db = db;
            objManufacturer = new Manufacturer();
        }

        public IActionResult OnGet(int? id)
        {
           
            
            if (id != 0)
            {
                objManufacturer = _db.Manufacturers.Find(id);
            }

            if (objManufacturer == null)
            {
                return NotFound();
            }

            
            return Page();
        }

        public IActionResult OnPost()
        {
         
                _db.Manufacturers.Remove(objManufacturer);
                TempData["success"] = "Manufacturer deleted Successfully";
            
                _db.SaveChanges();

            return RedirectToPage("./Index");
        }


    }
}
