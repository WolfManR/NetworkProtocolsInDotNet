using System.Collections.Generic;

namespace LibraryService.Data
{
    public interface IBooksCatalog : ICatalog<Book, string>
    {
        IList<Book> GetByTitle(string title);
        IList<Book> GetByAuthor(string authorName);
        IList<Book> GetByCategory(string category);
    }
}