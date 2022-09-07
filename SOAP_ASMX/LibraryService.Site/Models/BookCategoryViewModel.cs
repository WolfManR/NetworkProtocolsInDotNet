using LibraryServiceReference;

namespace LibraryService.Site.Models;

public class BookCategoryViewModel
{
    public Book[] Books { get; set; } = Array.Empty<Book>();
    public SearchType SearchType { get; set; }
    public string? SearchString { get; set; }
}