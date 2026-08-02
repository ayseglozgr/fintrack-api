using FinTrack.Domain.Entities;
using System.Linq.Expressions;

namespace FinTrack.Application.Interfaces.Repositories;

public interface IGenericRepository<T> where T : BaseEntity
{
    Task<T?> GetByIdAsync(int id);
    Task<IEnumerable<T>> GetAllAsync();

    // Filtreleme yapabilmek için (Örn: Email adresine göre kullanıcı aramak için)
    Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);

    Task AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(T entity); // Soft-delete'i Infrastructure katmanında handle edeceğiz

    // Veritabanı kayıt işlemini doğrudan repository üzerinden asenkron tetiklemek için
    Task<int> SaveChangesAsync();
}