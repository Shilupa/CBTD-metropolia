using DataAccess.Data;
using DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CBTD.Pages.Manufacturers
{
    public class UpsertModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        [BindProperty]  //synchonizes form fields with values in code behind
        public Manufacturer objManufacturer { get; set; }

        public UpsertModel(ApplicationDbContext db)  //dependency injection
        {
            _db = db;
			objManufacturer = new Manufacturer();
        }

        public IActionResult OnGet(int? id)
        {
           
            //edit mode
            if (id != 0)
            {
				objManufacturer = _db.Manufacturers.Find(id);
            }

            if (objManufacturer == null)
            {
                return NotFound();
            }

            //create new mode
            return Page();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

			//if this is a new Manufacturer
			if (objManufacturer.Id == 0)
            {
                _db.Manufacturers.Add(objManufacturer);
                TempData["success"] = "Manufacturer added Successfully";
            }
            //if category exists
            else
            {
                _db.Manufacturers.Update(objManufacturer);
                TempData["success"] = "Manufacturer updated Successfully";
            }
            _db.SaveChanges();

            return RedirectToPage("./Index");
        }


    }
}
