using System.Collections.Generic;

namespace LibraryService.Data
{
    public interface ICatalog<T, out TId>
    {
        TId Add(T item);
        int Update(T item);
        int Delete(T item);

        IList<T> GetAll();
        T GetById(int id);
    }
}