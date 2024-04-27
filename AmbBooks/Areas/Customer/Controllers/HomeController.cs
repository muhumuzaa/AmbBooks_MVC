using Amb.DataAccess.Repository.IRepository;
using Amb.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace AmbBooks.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        //unit of work. Gets products from the database
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        //to display all products on the homepage
        public IActionResult Index()
        {
            IEnumerable<Product> productList = _unitOfWork.Product.GetAll(includeProperties: "Category");
            return View(productList);
        }

        //To go to a single product detail page
		public IActionResult Details(int productId)
		{
			Product product = _unitOfWork.Product.Get(u=>u.Id == productId, includeProperties: "Category");
			return View(product);
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
