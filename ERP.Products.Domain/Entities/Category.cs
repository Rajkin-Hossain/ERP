using ERP.Products.Domain.ValueObjects;
using ERP.Shared.Domain.Entities;

namespace ERP.Products.Domain.Entities;

public sealed class Category : Entity<CategoryId>
{
    public CategoryName CategoryName { get; private set; }

    private Category() { } // For EF Core

    private Category(CategoryId id, CategoryName name)
    {
        Id = id;
        CategoryName = name;
    }

    // "Add Category" / Create
    public static Category Create(CategoryName categoryName)
    {
        return new Category(CategoryId.New(), categoryName);
    }
}






