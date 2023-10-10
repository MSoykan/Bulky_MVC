using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace BulkyBookWeb.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ICategoryRepository _categoryRepo;
        private readonly IProductRepository _productRepo;

        public HomeController(ILogger<HomeController> logger,
                              ICategoryRepository categoryRepo,
                              IProductRepository productRepo
                              )
        {
            _logger = logger;
			_categoryRepo= categoryRepo;
            _productRepo= productRepo;

		}



        public IActionResult Index()
        {
            IEnumerable<Product> productList = _productRepo.GetAll();
            return View(productList);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}