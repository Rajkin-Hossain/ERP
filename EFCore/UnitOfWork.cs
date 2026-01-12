using ERP.SharedKernal.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EFCore;

public class UnitOfWork(DbContext dbContext) : IUnitOfWork
{
    public Task<int> SaveChangesAsync(CancellationToken ct = default)
        => dbContext.SaveChangesAsync(ct);
}
