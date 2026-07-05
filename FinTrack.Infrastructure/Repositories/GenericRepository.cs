using FinTrack.Application.Interfaces.Repositories;
using FinTrack.Domain.Entities;
using FinTrack.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace FinTrack.Infrastructure.Repositories;

public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
{
    protected readonly FinTrackDbContext _context;
    protected readonly DbSet<T> _dbSet;

    public GenericRepository(FinTrackDbContext context)
    {
        _context = context;
        _dbSet = _context.Set<T>();
    }

    public async Task<T?> GetByIdAsync(int id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
    {
        return await _dbSet.Where(predicate).ToListAsync();
    }

    public async Task AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
    }

    public void Update(T entity)
    {
        _dbSet.Update(entity);
    }

    public void Delete(T entity)
    {
        // Soft Delete: Veriyi fiziksel olarak silmiyor, sadece işaretliyoruz
        entity.IsDeleted = true;
        Update(entity);
    }

    // Son yaptığımız konuşmaya istinaden eklediğimiz asenkron kaydetme metodu:
    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }
}