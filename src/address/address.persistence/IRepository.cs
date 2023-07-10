using System.Linq.Expressions;
using address.domain;
using Microsoft.EntityFrameworkCore;
using Specification.Net;

namespace address.persistence;

public interface IReadOnlyRepository<TEntity>
{
    Task<IEnumerable<TEntity>> GetAll(Specification<TEntity>? spec = null);
    Task<TEntity?> Get(Specification<TEntity> spec);
    Task<bool> Any(Specification<TEntity> spec);
    Task<TOut?> Get<TOut>(Specification<TEntity> spec, Expression<Func<TEntity, TOut>> transform);
    Task<IEnumerable<TOut>> GetAll<TOut>(Specification<TEntity> spec, Expression<Func<TEntity, TOut>> transform);
}

public interface IWriteRepository<TEntity>
{
    Task Create(TEntity entity);
    Task Update(TEntity entity);
    Task Delete(TEntity entity);
    Task Delete(Specification<TEntity> spec);
}

public class AddressRepository : IReadOnlyRepository<Address>, IWriteRepository<Address>
{
    private readonly AddressContext _dbContext;

    public AddressRepository(AddressContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<bool> Any(Specification<Address> spec)
    {
        return await _dbContext.Address.AnyAsync(spec.Expression).ConfigureAwait(false);
    }

    public async Task Create(Address entity)
    {
        _dbContext.Address.Add(entity);

        await _dbContext.SaveChangesAsync().ConfigureAwait(false);
    }

    public async Task Delete(Address entity)
    {
        var contextEntity = _dbContext.Attach(entity);
        contextEntity.State = EntityState.Deleted;
        await _dbContext.SaveChangesAsync().ConfigureAwait(false);
    }

    public async Task Delete(Specification<Address> spec)
    {
        await foreach (var address in _dbContext.Address.Where(spec.Expression).AsAsyncEnumerable().ConfigureAwait(false))
        {
            _dbContext.Remove(address);
        }
        await _dbContext.SaveChangesAsync().ConfigureAwait(false);
        //await _dbContext.Address.Where(spec.Expression).ExecuteDeleteAsync().ConfigureAwait(false);
    }

    public async Task<Address?> Get(Specification<Address> spec)
    {
        return await _dbContext.Address.AsNoTracking().FirstOrDefaultAsync(spec.Expression).ConfigureAwait(false); ;
    }

    public async Task<TOut?> Get<TOut>(Specification<Address> spec, Expression<Func<Address, TOut>> transform)
    {
        return await _dbContext.Address.AsNoTracking().Where(spec.Expression).Select(transform).FirstOrDefaultAsync().ConfigureAwait(false);
    }

    public async Task<IEnumerable<Address>> GetAll(Specification<Address>? spec = null)
    {
        var queryable = _dbContext.Address.AsNoTracking();
        if (spec != null) queryable = queryable.Where(spec.Expression);
        return await queryable.ToArrayAsync().ConfigureAwait(false); ;
    }

    public async Task<IEnumerable<TOut>> GetAll<TOut>(Specification<Address> spec, Expression<Func<Address, TOut>> transform)
    {
        return await _dbContext.Address.AsNoTracking().Where(spec.Expression).Select(transform).ToArrayAsync().ConfigureAwait(false);
    }

    public async Task Update(Address entity)
    {
        _dbContext.Attach(entity).State = EntityState.Modified;
        await _dbContext.SaveChangesAsync().ConfigureAwait(false);
    }
}
