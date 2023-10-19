using BulkyBook.DataAccess.Data;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models.Models;
using BulkyBook.Models.ViewModels;
using BulkyBook.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BulkyBookWeb.Areas.Admin.Controllers {
    [Area("Admin")]
	//[Authorize(Roles = SD.Role_Admin)]
	public class CompanyController : Controller
    {

        private readonly ICompanyRepository _companyRepo;
        private readonly ICategoryRepository _categoryRepo;

		public CompanyController(ICompanyRepository db,
                                 ICategoryRepository categoryDb
                                 )
        {
            _companyRepo = db;
            _categoryRepo = categoryDb;
		}
        public IActionResult Index()
        {
            List<Company> objCompanyList = _companyRepo.GetAll().ToList();
            //IEnumerable<SelectListItem> CategoryList = _categoryRepo.GetAll().ToList();
            return View(objCompanyList);
        }

        public IActionResult Create()
        {
            //IEnumerable<SelectListItem> CategoryList = new SelectListItem[1]= {

            //}
            return View();
        }
        [HttpPost]
        public IActionResult Create(Company obj)
        {
            
            //if (obj.Name != null && obj.Name.ToLower() == "test") {
            //	ModelState.AddModelError("", "Test is an invalid value");
            //}

            if (ModelState.IsValid)
            {

                _companyRepo.Add(obj);
                _companyRepo.Save();
                TempData["success"] = "Company created successfully";
                return RedirectToAction("Index", "Company");
            }
            return View();
        }
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Company? companyFromDb = _companyRepo.Get(u => u.Id == id);
            //Company? companyFromDb1 = _companyRepo.Categories.FirstOrDefault(x => x.Id == id);
            //Company? companyFromD2b = _companyRepo.Categories.Where(x => x.Id == id).FirstOrDefault();
            if (companyFromDb == null)
            {
                return NotFound();
            }
            return View(companyFromDb);
        }
        [HttpPost]
        public IActionResult Edit(Company obj)
        {
            if (ModelState.IsValid)
            {
                _companyRepo.Update(obj);
                _companyRepo.Save();
                TempData["success"] = "Company updated successfully";
                return RedirectToAction("Index", "Company");
            }
            return View();
        }
        //public IActionResult Delete(int? id)
        //{
        //    if (id == null || id == 0)
        //    {
        //        return NotFound();
        //    }
        //    Company? companyFromDb = _companyRepo.Get(u => u.Id == id);
        //    //Company? companyFromDb1 = _companyRepo.Categories.FirstOrDefault(x => x.Id == id);
        //    //Company? companyFromD2b = _companyRepo.Categories.Where(x => x.Id == id).FirstOrDefault();
        //    if (companyFromDb == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(companyFromDb);
        //}


        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int? id)
        {
            Company? obj = _companyRepo.Get(u => u.Id == id);
            if (obj == null)
            {
                return NotFound();
            }
            _companyRepo.Remove(obj);
            _companyRepo.Save();
            TempData["success"] = "Company deleted successfully";
            return RedirectToAction("Index");
        }

        public IActionResult Testing()
        {
            return View();
        }
		#region API CALLS

		[HttpGet]
		public IActionResult GetAll() {
			List<Company> objCompanyList = _companyRepo.GetAll().ToList();

			return Json(new { data = objCompanyList });
		}

        [HttpDelete]
		public IActionResult Delete(int? id) {
            var companyToBeDeleted = _companyRepo.Get(u=> u.Id ==id);
            if(companyToBeDeleted == null) {
                return Json(new { success = false, message = "Error while deleting" }); 
            }

            _companyRepo.Remove(companyToBeDeleted);
            _companyRepo.Save();


			return Json(new {  success = true, message = "Delete Successful" });
		}


		#endregion
	}
}
