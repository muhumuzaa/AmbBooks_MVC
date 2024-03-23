using AmbBooksRazor.Data;
using AmbBooksRazor.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AmbBooksRazor.Pages.Categories
{
    [BindProperties]
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        public Category Category { get; set; }

        public DeleteModel(ApplicationDbContext db)
        {
            _db = db;
        }
        public void OnGet(int? id)
        {
            if(id !=null && id != 0)
            {
				Category = _db.Categories.Find(id);
            }
        }

        public IActionResult OnPost()
        {
			Category catFromDb = _db.Categories.Find(Category.Id);
			if (catFromDb == null)
			{
				return NotFound();
			}

			_db.Categories.Remove(catFromDb);
			_db.SaveChanges();
            TempData["success"] = "Category Deleted successfully";
            return RedirectToPage("Index");
		}
    }
}
