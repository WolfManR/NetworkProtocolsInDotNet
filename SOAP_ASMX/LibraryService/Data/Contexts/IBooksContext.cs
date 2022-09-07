using System.Collections.Generic;

namespace LibraryService.Data.Contexts
{
    public interface IBooksContext
    {
        IList<Book> Books { get; }
    }
}