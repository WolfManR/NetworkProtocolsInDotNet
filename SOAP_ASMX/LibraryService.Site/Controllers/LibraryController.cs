using LibraryService.Site.Models;

using Microsoft.AspNetCore.Mvc;

using System.Diagnostics;
using LibraryServiceReference;

namespace LibraryService.Site.Controllers
{
    public class LibraryController : Controller
    {
        private readonly ILogger<LibraryController> _logger;

        public LibraryController(ILogger<LibraryController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index(SearchType searchType, string? searchString)
        {
            using var client = new BooksWebCatalogSoapClient(BooksWebCatalogSoapClient.EndpointConfiguration.BooksWebCatalogSoap);
            try
            {
                if (searchString is not { Length: >= 3 }) return View(new BookCategoryViewModel());

                var books = searchType switch
                {
                    SearchType.Title => client.GetBooksByTitle(searchString),
                    SearchType.Category => client.GetBooksByCategory(searchString),
                    SearchType.Author => client.GetBooksByAuthor(searchString),
                    _ => Array.Empty<Book>()
                };

                return View(new BookCategoryViewModel { Books = books });
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error");
            }
            return View(new BookCategoryViewModel());
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}