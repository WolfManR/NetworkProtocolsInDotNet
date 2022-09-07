using System;
using System.Collections.Generic;
using System.Linq;

using LibraryService.Data.Contexts;

namespace LibraryService.Data
{
    public class BooksCatalog : IBooksCatalog
    {
        private readonly IBooksContext _context;

        public BooksCatalog(IBooksContext context)
        {
            _context = context;
        }

        public string Add(Book item) => throw new NotImplementedException();

        public int Update(Book item) => throw new NotImplementedException();

        public int Delete(Book item) => throw new NotImplementedException();

        public IList<Book> GetAll() => throw new NotImplementedException();

        public Book GetById(int id) => throw new NotImplementedException();

        public IList<Book> GetByTitle(string title)
        {
            try
            {
                return _context.Books
                    .Where(b =>
                        b.Title.ToLower().Contains(title.ToLower()))
                    .ToList();
            }
            catch (Exception e)
            {
                return new List<Book>();
            }
        }

        public IList<Book> GetByAuthor(string authorName)
        {
            try
            {
                return _context.Books
                    .Where(b => 
                        b.Authors.Count(a => a.Name.ToLower().Contains(authorName.ToLower())) > 0)
                    .ToList();
            }
            catch (Exception e)
            {
                return new List<Book>();
            }
        }

        public IList<Book> GetByCategory(string category)
        {
            try
            {
                return _context.Books
                    .Where(b =>
                        b.Category.ToLower().Contains(category.ToLower()))
                    .ToList();
            }
            catch (Exception e)
            {
                return new List<Book>();
            }
        }
    }
}