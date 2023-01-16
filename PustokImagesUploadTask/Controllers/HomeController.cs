using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PustokImagesUploadTask.Models;
using PustokImagesUploadTask.ViewModels;

namespace PustokImagesUploadTask.Controllers
{
	public class HomeController : Controller
	{
		private readonly AppDbContext _context;

		public HomeController(AppDbContext context)
		{
			_context = context;
		}

		public IActionResult Index()
		{
			HomeViewModel homeViewModel = new HomeViewModel()
			{
				FeaturedBooks = _context.Books.Where(x => x.IsFeatured == true).Include(x => x.Author).Include(x => x.Category).Include(x => x.BookImages).ToList(),
				NewBooks = _context.Books.Where(x => x.IsNew == true).Include(x => x.Author).Include(x => x.Category).Include(x => x.BookImages).ToList(),
				DiscountedBooks = _context.Books.Where(x => x.DiscountPrice > 0).Include(x => x.Author).Include(x => x.Category).Include(x => x.BookImages).ToList(),
				AllBooks = _context.Books.Include(x => x.Author).Include(x => x.Category).Include(x => x.BookImages).ToList(),
				Sliders = _context.Sliders.ToList(),
				Features = _context.Features.ToList()
			};
			return View(homeViewModel);
		}
		public IActionResult Detail(int id)
		{
			Book book = _context.Books.Include(x => x.Author).Include(x => x.Category).Include(x => x.BookImages).FirstOrDefault(x => x.Id == id);
			return View(book);
		}

		public IActionResult Checkout()
		{
			List<BasketItemsViewModel> basketItemsList = new List<BasketItemsViewModel>();
			List<CheckOutViewModel> existitems = new List<CheckOutViewModel>();
			string basketitems = HttpContext.Request.Cookies["BasketItems"];
			if (basketitems is not null)
			{
				basketItemsList = JsonConvert.DeserializeObject<List<BasketItemsViewModel>>(basketitems);
				foreach (var item in basketItemsList)
				{
					CheckOutViewModel checkoutitem = new CheckOutViewModel()
					{
						Book = _context.Books.FirstOrDefault(x => x.Id == item.BookId),
						Count = item.Count,
					};
					existitems.Add(checkoutitem);
				}
			}

			return View(existitems);

		}
	}
}