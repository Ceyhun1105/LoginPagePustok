using PustokImagesUploadTask.Models;

namespace PustokImagesUploadTask.ViewModels
{
    public class BookViewModel
    {
        public List<Book> Books { get; set; }
        public List<Author> Authors { get; set; }
        public List<Category> Categories { get; set; }
        public Book Book { get; set; }
    }
}
