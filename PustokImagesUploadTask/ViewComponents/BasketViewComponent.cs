using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PustokImagesUploadTask.Models;
using PustokImagesUploadTask.ViewModels;

namespace PustokImagesUploadTask.ViewComponents
{
	public class BasketViewComponent : ViewComponent
	{
		private readonly AppDbContext _context;

		public BasketViewComponent(AppDbContext context)
		{
			_context = context;
		}
		public IViewComponentResult Invoke()
		{
			List<CheckOutViewModel> checkOutItems = new List<CheckOutViewModel>();
			List<BasketItemsViewModel> basketItems = new List<BasketItemsViewModel>();

			string existitems = HttpContext.Request.Cookies["BasketItems"];

			if(existitems is not null)
			{
				basketItems = JsonConvert.DeserializeObject<List<BasketItemsViewModel>>(existitems);
				foreach (var item in basketItems)
				{
					CheckOutViewModel basketItem = new CheckOutViewModel()
					{
						Book = _context.Books.Include(x=>x.BookImages).FirstOrDefault(x=>x.Id == item.BookId),
						Count= item.Count
					};
					checkOutItems.Add(basketItem);
				}
			}

			return View(checkOutItems);
		}
	}
}
