using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PustokImagesUploadTask.Models;
using PustokImagesUploadTask.ViewModels;

namespace PustokImagesUploadTask.ViewComponents
{
    public class PriceViewComponent : ViewComponent
    {
        private readonly AppDbContext _context;

        public PriceViewComponent(AppDbContext context)
        {
            _context = context;
        }
        public IViewComponentResult Invoke()
        {
            double price = 0;
            int count = 0;
            List<CheckOutViewModel> checkOutItems = new List<CheckOutViewModel>();
            List<BasketItemsViewModel> basketItems = new List<BasketItemsViewModel>();

            string existitems = HttpContext.Request.Cookies["BasketItems"];

            if (existitems is not null)
            {
                basketItems = JsonConvert.DeserializeObject<List<BasketItemsViewModel>>(existitems);
                foreach (var item in basketItems)
                {
                    CheckOutViewModel basketItem = new CheckOutViewModel()
                    {
                        Book = _context.Books.Include(x => x.BookImages).FirstOrDefault(x => x.Id == item.BookId),
                        Count = item.Count
                    };
                    price += (basketItem.Count * (basketItem.Book.SalePrice * (1 - basketItem.Book.DiscountPrice / 100)));
                    count += item.Count;
                }
            }
            PriceViewModel priceViewModel = new PriceViewModel()
            {
                Price = price,
                Count= count
            };
            return View(priceViewModel);
        }
    }
}
