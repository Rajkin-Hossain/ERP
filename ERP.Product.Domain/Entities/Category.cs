using ERP.Product.Domain.ValueObjects;
using ERP.SharedKernal;
using ERP.SharedKernal.Entities;

namespace ERP.Product.Domain.Entities;

public class Category : Entity
{
    public CategoryName CategoryName { get; private set; }

    private Category(Guid id, CategoryName name)
    {
        Id = id;
        CategoryName = name;
    }

    // "Add Category" / Create
    public static Category Create(Guid id, CategoryName categoryName)
    {
        if (id == Guid.Empty) throw new DomainException("Category id is required.");

        return new Category(id, categoryName);
    }
}
