using AbbyWeb.Data;
using AbbyWeb.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AbbyWeb.Pages.Categories;

[BindProperties]

    public class DeleteModel : PageModel
    {
		private readonly ApplicationDbContext _db;
		
		public Category Category { get; set; }

		public DeleteModel(ApplicationDbContext db)
		{
			_db = db;
		}

		
        public void OnGet(int id)
        {
			Category = _db.Category.Find(id);
				//Other ways to do it:
				//Category = _db.Category.FirstOrDefault(i => i.Id == id);
				//Category = _db.Category.SingleOrDefault(i => i.Id == id);
				//Category = _db.Category.Where(i => i.Id == id).FirstOrDefault();
        }

		public async Task<IActionResult> OnPost()
		{	
				var categoryFromDb = _db.Category.Find(Category.Id);

				if (categoryFromDb != null)
				{
					_db.Category.Remove(categoryFromDb);
					await _db.SaveChangesAsync();
					TempData["success"] = "Category deleted successfully";
					return RedirectToPage("Index");
				}
				return Page();
		}
    }

