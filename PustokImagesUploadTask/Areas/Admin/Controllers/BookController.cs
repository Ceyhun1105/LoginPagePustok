using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PustokImagesUploadTask.Helpers;
using PustokImagesUploadTask.Models;
using PustokImagesUploadTask.ViewModels;

namespace PustokImagesUploadTask.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BookController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public BookController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public IActionResult Index()
        {
            return View(_context.Books.ToList());
        }
        public IActionResult Create()
        {
            ViewBag.Authors = _context.Authors.ToList();
            ViewBag.Categories = _context.Categories.ToList();
            return View();
        }
        [HttpPost]
        public IActionResult Create(Book book)
        {
            ViewBag.Authors = _context.Authors.ToList();
            ViewBag.Categories = _context.Categories.ToList();

            if (book.ImageFiles == null) ModelState.AddModelError("ImageFiles", "Required");
            if (book.PosterImage == null) ModelState.AddModelError("PosterImage", "Required");
            if (book.HoverImage == null) ModelState.AddModelError("HoverImage", "Required");

            if (!ModelState.IsValid) return View(book);

            foreach (IFormFile formFile in book.ImageFiles)
            {
                if (!formFile.CheckFileLength(1048576))
                {
                    ModelState.AddModelError("ImageFiles", "Please , upload less than 1Mb");
                    return View();
                }
                if (!formFile.CheckFileType())
                {
                    ModelState.AddModelError("ImageFiles", "Please , upload only image file (jpeg or png) ");
                    return View();
                }


                BookImage bookImage = new BookImage()
                {
                    Book = book,
                    Name = formFile.SaveFile(_env.WebRootPath, "uploads/books"),
                    IsPoster = null
                };

                _context.BookImages.Add(bookImage);
            }

            if (!book.PosterImage.CheckFileLength(1048576))
            {
                ModelState.AddModelError("PosterImage", "Please , upload less than 1Mb");
                return View();
            }
            if (!book.PosterImage.CheckFileType())
            {
                ModelState.AddModelError("PosterImage", "Please , upload only image file (jpeg or png) ");
                return View();
            }
            BookImage bookImage1 = new BookImage()
            {
                Book = book,
                Name = book.PosterImage.SaveFile(_env.WebRootPath, "uploads/books"),
                IsPoster = true
            };
            _context.BookImages.Add(bookImage1);

            if (!book.HoverImage.CheckFileLength(1048576))
            {
                ModelState.AddModelError("HoverImage", "Please , upload less than 1Mb");
                return View();
            }
            if (!book.HoverImage.CheckFileType())
            {
                ModelState.AddModelError("HoverImage", "Please , upload only image file (jpeg or png) ");
                return View();
            }
            BookImage bookImage2 = new BookImage()
            {
                Book = book,
                Name = book.HoverImage.SaveFile(_env.WebRootPath, "uploads/books"),
                IsPoster = false
            };
            _context.BookImages.Add(bookImage2);



            _context.Books.Add(book);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }


        public IActionResult Update(int id)
        {
            ViewBag.Authors = _context.Authors.ToList();
            ViewBag.Categories = _context.Categories.ToList();
            Book book = _context.Books.Include(x => x.BookImages).Include(x => x.Author).Include(x => x.Category).FirstOrDefault(x => x.Id == id);
            if (book is null) return View("Error");
            if (!ModelState.IsValid) return View(book);
            return View(book);
        }
        [HttpPost]
        public IActionResult Update(Book book)
        {

            Book exist1 = _context.Books.Include(x => x.BookImages).FirstOrDefault(x => x.Id == book.Id);
            if (exist1 is null) return View("Error");

            if (book.BookImageIds is not null)
            {
                List<BookImage> images = exist1.BookImages.Where(bimg => !book.BookImageIds.Contains(bimg.Id) & bimg.IsPoster == null).ToList();
                foreach (var item in images)
                {
                    if (System.IO.File.Exists(Path.Combine(_env.WebRootPath, "uploads/books", item.Name)))
                        System.IO.File.Delete(Path.Combine(_env.WebRootPath, "uploads/books", item.Name));
                }
                exist1.BookImages.RemoveAll(bimg => !book.BookImageIds.Contains(bimg.Id) & bimg.IsPoster == null);
            }
            else
            {
                List<BookImage> images = exist1.BookImages.Where(bimg => bimg.IsPoster == null).ToList();
                foreach (var item in images)
                {
                    if (System.IO.File.Exists(Path.Combine(_env.WebRootPath, "uploads/books", item.Name)))
                        System.IO.File.Delete(Path.Combine(_env.WebRootPath, "uploads/books", item.Name));
                }
                exist1.BookImages.RemoveAll(bimg => bimg.IsPoster == null);
            }

            if (book.BookHoverImgId is null)
            {
                BookImage? bookImage = exist1.BookImages.FirstOrDefault(x => x.IsPoster == false);
                if (bookImage != null)
                {
                    if (System.IO.File.Exists(Path.Combine(_env.WebRootPath, "uploads/books", bookImage.Name)))
                        System.IO.File.Delete(Path.Combine(_env.WebRootPath, "uploads/books", bookImage.Name));
                    exist1.BookImages.Remove(exist1.BookImages?.FirstOrDefault(x => x.IsPoster == false));
                }
            }
            if (book.BookPosterImgId is null)
            {
                BookImage? bookImage = exist1.BookImages.FirstOrDefault(x => x.IsPoster == true);
                if (bookImage != null)
                {
                    if (System.IO.File.Exists(Path.Combine(_env.WebRootPath, "uploads/books", bookImage.Name)))
                        System.IO.File.Delete(Path.Combine(_env.WebRootPath, "uploads/books", bookImage.Name));
                    exist1.BookImages.Remove(exist1.BookImages?.FirstOrDefault(x => x.IsPoster == true));
                }
            }



            if (book.ImageFiles is not null)
            {
                List<BookImage> oldbookimages = _context.BookImages.Where(x => x.BookId == book.Id).Where(x => x.IsPoster == null).ToList();

                foreach (IFormFile formFile in book.ImageFiles)
                {
                    if (!formFile.CheckFileLength(1024576)) ModelState.AddModelError("ImageFiles", "Please, upload less than 1 Mb");
                    if (!formFile.CheckFileType()) ModelState.AddModelError("ImageFiles", "Please, upload only image (jpeg,png)");

                    BookImage bookImage = new BookImage()
                    {
                        BookId = book.Id,
                        Name = formFile.SaveFile(_env.WebRootPath, "uploads/books"),
                        IsPoster = null
                    };
                    _context.BookImages.Add(bookImage);
                }
            }
            if (book.HoverImage is not null)
            {

                BookImage bookImage2 = new BookImage()
                {
                    Book = exist1,
                    Name = book.HoverImage.SaveFile(_env.WebRootPath, "uploads/books"),
                    IsPoster = false
                };
                _context.BookImages.Add(bookImage2);
            }

            if (book.PosterImage is not null)
            {

                BookImage bookImage2 = new BookImage()
                {
                    Book = exist1,
                    Name = book.PosterImage.SaveFile(_env.WebRootPath, "uploads/books"),
                    IsPoster = true
                };
                _context.BookImages.Add(bookImage2);
            }


            if (!ModelState.IsValid) return View();

            Book exist = _context.Books.FirstOrDefault(x => x.Id == book.Id);

            exist.Name = book.Name;
            exist.IsFeatured = book.IsFeatured;
            exist.IsNew = book.IsNew;
            exist.AuthorId = book.AuthorId;
            exist.CategoryId = book.CategoryId;
            exist.Description = book.Description;
            exist.DiscountPrice = book.DiscountPrice;
            exist.CostPrice = book.CostPrice;
            exist.SalePrice = book.SalePrice;
            exist.Code = book.Code;
            exist.IsAvalible = book.IsAvalible;

            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
        public IActionResult Delete(int id)
        {
            Book book = _context.Books.Find(id);
            if (book is null) return NotFound();

            foreach (BookImage image in _context.BookImages.Where(x => x.BookId == id))
            {
                if (System.IO.File.Exists(Path.Combine(_env.WebRootPath, "uploads/books", image.Name)))
                    System.IO.File.Delete(Path.Combine(_env.WebRootPath, "uploads/books", image.Name));
            }

            _context.Books.Remove(book);
            _context.SaveChanges();

            return Ok();

        }


        public IActionResult AddToBasket(int bookid)
        {
            List<BasketItemsViewModel> basketItems = new List<BasketItemsViewModel>();

            string existBasketItems = HttpContext.Request.Cookies["BasketItems"];

            Book checkbook = _context.Books.FirstOrDefault(x=>x.Id== bookid);

            if(checkbook == null) return NotFound();

            if (existBasketItems is not null)
            {
                basketItems = JsonConvert.DeserializeObject<List<BasketItemsViewModel>>(existBasketItems);
            }

            var existitem = basketItems.FirstOrDefault(x => x.BookId == bookid);

            if (existitem is not null)  existitem.Count += 1;
            else
            {
                existitem = new BasketItemsViewModel()
                {
                    BookId = bookid,
                    Count = 1
                };
                basketItems.Add(existitem);
            }

            string basketItemsStr = JsonConvert.SerializeObject(basketItems);

            HttpContext.Response.Cookies.Append("BasketItems", basketItemsStr);

            return Ok();
        }

        public IActionResult GetBasketItems()

        {

            List<BasketItemsViewModel> basketItems = new List<BasketItemsViewModel>();

            string basketItemsStr = HttpContext.Request.Cookies["BasketItems"];

            if (basketItemsStr is not null)
                basketItems = JsonConvert.DeserializeObject<List<BasketItemsViewModel>>(basketItemsStr);

            return Json(basketItems);

        }

    }
}
