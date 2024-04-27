using Amb.DataAccess.Data;
using Amb.DataAccess.Repository.IRepository;
using Amb.Models;
using Amb.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace AmbBooks.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;

		}
        public IActionResult Index()
        {
            //get a list of all products from the database
            List<Product> objProductList = _unitOfWork.Product.GetAll(includeProperties:"Category").ToList();

            

            return View(objProductList);
        }

        public IActionResult Upsert(int? id)
        {
            ProductVM productVM = new()
            {
                CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem //we return a new object of type SelectListItem: 
				{
					//from each selectListItem/Category object retrieved, we get the name & Id
					Text = u.Name,
					Value = u.Id.ToString()
				}),
                Product = new Product()
            };
            if(id==null || id == 0) //means its a create 
            {
				return View(productVM);
            }
            else
            {
                //update a product
                productVM.Product = _unitOfWork.Product.Get(u => u.Id == id);
                return View(productVM);
            }
            
        }
        [HttpPost]
        public IActionResult Upsert(ProductVM productVM, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                if(file != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string productPath = Path.Combine(wwwRootPath, @"images\product");

                    if (!string.IsNullOrEmpty(productVM.Product.ImageUrl))
                    {
                        //delete the old image
                        var oldImagePath = Path.Combine(wwwRootPath, productVM.Product.ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    using (var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    productVM.Product.ImageUrl = @"\images\product\" + fileName;
                }

                if(productVM.Product.Id == 0)
                {
					_unitOfWork.Product.Add(productVM.Product); //add the category object to the category table.
				}
                else
                {
					_unitOfWork.Product.Update(productVM.Product); //add the category object to the category table.
				}
                
                _unitOfWork.Save(); //go to database and add these changes.
                TempData["Success"] = "Category created successfully";
                return RedirectToAction("Index");
            }
            else
            {

				productVM.CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem //we return a new object of type SelectListItem: 
                {
                    //from each selectListItem/Category object retrieved, we get the name & Id
                    Text = u.Name,
                    Value = u.Id.ToString()
                });
					
				

			
				return View(productVM);

			}
            

        }
    
        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            List<Product> objProductList = _unitOfWork.Product.GetAll(includeProperties: "Category").ToList();
            return Json(new { data = objProductList });
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var productToBeDeleted = _unitOfWork.Product.Get(u=>u.Id == id);
            if(productToBeDeleted == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }
            //delete image from local files
            var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, productToBeDeleted.ImageUrl.TrimStart('\\'));
            if (System.IO.File.Exists(oldImagePath))
            {
                System.IO.File.Delete(oldImagePath);
            }
            _unitOfWork.Product.Remove(productToBeDeleted);
            _unitOfWork.Save();

            return Json(new { success = true, message = "Delete Successful" });
        }
        #endregion
    }
}
