using System.Linq;
using System.Web.Services;
using LibraryService.Data;
using LibraryService.Data.Contexts;

namespace LibraryService
{
    /// <summary>
    /// Summary description for BooksWebCatalog
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class BooksWebCatalog : WebService
    {
        private readonly IBooksCatalog _catalog;

        public BooksWebCatalog()
        {
            _catalog = new BooksCatalog(new BooksContext());
        }

        [WebMethod]
        public Book[] GetBooksByTitle(string title)
        {
            return _catalog.GetByTitle(title).ToArray();
        }

        [WebMethod]
        public Book[] GetBooksByAuthor(string authorName)
        {
            return _catalog.GetByAuthor(authorName).ToArray();
        }

        [WebMethod]
        public Book[] GetBooksByCategory(string category)
        {
            return _catalog.GetByCategory(category).ToArray();
        }
    }
}
