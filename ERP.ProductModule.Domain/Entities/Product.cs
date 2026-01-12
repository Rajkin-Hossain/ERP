using ERP.ProductModule.Domain.ValueObjects;
using ERP.SharedKernal;
using ERP.SharedKernal.Entities;

namespace ERP.ProductModule.Domain.Entities;

public class Product : AggregateRoot
{
    public ProductName ProductName { get; private set; }
    public CategoryId CategoryId { get; private set; }
    public ImageUrl ImageUrl { get; private set; }
    public Price Price { get; private set; }

    private Product(
        Guid id,
        ProductName productName,
        CategoryId categoryId,
        ImageUrl imageUrl,
        Price price)
    {
        Id = id;
        ProductName = productName;
        CategoryId = categoryId;
        ImageUrl = imageUrl;
        Price = price;
    }

    // "Add Product" / Create
    public static Product Create(
        Guid id,
        ProductName productName,
        CategoryId categoryId,
        ImageUrl imageUrl,
        Price price)
    {
        if (id == Guid.Empty) throw new DomainException("Product id is required.");
        return new Product(id, productName, categoryId, imageUrl, price);
    }

    // "Update Product"
    public void UpdateDetails(
        ProductName productName,
        CategoryId categoryId,
        ImageUrl imageUrl,
        Price price)
    {
        ProductName = productName;
        CategoryId = categoryId;
        ImageUrl = imageUrl;
        Price = price;
    }

    // "UpdatePrice"
    public void UpdatePrice(Price newPrice)
    {
        // Example rule: cannot set same price (optional)
        if (newPrice.Amount == Price.Amount && newPrice.Currency == Price.Currency)
            return;

        Price = newPrice;
    }
}
