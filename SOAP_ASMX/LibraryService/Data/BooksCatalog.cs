using System;
using System.Collections.Generic;

namespace LibraryService.Data
{
    public class BooksCatalog : IBooksCatalog
    {
        public string Add(Book item) => throw new NotImplementedException();

        public int Update(Book item) => throw new NotImplementedException();

        public int Delete(Book item) => throw new NotImplementedException();

        public IList<Book> GetAll() => null;

        public Book GetById(int id) => null;

        public IList<Book> GetByTitle(string title) => null;

        public IList<Book> GetByAuthor(string authorName) => null;

        public IList<Book> GetByCategory(string category) => null;
    }
}