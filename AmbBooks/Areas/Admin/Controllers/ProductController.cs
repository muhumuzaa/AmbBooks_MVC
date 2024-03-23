using Amb.DataAccess.Repository.IRepository;
using Amb.Models;
using Microsoft.AspNetCore.Mvc;

namespace AmbBooks.Areas.Admin.Controllers
{
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

        }
        public IActionResult Index()
        {
            List<Product> objProducctList = _unitOfWork.Product.GetAll().ToList();
            return View(objProducctList);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Product obj)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Product.Add(obj);
                _unitOfWork.Save();
                TempData["Success"] = "Product Added Successfully";
                return RedirectToAction("Index");
            }
            return View();
        }

        public IActionResult Edit(int? id) 
        { 
            //check if Id is valid
            if(id == null || id == 0)
            {
                return NotFound();
            }
            //if Id is valid, fetch the product from the database
            Product prodFromDb = _unitOfWork.Product.Get(u => u.Id == id);
            if(prodFromDb == null)
            {
                return NotFound();
            }
            return View(prodFromDb);
        }
        [HttpPost]
        public IActionResult Edit(Product obj) 
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Product.Update(obj);
                _unitOfWork.Save();
                TempData["Success"] = "Product updated successfully";
                return RedirectToAction("Index");
            }
            return View();
        }

        public IActionResult Delete(int? id) 
        {
            if(id == null || id == 0)
            {
                return NotFound();
            }
            //else. Create an object of the product and fetch it from the database
            Product? prodFromDb = _unitOfWork.Product.Get(u => u.Id == id);

            //check returned object isnt empty
            if(prodFromDb == null)
            {
                return NotFound();
            }
            return View(prodFromDb);
        }
        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int? id) 
        {
            Product? prodFromDb = _unitOfWork.Product.Get(u => u.Id == id);
            if (prodFromDb == null)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                _unitOfWork.Product.Remove(prodFromDb);
                _unitOfWork.Save();
                TempData["Success"] = "Product Removed Successfully";
                return RedirectToAction("Index");
            }
            return View();
        }
    }
}
