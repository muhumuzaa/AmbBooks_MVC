using Amb.DataAccess.Data;
using Amb.DataAccess.Repository.IRepository;
using Amb.Models;
using Microsoft.AspNetCore.Mvc;

namespace AmbBooks.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            List<Category> objCategoryList = _unitOfWork.Category.GetAll().ToList();
            return View(objCategoryList);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Category obj)
        {
            if (obj.Name == obj.DisplayOrder.ToString())
            {
                ModelState.AddModelError("name", "Display order cannot be the same as name");
            }


            if (ModelState.IsValid)
            {
                _unitOfWork.Category.Add(obj); //add the category object to the category table.
                _unitOfWork.Save(); //go to database and add these changes.
                TempData["Success"] = "Category created successfully";
                return RedirectToAction("Index");
            }
            return View();

        }

        public IActionResult Edit(int? id)
        {
            //check if id of category is valid
            if (id == null || id == 0)
            {
                return NotFound();
            }

            //if id is valid, get that category from the database. using the _db.Categories
            Category CategoryFromDb = _unitOfWork.Category.Get(u => u.Id == id);

            if (CategoryFromDb == null)
            {
                return NotFound();
            }

            return View(CategoryFromDb);
        }

        [HttpPost]
        public IActionResult Edit(Category obj)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Category.Update(obj);
                _unitOfWork.Save();
                TempData["Success"] = "Category Updated successfully";
                return RedirectToAction("Index");
            }

            return View();
        }

        public IActionResult Delete(int? id)
        {
            //check if Id exists
            if (id == null || id == 0)
            {
                return NotFound();
            }

            //else return the actual object
            Category? categoryFromDb = _unitOfWork.Category.Get(u => u.Id == id);
            if (categoryFromDb == null)
            {
                return NotFound();
            }
            return View(categoryFromDb);

        }
        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int? id)
        {
            Category? catFromDb = _unitOfWork.Category.Get(u => u.Id == id);
            if (catFromDb == null)
            {
                return NotFound();
            }

            _unitOfWork.Category.Remove(catFromDb);
            _unitOfWork.Save();
            TempData["Success"] = "Category Deleted Successfully";
            return RedirectToAction("Index");



        }
    }
}
