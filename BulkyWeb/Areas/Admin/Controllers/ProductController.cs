using BulkyBook.DataAccess.Data;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models.Models;
using BulkyBook.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BulkyBookWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {

        private readonly IProductRepository _productRepo;
        private readonly ICategoryRepository _categoryRepo;
		private readonly IWebHostEnvironment _webHostEnvironment;

		public ProductController(IProductRepository db,
                                 IWebHostEnvironment webHostEnvironment,
                                 ICategoryRepository categoryDb
                                 )
        {
            _productRepo = db;
            _webHostEnvironment = webHostEnvironment;
            _categoryRepo = categoryDb;
		}
        public IActionResult Index()
        {
            List<Product> objProductList = _productRepo.GetAll().ToList();
            //IEnumerable<SelectListItem> CategoryList = _categoryRepo.GetAll().ToList();
            return View(objProductList);
        }

        public IActionResult Create()
        {
            //IEnumerable<SelectListItem> CategoryList = new SelectListItem[1]= {

            //}
            return View();
        }
        [HttpPost]
        public IActionResult Create(Product obj, IFormFile? file)
        {
            
            //if (obj.Name != null && obj.Name.ToLower() == "test") {
            //	ModelState.AddModelError("", "Test is an invalid value");
            //}

            if (ModelState.IsValid)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                if(file != null) {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string productPath = Path.Combine(wwwRootPath, @"images\product");



                    using(var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create)) {
                        file.CopyTo(fileStream);
                    }

                    obj.ImageUrl = @"\images\"+ fileName;

				}

                _productRepo.Add(obj);
                _productRepo.Save();
                TempData["success"] = "Product created successfully";
                return RedirectToAction("Index", "Product");
            }
            return View();
        }
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Product? productFromDb = _productRepo.Get(u => u.Id == id);
            //Product? productFromDb1 = _productRepo.Categories.FirstOrDefault(x => x.Id == id);
            //Product? productFromD2b = _productRepo.Categories.Where(x => x.Id == id).FirstOrDefault();
            if (productFromDb == null)
            {
                return NotFound();
            }
            return View(productFromDb);
        }
        [HttpPost]
        public IActionResult Edit(Product obj)
        {
            if (ModelState.IsValid)
            {
                _productRepo.Update(obj);
                _productRepo.Save();
                TempData["success"] = "Product updated successfully";
                return RedirectToAction("Index", "Product");
            }
            return View();
        }
        //public IActionResult Delete(int? id)
        //{
        //    if (id == null || id == 0)
        //    {
        //        return NotFound();
        //    }
        //    Product? productFromDb = _productRepo.Get(u => u.Id == id);
        //    //Product? productFromDb1 = _productRepo.Categories.FirstOrDefault(x => x.Id == id);
        //    //Product? productFromD2b = _productRepo.Categories.Where(x => x.Id == id).FirstOrDefault();
        //    if (productFromDb == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(productFromDb);
        //}


        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int? id)
        {
            Product? obj = _productRepo.Get(u => u.Id == id);
            if (obj == null)
            {
                return NotFound();
            }
            _productRepo.Remove(obj);
            _productRepo.Save();
            TempData["success"] = "Product deleted successfully";
            return RedirectToAction("Index");
        }

        public IActionResult Testing()
        {
            return View();
        }
		#region API CALLS

		[HttpGet]
		public IActionResult GetAll() {
			List<Product> objProductList = _productRepo.GetAll().ToList();

			return Json(new { data = objProductList });
		}

        [HttpDelete]
		public IActionResult Delete(int? id) {
            var productToBeDeleted = _productRepo.Get(u=> u.Id ==id);
            if(productToBeDeleted == null) {
                return Json(new { success = false, message = "Error while deleting" }); 
            }

            var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, _productRepo.Get(x => x.Id == id).
                ImageUrl.TrimStart('\\'));

            if(System.IO.File.Exists(oldImagePath)) {
                System.IO.File.Delete(oldImagePath);
            }

            _productRepo.Remove(productToBeDeleted);
            _productRepo.Save();


			return Json(new {  success = true, message = "Delete Successful" });
		}


		#endregion
	}
}
