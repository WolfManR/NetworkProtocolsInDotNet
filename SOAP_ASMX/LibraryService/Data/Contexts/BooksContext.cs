using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

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
            _data = JsonConvert.DeserializeObject<List<Book>>(Encoding.UTF8.GetString(Properties.Resources.books));
        }
    }
}