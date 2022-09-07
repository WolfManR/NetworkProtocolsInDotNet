using System.Collections.Generic;

namespace LibraryService.Data.Contexts
{
    public class BooksContext : IBooksContext
    {
        private IList<Book> _data;

        public BooksContext()
        {
            Initialize();
        }

        public IList<Book> Books => _data;

        private void Initialize()
        {

        }
    }
}