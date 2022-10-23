using Microsoft.EntityFrameworkCore;

namespace ClinicService.Data.Storage;

public class BaseRepository<T,TId> : IRepository<T, TId> where T : class, IEntity<TId>
{
    protected readonly ClinicContext _context;

    protected BaseRepository(ClinicContext context)
    {
        _context = context;
    }

    public TId Add(T item)
    {
        _context.Set<T>().Add(item);
        _context.SaveChanges();
        return item.Id;
    }

    public virtual void Update(T item)
    {
        _context.Attach(item).State = EntityState.Modified;
        try
        {
            _context.SaveChanges();
        }
        catch (DbUpdateConcurrencyException e)
        {
            if (GetById(item.Id) is null) throw new KeyNotFoundException("Not found entity", e);
            throw;
        }
    }

    public void Delete(T item)
    {
        Delete(item.Id);
    }

    public void Delete(TId id)
    {
        if (GetById(id) is not { } entity) throw new KeyNotFoundException();
        _context.Remove(entity);
        _context.SaveChanges();
    }

    public IList<T> GetAll() => _context.Set<T>().ToList();

    public T? GetById(TId id) => _context.Set<T>().Find(id);
}